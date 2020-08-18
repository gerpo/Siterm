using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Serilog;
using Siterm.Signature.Exceptions;
using Siterm.Signature.Resources;
using wgssSTU;
using Rectangle = System.Drawing.Rectangle;

namespace Siterm.Signature.Services
{
    public class SignatureService : IDisposable
    {
        private readonly ILogger _logger;
        private IUsbDevice _usbDevice;

        private Bitmap _bitmap; // This bitmap that we display on the screen.
        private byte[] _bitmapData; // This is the flattened data of the bitmap that we send to the device.

        private Button[] _buttons; // The array of buttons that we are emulating.
        private ICapability _capability;

        private encodingMode _encodingMode; // How we send the bitmap to the device.

        // The isDown flag is used like this:
        // 0 = up
        // +ve = down, pressed on button number
        // -1 = down, inking
        // -2 = down, ignoring
        private int _isDown;

        // Array of data being stored. This can be subsequently used as desired.
        private List<IPenData> _penData = new List<IPenData>();

        private Pen _penInk; // cached object.

        private ProtocolHelper _protocolHelper = new ProtocolHelper();
        private Tablet _tablet;
        private bool _useColor;

        public SignatureService(ILogger logger)
        {
            _logger = logger;
        }

        public Bitmap Signature { get; private set; }

        public void Dispose()
        {
            // Ensure that you correctly disconnect from the tablet, otherwise you are
            // likely to get errors when wanting to connect a second time.
            if (_tablet == null) return;
            _tablet.onPenData -= OnPenData;
            _tablet.onGetReportException -= OnGetReportException;
            _tablet.setInkingMode(0x00);
            _tablet.setClearScreen();
            _tablet.disconnect();

            _bitmap?.Dispose();
            _penInk?.Dispose();
        }

        public event EventHandler<SignatureEventArgs> SignatureCreated;

        protected virtual void OnSignatureCreated(Bitmap signature)
        {
            SignatureCreated?.Invoke(this,
                new SignatureEventArgs {Signature = signature});
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearScreen();
            _penData = null;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (_penData.Count != 0)
                ClearScreen();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Save the image.
            Signature = GetImage(new Rectangle(0, 0, _capability.screenWidth, _capability.screenHeight));

            // Fire Event and disconnect
            OnSignatureCreated(Signature);
        }

        private void CalculateEncoding()
        {
            var idP = _tablet.getProductId();
            var encodingFlag = (encodingFlag) _protocolHelper.simulateEncodingFlag(idP);
            if ((encodingFlag & (encodingFlag.EncodingFlag_16bit | encodingFlag.EncodingFlag_24bit)) != 0)
                if (_tablet.supportsWrite())
                    _useColor = true;
            if ((encodingFlag & encodingFlag.EncodingFlag_24bit) != 0)
                _encodingMode = _tablet.supportsWrite()
                    ? encodingMode.EncodingMode_24bit_Bulk
                    : encodingMode.EncodingMode_24bit;
            else if ((encodingFlag & encodingFlag.EncodingFlag_16bit) != 0)
                _encodingMode = _tablet.supportsWrite()
                    ? encodingMode.EncodingMode_16bit_Bulk
                    : encodingMode.EncodingMode_16bit;
            else
                _encodingMode = encodingMode.EncodingMode_1bit;
        }

        private void ClearScreen()
        {
            // note: There is no need to clear the tablet screen prior to writing an image.
            _tablet.writeImage((byte) _encodingMode, _bitmapData);

            _penData.Clear();
            _isDown = 0;
        }

        private void ConnectTablet()
        {
            _tablet = new Tablet();

            var errorCode = _tablet.usbConnect(_usbDevice, true);
            if (errorCode.value == 0)
                _capability = _tablet.getCapability();
            else
                throw new Exception(errorCode.message);
        }

        private void CreateAndTransferBitmap(string name)
        {
            // Size the bitmap to the size of the LCD screen.
            _bitmap = new Bitmap(_capability.screenWidth, _capability.screenHeight, PixelFormat.Format32bppArgb);
            {
                var gfx = Graphics.FromImage(_bitmap);
                gfx.Clear(Color.White);

                // Uses pixels for units as DPI won't be accurate for tablet LCD.
                var font = new Font(FontFamily.GenericSansSerif, _buttons[0].Bounds.Height / 2F,
                    GraphicsUnit.Pixel);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                gfx.TextRenderingHint = _useColor
                    ? TextRenderingHint.AntiAliasGridFit
                    : TextRenderingHint.SingleBitPerPixel;

                //Draw the name
                var upperName = new CultureInfo("de-De").TextInfo.ToTitleCase(name);
                var size = new PointF(10 + gfx.MeasureString(upperName, font).Width / 2, 10);
                //gfx.MeasureString(upperName, font);
                gfx.DrawString(upperName, font, Brushes.Black, size, sf);

                // Draw the buttons
                for (var i = 0; i < _buttons.Length; ++i)
                {
                    if (_useColor)
                        gfx.FillRectangle(Brushes.LightGray, _buttons[i].Bounds);
                    gfx.DrawRectangle(Pens.Black, _buttons[i].Bounds);
                    gfx.DrawString(_buttons[i].Text, font, Brushes.Black, _buttons[i].Bounds, sf);
                }

                sf.Dispose();
                gfx.Dispose();
                font.Dispose();
            }

            // Now the bitmap has been created, it needs to be converted to device-native
            // format.
            {
                // Unfortunately it is not possible for the native COM component to
                // understand .NET bitmaps. We have therefore convert the .NET bitmap
                // into a memory blob that will be understood by COM.

                var stream = new MemoryStream();
                _bitmap.Save(stream, ImageFormat.Png);
                _bitmapData =
                    (byte[])
                    _protocolHelper.resizeAndFlatten(stream.ToArray(), 0, 0, (uint) _bitmap.Width,
                        (uint) _bitmap.Height, _capability.screenWidth, _capability.screenHeight,
                        (byte) _encodingMode, Scale.Scale_Fit, 0, 0);
                _protocolHelper = null;
                stream.Dispose();
            }
        }

        private void CreateButtons()
        {
            _buttons = new Button[3];
            // Place the buttons across the bottom of the screen.

            var w2 = _capability.screenWidth / 3;
            var w3 = _capability.screenWidth / 3;
            var w1 = _capability.screenWidth - w2 - w3;
            var y = _capability.screenHeight * 6 / 7;
            var h = _capability.screenHeight - y;

            _buttons[0].Bounds = new Rectangle(0, y, w1, h);
            _buttons[1].Bounds = new Rectangle(w1, y, w2, h);
            _buttons[2].Bounds = new Rectangle(w1 + w2, y, w3, h);

            _buttons[0].Text = UiStrings.OkBtnLabel;
            _buttons[1].Text = UiStrings.DeleteBtnLabel;
            _buttons[2].Text = UiStrings.CancelBtnLabel;
            _buttons[0].Click = BtnOk_Click;
            _buttons[1].Click = BtnClear_Click;
            _buttons[2].Click = BtnCancel_Click;
        }

        // Draw an image with the existed points.
        private Bitmap GetImage(Rectangle rect)
        {
            using var bitmap = new Bitmap(rect.Width, rect.Height);
            try
            {
                var graphics = Graphics.FromImage(bitmap);

                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                using var brush = new SolidBrush(Color.White);
                graphics.FillRectangle(brush, 0, 0, rect.Width, rect.Height);

                for (var i = 1; i < _penData.Count; i++)
                {
                    PointF p1 = TabletToScreen(_penData[i - 1]);
                    PointF p2 = TabletToScreen(_penData[i]);

                    if (_penData[i - 1].sw > 0 || _penData[i].sw > 0)
                        graphics.DrawLine(_penInk, p1, p2);
                }

                var nb = ImageTrim(bitmap);
                nb.MakeTransparent(Color.White);
                return nb;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Signature service was not able to create bitmap.");
            }

            return bitmap;
        }

        private void SetupUsbDevice()
        {
            var usbDevices = new UsbDevices();
            if (usbDevices.Count != 0)
                try
                {
                    _usbDevice = usbDevices[0]; // select a device
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error with Signature-Pad.");
                }
            else
                throw new NoSignaturePadException();
        }

        public void GetSignature(string name)
        {
            try
            {
                SetupUsbDevice();
                ConnectTablet();
                if (_buttons == null) CreateButtons();
                CalculateEncoding();
                CreateAndTransferBitmap(name);

                _penInk = new Pen(Color.Black, 2.7F);
                _penInk.StartCap = _penInk.EndCap = LineCap.Round;
                _penInk.LineJoin = LineJoin.Round;

                // Add the delegate that receives pen data.
                _tablet.onPenData += OnPenData;
                _tablet.onGetReportException += OnGetReportException;

                // Initialize the screen
                ClearScreen();

                // Enable the pen data on the screen (if not already)
                _tablet.setInkingMode(0x01);
            }
            catch (NoSignaturePadException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while setting up for signature.");
            }
        }

        private static Bitmap ImageTrim(Bitmap img)
        {
            //get image data
            var bd = img.LockBits(new Rectangle(Point.Empty, img.Size),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var rgbValues = new int[img.Height * img.Width];
            Marshal.Copy(bd.Scan0, rgbValues, 0, rgbValues.Length);
            img.UnlockBits(bd);

            #region determine bounds

            var left = bd.Width;
            var top = bd.Height;
            var right = 0;
            var bottom = 0;

            //determine top
            for (var i = 0; i < rgbValues.Length; i++)
            {
                var color = rgbValues[i] & 0xffffff;
                if (color == 0xffffff) continue;
                var r = i / bd.Width;
                var c = i % bd.Width;

                if (left > c)
                    left = c;
                if (right < c)
                    right = c;
                bottom = r;
                top = r;
                break;
            }

            //determine bottom
            for (var i = rgbValues.Length - 1; i >= 0; i--)
            {
                var color = rgbValues[i] & 0xffffff;
                if (color == 0xffffff) continue;
                var r = i / bd.Width;
                var c = i % bd.Width;

                if (left > c)
                    left = c;
                if (right < c)
                    right = c;
                bottom = r;
                break;
            }

            if (bottom > top)
                for (var r = top + 1; r < bottom; r++)
                {
                    //determine left
                    for (var c = 0; c < left; c++)
                    {
                        var color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color == 0xffffff) continue;
                        if (left <= c) continue;
                        left = c;
                        break;
                    }

                    //determine right
                    for (var c = bd.Width - 1; c > right; c--)
                    {
                        var color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color == 0xffffff) continue;
                        if (right >= c) continue;
                        right = c;
                        break;
                    }
                }

            var width = right - left + 1;
            var height = bottom - top + 1;

            #endregion determine bounds

            //copy image data
            var imgData = new int[width * height];
            for (var r = top; r <= bottom; r++)
                Array.Copy(rgbValues, r * bd.Width + left, imgData, (r - top) * width, width);

            //create new image
            var newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var nbd
                = newImage.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(imgData, 0, nbd.Scan0, imgData.Length);
            newImage.UnlockBits(nbd);

            return newImage;
        }

        private void OnGetReportException(ITabletEventsException tabletEventsException)
        {
            try
            {
                tabletEventsException.getException();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Signature-Pad reported an exception.");
                _tablet.disconnect();
                _tablet = null;
                _penData = null;
            }
        }

        private void OnPenData(IPenData penData) // Process incoming pen data
        {
            var pt = TabletToScreen(penData);

            var btn = 0; // will be +ve if the pen is over a button.
            {
                for (var i = 0; i < _buttons.Length; ++i)
                    if (_buttons[i].Bounds.Contains(pt))
                    {
                        btn = i + 1;
                        break;
                    }
            }

            var isDown = penData.sw != 0;

            // This code uses a model of four states the pen can be in:
            // down or up, and whether this is the first sample of that state.

            if (isDown)
            {
                if (_isDown == 0)
                    if (btn > 0)
                        _isDown = btn;
                    else
                        _isDown = -1;

                // The pen is down, store it for use later.
                if (_isDown == -1)
                    _penData.Add(penData);
            }
            else
            {
                if (_isDown != 0)
                {
                    // transition to up
                    if (btn > 0)
                        if (btn == _isDown)
                            _buttons[btn - 1].PerformClick();
                    _isDown = 0;
                }

                // Add up data once we have collected some down data.
                if (_penData.Count != 0)
                    _penData.Add(penData);
            }
        }

        private Point TabletToScreen(IPenData penData)
        {
            // Screen means LCD screen of the tablet.
            return
                Point.Round(new PointF((float) penData.x * _capability.screenWidth / _capability.tabletMaxX,
                    (float) penData.y * _capability.screenHeight / _capability.tabletMaxY));
        }

        private struct Button
        {
            public Rectangle Bounds; // in Screen coordinates
            public string Text;
            public EventHandler Click;

            public void PerformClick()
            {
                Click(this, null);
            }
        }
    }

    public class SignatureEventArgs : EventArgs
    {
        public Bitmap Signature { get; set; }
    }
}
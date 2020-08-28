using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using iText.Forms;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Serilog;
using Siterm.Domain.Models;
using Siterm.Instructions.Models;
using Siterm.Settings.Models;
using Siterm.Settings.Services;

namespace Siterm.Instructions.Services
{
    public class InstructionPdfService
    {
        private readonly string _instructionsArchiveFolder;
        private readonly string _instructionsFolder;
        private readonly ILogger _logger;
        private readonly string _templateFilePath;

        public InstructionPdfService(SettingsProvider settingsProvider, ILogger logger)
        {
            _logger = logger;
            _templateFilePath = settingsProvider.GetSetting(SettingName.InstructionTemplateFile).Value;
            _instructionsFolder = settingsProvider.GetSetting(SettingName.InstructionFolderName).Value;
            _instructionsArchiveFolder = settingsProvider.GetSetting(SettingName.InstructionArchiveFolderName).Value;
        }

        public void ArchiveOlderInstructions(IEnumerable<Instruction> oldInstructions)
        {
            foreach (var oldInstruction in oldInstructions)
            {
                var oldFileInfo = new FileInfo(oldInstruction.Path);
                var archivePath = Path.Combine(oldFileInfo.DirectoryName ?? string.Empty, _instructionsArchiveFolder);

                if (!oldFileInfo.Exists) continue;
                if (!Directory.Exists(archivePath)) Directory.CreateDirectory(archivePath);

                oldFileInfo.MoveTo(Path.Combine(archivePath, oldFileInfo.Name), true);
            }
        }

        public IList<string> CreateInstructionPdf(InstructionDraft instructionDraft)
        {
            var device = instructionDraft.Device;
            var instructionFolderPath = GetDeviceInstructionsPath(device);

            return (from userDraft in instructionDraft.Instructed
                let instructionPath = Path.Combine(instructionFolderPath, userDraft.FileName)
                select FillAndSavePdf(userDraft, instructionDraft, instructionPath)).ToList();
        }

        private string FillAndSavePdf(UserDraft userDraft, InstructionDraft instructionDraft, string instructionPath)
        {
            try
            {
                using var reader = new PdfReader(_templateFilePath);
                using var writer = new PdfWriter(instructionPath);
                using var pdf = new PdfDocument(reader, writer);

                var form = PdfAcroForm.GetAcroForm(pdf, true);
                var doc = new Document(pdf);

                form.SetGenerateAppearance(true);

                form.GetField("deviceName").SetValue(instructionDraft.Device.Name);
                form.GetField("deviceChief").SetValue(instructionDraft.Device.Chief?.FullName ?? string.Empty);
                form.GetField("onlyFor").SetValue(instructionDraft.OnlyFor ?? string.Empty);
                form.GetField("notFor").SetValue(instructionDraft.ExceptFor ?? string.Empty);
                form.GetField("instructor").SetValue(instructionDraft.Instructor.FullName);
                form.GetField("instructed").SetValue(userDraft.FullName);
                form.GetField("instructionDate").SetValue(DateTime.Today.ToString("d", CultureInfo.CurrentCulture));

                //var signatureImage = PositionImage(form, "instructorSignature", instructionDraft.Instructor.Signature);
                //var instructedImage = PositionImage(form, "instructedSignature", userDraft.Signature);

                //doc.Add(signatureImage);
                //doc.Add(instructedImage);
                form.RemoveField("instructorSignature");
                form.RemoveField("instructedSignature");
                doc.Close();
                pdf.Close();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Instruction pdf could not be filled or saved.");
                if (File.Exists(instructionPath)) File.Delete(instructionPath);
                return null;
            }

            return instructionPath;
        }

        private string GetDeviceInstructionsPath(Device device)
        {
            var instructionFolderPath = Path.Combine(device.Path, _instructionsFolder);

            if (!Directory.Exists(instructionFolderPath)) Directory.CreateDirectory(instructionFolderPath);

            return instructionFolderPath;
        }

        private static Image PositionImage(PdfAcroForm pdfAcroForm, string fieldName, System.Drawing.Image image)
        {
            var field = pdfAcroForm.GetField(fieldName).GetWidgets()[0];
            var fieldRect = field.GetRectangle();

            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.MemoryBmp);
            var signatureImageData = ImageDataFactory.Create(ms.ToArray());
            var width = fieldRect.GetAsNumber(2).FloatValue() -
                        fieldRect.GetAsNumber(0).FloatValue();
            var height = fieldRect.GetAsNumber(3).FloatValue() -
                         fieldRect.GetAsNumber(1).FloatValue();
            var signatureImage = new Image(signatureImageData)
                .ScaleToFit(width, height).SetFixedPosition(fieldRect.GetAsNumber(0).FloatValue(),
                    fieldRect.GetAsNumber(1).FloatValue());
            return signatureImage;
        }
    }
}
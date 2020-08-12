using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Siterm.Domain.Models;

namespace Siterm.WPF.Controls
{
    public partial class DeviceInfoPanel : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty DeviceProperty = DependencyProperty.Register(
            "Device",
            typeof(Device),
            typeof(DeviceInfoPanel),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DevicePropertyChanged)
        );

        public DeviceInfoPanel()
        {
            InitializeComponent();
        }

        public Device Device
        {
            get => (Device) GetValue(DeviceProperty);
            set => SetValue(DeviceProperty, value);
        }

        public bool HasDevice => !(Device is null);

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private static void DevicePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DeviceInfoPanel) d).NotifyPropertyChanged("Device");
            ((DeviceInfoPanel) d).NotifyPropertyChanged("HasDevice");
        }
    }
}
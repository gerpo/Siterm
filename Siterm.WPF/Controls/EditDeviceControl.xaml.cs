using System.Windows;
using Siterm.Domain.Models;

namespace Siterm.WPF.Controls
{
    public partial class EditDeviceControl
    {
        public static readonly DependencyProperty DeviceProperty = DependencyProperty.Register("Device", typeof(Device),
            typeof(EditDeviceControl), new PropertyMetadata(default(Device)));

        public EditDeviceControl()
        {
            InitializeComponent();
        }

        public Device Device
        {
            get => (Device) GetValue(DeviceProperty);
            set => SetValue(DeviceProperty, value); 
        }
    }
}
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Siterm.Domain.Models;
using Siterm.Support.Misc;

namespace Siterm.WPF.Controls
{
    public partial class DeviceInfoPanel : INotifyPropertyChanged
    {
        public static readonly DependencyProperty DeviceProperty = DependencyProperty.Register(
            "Device",
            typeof(Device),
            typeof(DeviceInfoPanel),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DevicePropertyChanged)
        );
        public static readonly DependencyProperty NewInstructionCommandProperty = DependencyProperty.Register(
            "NewInstructionCommand",
            typeof(RelayCommand),
            typeof(DeviceInfoPanel)
        );

        public static readonly DependencyProperty NewServiceReportCommandProperty = DependencyProperty.Register(
            "NewServiceReportCommand",
            typeof(RelayCommand),
            typeof(DeviceInfoPanel)
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

        public RelayCommand NewInstructionCommand
        {
            get => (RelayCommand)GetValue(NewInstructionCommandProperty);
            set => SetValue(NewInstructionCommandProperty, value);
        }
        public RelayCommand NewServiceReportCommand
        {
            get => (RelayCommand)GetValue(NewServiceReportCommandProperty);
            set => SetValue(NewServiceReportCommandProperty, value);
        }
        public bool HasDevice => !(Device is null);

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private static void DevicePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DeviceInfoPanel) d).NotifyPropertyChanged(nameof(Device));
            ((DeviceInfoPanel) d).NotifyPropertyChanged(nameof(HasDevice));
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ContentVerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DataGridRow dataGridRow)) return;
            switch (dataGridRow.DataContext)
            {
                case Instruction instruction:
                    Helper.OpenFile(instruction.Path);
                    break;
                case ServiceReport serviceReport:
                    Helper.OpenFile(serviceReport.Path);
                    break;
            }
        }
    }
}
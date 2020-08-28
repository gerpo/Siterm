using System.Collections.Generic;
using System.Windows;
using Siterm.Domain.Models;
using Siterm.Instructions.Models;
using Siterm.Support.Misc;

namespace Siterm.WPF.Controls
{
    public partial class CreateInstructionControl
    {
        public static readonly DependencyProperty DeviceListProperty = DependencyProperty.Register(
            nameof(DeviceList),
            typeof(IEnumerable<Device>),
            typeof(CreateInstructionControl)
        );

        public static readonly DependencyProperty UserEmailListProperty = DependencyProperty.Register(
            nameof(UserEmailList),
            typeof(IEnumerable<string>),
            typeof(CreateInstructionControl)
        );

        public static readonly DependencyProperty SelectedDeviceProperty = DependencyProperty.Register(
            nameof(SelectedDevice),
            typeof(Device),
            typeof(CreateInstructionControl),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedDevicePropertyChanged)
        );

        public static readonly DependencyProperty SelectionLimitedProperty = DependencyProperty.Register(
            nameof(SelectionLimited),
            typeof(bool),
            typeof(CreateInstructionControl)
        );

        public static readonly DependencyProperty OnlyForProperty = DependencyProperty.Register(
            nameof(OnlyFor),
            typeof(string),
            typeof(CreateInstructionControl)
        );

        public static readonly DependencyProperty ExceptForProperty = DependencyProperty.Register(
            nameof(ExceptFor),
            typeof(string),
            typeof(CreateInstructionControl)
        );

        public static readonly DependencyProperty InstructionDraftProperty = DependencyProperty.Register(
            nameof(InstructionDraft),
            typeof(InstructionDraft),
            typeof(CreateInstructionControl)
        );

        public static readonly DependencyProperty CreateInstructionCommandProperty = DependencyProperty.Register(
            nameof(CreateInstructionCommand),
            typeof(RelayCommand),
            typeof(CreateInstructionControl)
        );

        public CreateInstructionControl()
        {
            InitializeComponent();
        }

        public IEnumerable<Device> DeviceList
        {
            get => (IEnumerable<Device>) GetValue(DeviceListProperty);
            set => SetValue(DeviceListProperty, value);
        }

        public IEnumerable<string> UserEmailList
        {
            get => (IEnumerable<string>) GetValue(UserEmailListProperty);
            set => SetValue(UserEmailListProperty, value);
        }

        public Device SelectedDevice
        {
            get => (Device) GetValue(SelectedDeviceProperty);
            set => SetValue(SelectedDeviceProperty, value);
        }

        public bool SelectionLimited
        {
            get => (bool) GetValue(SelectionLimitedProperty);
            set => SetValue(SelectionLimitedProperty, value);
        }

        public string OnlyFor
        {
            get => (string) GetValue(OnlyForProperty);
            set => SetValue(OnlyForProperty, value);
        }

        public string ExceptFor
        {
            get => (string) GetValue(ExceptForProperty);
            set => SetValue(ExceptForProperty, value);
        }

        public InstructionDraft InstructionDraft
        {
            get => (InstructionDraft) GetValue(InstructionDraftProperty);
            set => SetValue(InstructionDraftProperty, value);
        }

        public RelayCommand CreateInstructionCommand
        {
            get => (RelayCommand) GetValue(CreateInstructionCommandProperty);
            set => SetValue(CreateInstructionCommandProperty, value);
        }

        private void AddInstructedBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (InstructionDraft.Instructed is null || InstructionDraft.Instructed.Count >= 10) return;
            InstructionDraft.Instructed.Add(new UserDraft());
        }

        private void RemoveInstructedBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (InstructionDraft.Instructed is null || InstructionDraft.Instructed.Count <= 1) return;
            InstructionDraft.Instructed.RemoveAt(InstructionDraft.Instructed.Count - 1);
        }

        private static void SelectedDevicePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Siterm.Support.Misc;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF.Views
{
    public partial class CreateInstructionView : MetroWindow, IActivable
    {
        public CreateInstructionView(CreateInstructionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
        }

        public async Task ActivateAsync(object parameter)
        {
            var requestedDeviceId = parameter is int i ? i : -1;
            await ((CreateInstructionViewModel) DataContext).FetchDevices(requestedDeviceId);
            await ((CreateInstructionViewModel) DataContext).FetchUsers();

            ((CreateInstructionViewModel)DataContext).ClosingRequest += OnClosingRequest;

            return;
        }

        private void OnClosingRequest(object? sender, EventArgs e)
        {
            Close();
        }
    }
}   
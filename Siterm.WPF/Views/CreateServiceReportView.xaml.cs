#nullable enable
using System;
using System.Threading.Tasks;
using Siterm.Support.Misc;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF.Views
{
    public partial class CreateServiceReportView : IActivable
    {
        public CreateServiceReportView(CreateServiceReportViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public async Task ActivateAsync(object parameter)
        {
            var requestedDeviceId = parameter is int i ? i : -1;
            await ((CreateServiceReportViewModel) DataContext).FetchDevices(requestedDeviceId);
            //await ((CreateServiceReportViewModel) DataContext).FetchUsers();

            ((CreateServiceReportViewModel) DataContext).ClosingRequest += OnClosingRequest;
        }

        private void OnClosingRequest(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
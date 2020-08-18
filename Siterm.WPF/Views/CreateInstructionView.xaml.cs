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

        public Task ActivateAsync(object parameter)
        {
            var requestedDeviceId = parameter is int i ? i : -1;
            ((CreateInstructionViewModel) DataContext).FetchDevices(requestedDeviceId);
            ((CreateInstructionViewModel) DataContext).FetchUsers();
            return Task.CompletedTask;
        }
    }
}   
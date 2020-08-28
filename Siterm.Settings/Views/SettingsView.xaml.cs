using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Siterm.Settings.ViewModels;
using Siterm.Support.Misc;

namespace Siterm.Settings.Views
{
    public partial class SettingsView : MetroWindow, IActivable
    {
        public SettingsView(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            return Task.CompletedTask;
        }
    }
}
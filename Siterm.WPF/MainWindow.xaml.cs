using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Siterm.Support.Misc;
using Siterm.WPF.State.Navigators;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF
{
    public partial class MainWindow : MetroWindow, IActivable
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            MainTabControl.SelectedIndex = 0;
        }

        public Task ActivateAsync(object parameter)
        {
            return Task.CompletedTask;
        }
    }
}
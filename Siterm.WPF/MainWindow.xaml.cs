using System.Threading.Tasks;
using Siterm.Support.Misc;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF
{
    public partial class MainWindow : IActivable
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
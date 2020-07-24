using MahApps.Metro.Controls;

namespace Siterm.WPF
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainTabControl.SelectedIndex = 0;
        }
    }
}
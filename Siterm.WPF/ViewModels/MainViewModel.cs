using System.Collections.ObjectModel;
using Siterm.Settings.ViewModels;
using Siterm.Settings.Views;
using Siterm.Support.Misc;
using Siterm.WPF.State.Navigators;

namespace Siterm.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly SettingsView _settingsView;
        public INavigator Navigator { get; set; } = new Navigator();

        public ObservableCollection<ITabItemViewModel> TabItems { get; }

        public RelayCommand OpenSettingsCommand { get; }

        public MainViewModel(TabViewModelCollectionFactory tabViewModelCollectionFactory, SettingsView settingsView)
        {
            _settingsView = settingsView;
            TabItems = tabViewModelCollectionFactory.TabItemViewModelCollection;

            OpenSettingsCommand = new RelayCommand(OpenSettings);
        }

        private void OpenSettings(object obj)
        {
            _settingsView.Show();
        }
    }
}
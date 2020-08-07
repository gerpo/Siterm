using System.Collections.ObjectModel;
using Siterm.Settings.Views;
using Siterm.Support.Misc;
using Siterm.WPF.State.Navigators;

namespace Siterm.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly SimpleNavigationService _navigationService;

        public MainViewModel(TabViewModelCollectionFactory tabViewModelCollectionFactory,
            SimpleNavigationService navigationService)
        {
            _navigationService = navigationService;
            TabItems = tabViewModelCollectionFactory.TabItemViewModelCollection;

            OpenSettingsCommand = new RelayCommand(OpenSettings);
        }

        public ObservableCollection<ITabItemViewModel> TabItems { get; }

        public RelayCommand OpenSettingsCommand { get; }

        private async void OpenSettings(object obj)
        {
            await _navigationService.ShowAsync<SettingsView>();
        }
    }
}
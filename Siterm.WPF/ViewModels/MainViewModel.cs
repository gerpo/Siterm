using System.Collections.ObjectModel;
using Siterm.WPF.State.Navigators;

namespace Siterm.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public INavigator Navigator { get; set; } = new Navigator();

        public ObservableCollection<ITabItemViewModel> TabItems { get; }

        public MainViewModel(TabViewModelCollectionFactory tabViewModelCollectionFactory)
        {
            TabItems = tabViewModelCollectionFactory.TabItemViewModelCollection;
        }
    }
}
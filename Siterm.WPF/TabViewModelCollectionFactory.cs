using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF
{
    public class TabViewModelCollectionFactory
    {
        public TabViewModelCollectionFactory(IEnumerable<ITabItemViewModel> tabItemViewModels)
        {
            foreach (var tabItemViewModel in tabItemViewModels.OrderBy(t => t.Position))
                TabItemViewModelCollection.Add(tabItemViewModel);
        }

        public ObservableCollection<ITabItemViewModel> TabItemViewModelCollection { get; } =
            new ObservableCollection<ITabItemViewModel>();
    }
}
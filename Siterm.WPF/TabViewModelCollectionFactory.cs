using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF
{
    public class TabViewModelCollectionFactory
    {
        public ObservableCollection<ITabItemViewModel> TabItemViewModelCollection { get; } = new ObservableCollection<ITabItemViewModel>();

        public TabViewModelCollectionFactory(IEnumerable<ITabItemViewModel> tabItemViewModels)
        {
            foreach (var tabItemViewModel in tabItemViewModels.OrderBy(t => t.Position))
            {
                TabItemViewModelCollection.Add(tabItemViewModel);
            }
        }
    }
}

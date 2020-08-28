using System.Windows;
using Siterm.Substance.Models;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF.Views
{
    public partial class SubstanceView
    {
        public SubstanceView()
        {
            InitializeComponent();
        }

        private void SubstanceTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = SubstanceTreeView.SelectedItem;

            if (selectedItem is SubstanceTreeViewItem substanceTreeViewItem)
                ((SubstanceViewModel) DataContext).SelectedSubstanceChanged(substanceTreeViewItem.Model);
        }
    }
}
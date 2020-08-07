using System.Windows;
using System.Windows.Controls;
using Siterm.Substance.Models;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF.Views
{
    /// <summary>
    ///     Interaktionslogik für SubstanceView.xaml
    /// </summary>
    public partial class SubstanceView : UserControl
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
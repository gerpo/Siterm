using System.Windows;
using Siterm.Facility.Models;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF.Views
{
    public partial class FacilityView
    {
        public FacilityView()
        {
            InitializeComponent();
        }

        private void FacilityTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = FacilityTreeView.SelectedItem;

            if (selectedItem is DeviceTreeViewItem deviceTreeViewItem)
                ((FacilityViewModel) DataContext).SelectedDeviceChanged(deviceTreeViewItem.Model);
        }
    }
}
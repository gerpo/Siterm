using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Siterm.Support.ControlModels;

namespace Siterm.Facility.Models
{
    public class FacilityTreeViewItem : GenericTreeViewItem<Domain.Models.Facility>
    {
        private ICollectionView _deviceCollectionView;

        public FacilityTreeViewItem(Domain.Models.Facility facility)
        {
            Model = facility;
            Devices = new ObservableCollection<DeviceTreeViewItem>(
                facility.Devices.Select(d => new DeviceTreeViewItem(d)));
            ApplyFilter(string.Empty);
        }

        public ObservableCollection<DeviceTreeViewItem> Devices { get; }

        public ICollectionView DeviceCollectionView
        {
            get => _deviceCollectionView;
            set => SetField(ref _deviceCollectionView, value);
        }

        public bool ApplyFilter(string filterTerm)
        {
            DeviceCollectionView = CollectionViewSource.GetDefaultView(Devices);

            DeviceCollectionView.Filter = o =>
                string.IsNullOrEmpty(filterTerm) ||
                ((DeviceTreeViewItem) o).Model.Name.Contains(filterTerm,
                    StringComparison.CurrentCultureIgnoreCase);

            return HasRelevantChildren(filterTerm);
        }

        private bool HasRelevantChildren(string filterTerm)
        {
            return string.IsNullOrEmpty(filterTerm) || Devices.Any(d => d.Model.Name.Contains(filterTerm));
        }
    }
}
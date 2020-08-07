using System.Collections.Generic;
using System.Linq;
using Siterm.Support.ControlModels;

namespace Siterm.Facility.Models
{
    public class FacilityTreeViewItem : GenericTreeViewItem<Domain.Models.Facility>
    {
        public FacilityTreeViewItem(Domain.Models.Facility facility)
        {
            Model = facility;
            Devices = facility.Devices.Select(d => new DeviceTreeViewItem(d)).ToList();
        }

        public IList<DeviceTreeViewItem> Devices { get; }
    }
}
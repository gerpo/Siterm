using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using System.Threading.Tasks;

namespace Siterm.DatabaseInitialization.Services
{
    public class DeviceImporter : IImporter
    {
        private readonly DeviceDataService _deviceDataService;
        private readonly FacilityDataService _facilityDataService;
        private readonly IEnumerable<string> _nameFilterList = new string[] { };

        public DeviceImporter(FacilityDataService facilityDataService,
            DeviceDataService deviceDataService)
        {
            _facilityDataService = facilityDataService;
            _deviceDataService = deviceDataService;
        }

        public int Order => 3;

        public async Task Execute()
        {
            var allFacilities = await _facilityDataService.GetAll();
            var oldDevices = await _deviceDataService.GetAll();

            foreach (var facility in allFacilities)
            {
                var dirInfo = new DirectoryInfo(facility.Path);
                var devices = dirInfo.GetDirectories()
                    .Select(deviceInfo => new {deviceInfo, nameSplit = deviceInfo.Name.Split('_')})
                    .Where(d => d.nameSplit.Length == 4 && d.nameSplit[0] == "M" &&
                                int.TryParse(d.nameSplit[1], out _)) // check if the folder name has format of M_NR_NAME
                    .Where(d => !_nameFilterList.Contains(d.deviceInfo.Name)) // check if folder name is in filter list
                    .Select(d =>
                        new Device
                        {
                            Path = d.deviceInfo.FullName,
                            Name = d.nameSplit[2],
                            DeviceNumber = int.Parse(d.nameSplit[1]),
                            Facility = facility
                        })
                    .ToList();

                var filteredList = devices.Where(s => oldDevices.All(os => os.Path != s.Path)).ToList();

                facility.Devices = filteredList;

                await _facilityDataService.Update(facility.Id, facility);
            }
        }
    }
}
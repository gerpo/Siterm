using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Settings.Models;
using Siterm.Settings.Services;

namespace Siterm.DatabaseInitialization.Services
{
    public class FacilityImporter : IImporter
    {
        private readonly FacilityDataService _dataService;
        private readonly string _facilityPath;
        private readonly IEnumerable<string> _nameFilterList = new[] {"YY_Musterordner"};

        public FacilityImporter(SettingsProvider settingsProvider, FacilityDataService dataService)
        {
            _dataService = dataService;
            _facilityPath = settingsProvider.GetSetting(SettingName.FacilityPath).Value;
        }

        public int Order => 2;

        public async void Execute()
        {
            var pathInfo = new DirectoryInfo(_facilityPath);
            if (!pathInfo.Exists) return;

            var oldFacilities = await _dataService.GetAll();

            var facilityList = pathInfo.GetDirectories()
                .Select(facilityInfo => new {facilityInfo, nameSplit = facilityInfo.Name.Split('_')})
                .Where(d => d.nameSplit.Length == 2 &&
                            int.TryParse(d.nameSplit[0], out _)) // check if the folder name has format of NR_NAME
                .Where(d => !_nameFilterList.Contains(d.facilityInfo.Name)) // check if folder name is in filter list
                .Select(d =>
                    new Facility
                        {Path = d.facilityInfo.FullName, Name = d.nameSplit[1], OrderNr = int.Parse(d.nameSplit[0])})
                .ToList();

            var filteredList = facilityList.Where(s => oldFacilities.All(os => os.Path != s.Path)).ToList();

            _dataService.CreateAll(filteredList);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using System.Threading.Tasks;

namespace Siterm.DatabaseInitialization.Services
{
    internal class ServiceReportImporter : IImporter
    {
        private const string Extension = "pdf";
        private readonly DeviceDataService _deviceDataService;
        private readonly IEnumerable<string> _nameFilterList = new string[] { };
        private readonly ServiceReportDataService _serviceReportDataService;
        private readonly string _serviceReportFolderName;

        public ServiceReportImporter(SettingsProvider settingsProvider, DeviceDataService deviceDataService,
            ServiceReportDataService serviceReportDataService)
        {
            _deviceDataService = deviceDataService;
            _serviceReportDataService = serviceReportDataService;

            _serviceReportFolderName = settingsProvider.GetSetting(SettingName.ServiceReportFolderName).Value;
        }

        public int Order => 4;

        public async Task Execute()
        {
            if (string.IsNullOrEmpty(_serviceReportFolderName)) return;

            var allDevices = await _deviceDataService.GetAll();
            var oldInstructions = await _serviceReportDataService.GetAll();

            foreach (var device in allDevices)
            {
                var serviceReportPath = Path.Join(device.Path, _serviceReportFolderName);
                var dirInfo = new DirectoryInfo(serviceReportPath);

                if (!dirInfo.Exists) continue;

                var serviceReports = dirInfo.GetFiles($"*.{Extension}")
                    .Select(reportInfo => new
                        {reportInfo, nameSplit = reportInfo.Name.Replace($".{Extension}", "").Split('_')})
                    .Where(i =>
                        i.nameSplit.Length == 3 && // check if the folder name has format of DEVICENAME_DATE_VALIDITY
                        DateTime.TryParse(i.nameSplit[1], out _) &&
                        ServiceReport.StringValidityMap.ContainsKey(i.nameSplit[2]))
                    .Where(i => !_nameFilterList.Contains(i.reportInfo.Name)) // check if folder name is in filter list
                    .Select(i =>
                        new ServiceReport
                        {
                            Path = i.reportInfo.FullName,
                            CreatedAt = DateTime.Parse(i.nameSplit[1]),
                            Validity = ServiceReport.StringValidityMap[i.nameSplit[2]],
                            Device = device
                        })
                    .ToList();

                var filteredList = serviceReports.Where(s => oldInstructions.All(os => os.Path != s.Path)).ToList();

                device.ServiceReports = filteredList;

                await _deviceDataService.Update(device.Id, device);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.Excel.Services;
using Siterm.ServiceReports.Models;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Support.Misc;
namespace Siterm.ServiceReports.Services
{
    public class ServiceReportService
    {
        private readonly SettingsProvider _settingsProvider;
        private string _serviceReportFolderName;

        public ServiceReportService(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            _serviceReportFolderName = settingsProvider.GetSetting(SettingName.ServiceReportFolderName).Value;
        }

        public IEnumerable<ServiceReportTemplate> GetServiceReportTemplates(Device device)
        {
            var serviceReportFolder = Path.Combine(device.Path, _serviceReportFolderName);
            var serviceReportFolderInfo = new DirectoryInfo(serviceReportFolder);
            if (!serviceReportFolderInfo.Exists) return null;

            var possibleServiceTemplates = serviceReportFolderInfo.GetFiles();
            
            return possibleServiceTemplates.Where(IsServiceTemplate).Select(CreateServiceReportTemplate);
        }

        private ServiceReportTemplate CreateServiceReportTemplate(FileInfo fileInfo)
        {
            return new ServiceReportTemplate
            {
                Validity = GetValidity(fileInfo),
                Path = fileInfo.FullName,
                ServiceTasks = ReadServiceTemplateService.ReadFile(fileInfo)
            };
        }

        private static ServiceReport.ValidityType GetValidity(FileSystemInfo fileInfo)
        {
            var validity = fileInfo.Name.Split('_')[1].Split('.')[0];
            return ServiceReport.StringValidityMap[validity];
        }
        private bool IsServiceTemplate(FileInfo pathInfo)
        {
            return pathInfo.Exists && pathInfo.Directory != null && pathInfo.Directory.FullName.EndsWith(_serviceReportFolderName, StringComparison.CurrentCulture) &&
                   pathInfo.Name.Split('_').Length == 2 && Helper.IsExcel(pathInfo);
        }
    }
}
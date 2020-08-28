using System.Collections.Generic;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.Instructions.Models;
using Siterm.ServiceReports.Services;
using Siterm.Support.Misc;

namespace Siterm.ServiceReports.Models
{
    public class ServiceReportDraft : CanNotifyPropertyChanged
    {
        private readonly ServiceReportService _serviceReportService;
        private Device _device;

        private ServiceReportTemplate _selectedServiceTemplate;

        private IEnumerable<ServiceReportTemplate> _serviceTemplates;

        private UserDraft _serviceWorker;

        public ServiceReportDraft(ServiceReportService serviceReportService)
        {
            _serviceReportService = serviceReportService;
        }

        public Device Device
        {
            get => _device;
            set => OnDeviceChange(value);
        }

        public IEnumerable<ServiceReportTemplate> ServiceTemplates
        {
            get => _serviceTemplates;
            set => SetField(ref _serviceTemplates, value);
        }

        public ServiceReportTemplate SelectedServiceTemplate
        {
            get => _selectedServiceTemplate;
            set => SetField(ref _selectedServiceTemplate, value);
        }

        public UserDraft ServiceWorker
        {
            get => _serviceWorker ??= new UserDraft();
            set => SetField(ref _serviceWorker, value);
        }

        private void OnDeviceChange(Device value)
        {
            SetField(ref _device, value);
            if (value == null) return;
            ServiceTemplates = _serviceReportService.GetServiceReportTemplates(Device);
            SelectedServiceTemplate = ServiceTemplates.FirstOrDefault();
        }
    }
}
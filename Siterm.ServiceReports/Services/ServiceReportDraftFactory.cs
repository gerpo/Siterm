using Siterm.ServiceReports.Models;

namespace Siterm.ServiceReports.Services
{
    public class ServiceReportDraftFactory
    {
        private readonly ServiceReportService _serviceReportService;

        public ServiceReportDraftFactory(ServiceReportService serviceReportService)
        {
            _serviceReportService = serviceReportService;
        }

        public ServiceReportDraft CreateServiceReportDraft()
        {
            return new ServiceReportDraft(_serviceReportService);
        }
    }
}
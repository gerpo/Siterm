using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class ServiceReportDataService : GenericDataService<ServiceReport>
    {
        public ServiceReportDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Siterm.ServiceReports.Services;

namespace Siterm.ServiceReports
{
    public static class ServiceReportServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ServiceReportService>();
            services.AddTransient<ServiceReportDraftFactory>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Siterm.ServiceReports.Services;
using Siterm.Support.Misc;

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
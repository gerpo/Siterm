using Microsoft.Extensions.DependencyInjection;
using Siterm.EntityFramework.Services;

namespace Siterm.EntityFramework
{
    public static class EntityServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<SitermDbContextFactory>();
            services.AddDbContext<SitermDbContext>();
            services.AddTransient<SubstanceDataService>();
            services.AddTransient<FacilityDataService>();
            services.AddTransient<DeviceDataService>();
            services.AddTransient<InstructionDataService>();
            services.AddTransient<ServiceReportDataService>();
            services.AddTransient<UserDataService>();
        }
    }
}
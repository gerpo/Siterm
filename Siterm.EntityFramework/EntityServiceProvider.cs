using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;

namespace Siterm.EntityFramework
{
    public static class EntityServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<SitermDbContextFactory>();
            services.AddDbContext<SitermDbContext>();
            services.AddScoped<SubstanceDataService>();
            services.AddScoped<FacilityDataService>();
            services.AddScoped<DeviceDataService>();
            services.AddScoped<InstructionDataService>();
            services.AddScoped<ServiceReportDataService>();
            services.AddScoped<UserDataService>();
        }
    }
}
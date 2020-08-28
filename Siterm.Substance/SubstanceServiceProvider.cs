using Microsoft.Extensions.DependencyInjection;
using Siterm.Substance.Services;

namespace Siterm.Substance
{
    public static class SubstanceServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<PSentenceFactory>();
            services.AddSingleton<HSentenceFactory>();
            services.AddTransient<SubstanceInfoService>();
        }
    }
}
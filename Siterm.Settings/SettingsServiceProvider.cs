using Microsoft.Extensions.DependencyInjection;
using Siterm.Settings.Services;
using Siterm.Settings.ViewModels;
using Siterm.Settings.Views;

namespace Siterm.Settings
{
    public static class SettingsServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<SettingsProvider>();
            services.AddTransient<SettingsValidator>();
            services.AddTransient<SettingsWriter>();

            services.AddScoped<SettingsViewModel>();

            services.AddTransient<SettingsView>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
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

            services.AddTransient<SettingsViewModel>();

            services.AddTransient(serviceProvider => new SettingsView
                {DataContext = serviceProvider.GetRequiredService(typeof(SettingsViewModel))});
        }
    }
}

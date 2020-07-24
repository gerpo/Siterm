using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Siterm.EntityFramework;
using Siterm.Settings;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Support.Misc;
using Siterm.Support.Services;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceProvider = CreateServiceProvider();

            var window = new MainWindow {DataContext = serviceProvider.GetRequiredService<MainViewModel>()};
            window.Show();

            base.OnStartup(e);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            RegisterLogger(services);
            RegisterConfiguration(services);

            SettingsServiceProvider.RegisterServices(services);

            services.AddSingleton<SitermDbContextFactory>();
            services.AddSingleton<TabViewModelCollectionFactory>();
            services.AddTransient<RtfToFlowConverter>();

            RegisterViewModels(services);
            RegisterTabItemViewModels(services);

            return services.BuildServiceProvider();
        }

        private static void RegisterConfiguration(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            services.Configure<AppSettings>(x => configuration.GetSection(nameof(AppSettings)).Bind(x));
        }

        private static void RegisterLogger(IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(@"logs\siterm_log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddSingleton<ILogger>(logger);

            Log.Logger = logger;
        }

        private static void RegisterTabItemViewModels(IServiceCollection services)
        {
            var viewModels = Assembly.GetAssembly(typeof(ITabItemViewModel))?.GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(ITabItemViewModel)));

            if (viewModels is null) return;

            foreach (var viewModel in viewModels)
                services.Add(new ServiceDescriptor(typeof(ITabItemViewModel), viewModel, ServiceLifetime.Scoped));
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            var viewModels = Assembly.GetAssembly(typeof(MainViewModel))?.GetTypes()
                .Where(t => t.IsClass && t.IsSubclassOf(typeof(BaseViewModel)));

            if (viewModels is null) return;

            foreach (var viewModel in viewModels) services.AddScoped(viewModel);
        }
    }
}
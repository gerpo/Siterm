using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Siterm.EntityFramework;
using Siterm.Excel;
using Siterm.Instructions;
using Siterm.Mail;
using Siterm.ServiceReports;
using Siterm.Settings;
using Siterm.Settings.Models;
using Siterm.Signature;
using Siterm.Substance;
using Siterm.Support.Misc;
using Siterm.Support.Services;
using Siterm.WPF.State.Navigators;
using Siterm.WPF.ViewModels;
using Siterm.WPF.Views;

namespace Siterm.WPF
{
    public partial class App
    {
        private IHost _host;

        protected override void OnExit(ExitEventArgs e)
        {
            _host?.StopAsync();
            _host?.Dispose();

            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            _host = CreateHost();
            var serviceProvider = _host.Services;
            _host.RunAsync();

            var window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        private static void ConfigureServices(HostBuilderContext hostBuilderContext,
            IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<AppSettings>(x =>
                hostBuilderContext.Configuration.GetSection(nameof(AppSettings)).Bind(x));

            RegisterLogger(serviceCollection);
            serviceCollection.AddScoped<SimpleNavigationService>();
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddTransient(_ => DialogCoordinator.Instance);

            SettingsServiceProvider.RegisterServices(serviceCollection);
            EntityServiceProvider.RegisterServices(serviceCollection);
            ExcelServiceProvider.RegisterServices(serviceCollection);
            SignatureServiceProvider.RegisterServices(serviceCollection);
            SubstanceServiceProvider.RegisterServices(serviceCollection);
            InstructionsServiceProvider.RegisterServices(serviceCollection);
            ServiceReportServiceProvider.RegisterServices(serviceCollection);
            MailServiceProvider.RegisterServices(serviceCollection);

            serviceCollection.AddTransient<CreateInstructionView>();
            serviceCollection.AddTransient<CreateServiceReportView>();
            serviceCollection.AddTransient<CreateInstructionViewModel>();
            serviceCollection.AddTransient<CreateServiceReportViewModel>();

            serviceCollection.AddTransient<EditDeviceView>();
            serviceCollection.AddTransient<EditDeviceViewModel>();

            serviceCollection.AddSingleton<TabViewModelCollectionFactory>();
            serviceCollection.AddTransient<RtfToFlowConverter>();

            RegisterViewModels(serviceCollection);
            RegisterTabItemViewModels(serviceCollection);
        }

        private static IHost CreateHost()
        {
            return Host.CreateDefaultBuilder()
                .UseWindowsService()
                .ConfigureAppConfiguration(RegisterConfiguration)
                .ConfigureServices(ConfigureServices).Build();
        }

        private static void RegisterConfiguration(HostBuilderContext hostBuilderContext,
            IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
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
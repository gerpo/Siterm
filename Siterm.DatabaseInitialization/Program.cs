using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Siterm.DatabaseInitialization.Services;
using Siterm.Domain.Models;
using Siterm.EntityFramework;
using Siterm.EntityFramework.Services;
using Siterm.Settings;
using Siterm.Settings.Models;

namespace Siterm.DatabaseInitialization
{
    public class Program
    {
        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            RegisterLogger(services);
            RegisterConfiguration(services); 
            SettingsServiceProvider.RegisterServices(services);
            EntityServiceProvider.RegisterServices(services);

            RegisterImporterPipeline(services);

            return services.BuildServiceProvider();
        }

        private static void RegisterImporterPipeline(IServiceCollection services)
        {
            var pipelineItems = Assembly.GetAssembly(typeof(IImporter))?.GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(IImporter)));

            if (pipelineItems is null) return;

            foreach (var viewModel in pipelineItems)
                services.Add(new ServiceDescriptor(typeof(IImporter), viewModel, ServiceLifetime.Transient));


            services.AddTransient<ImporterPipeline>();
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var serviceProvider = CreateServiceProvider();

            serviceProvider.GetRequiredService<SitermDbContextFactory>().CreateDbContext().Database.Migrate();
            var importerService = serviceProvider.GetRequiredService<ImporterPipeline>();
            importerService.Run();
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
    }
}
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Siterm.EntityFramework;
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

            services.AddSingleton<SitermDbContextFactory>();

            return services.BuildServiceProvider();
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var serviceProvider = CreateServiceProvider();
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
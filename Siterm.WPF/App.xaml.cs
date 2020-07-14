using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Siterm.EntityFramework;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceProvider = CreateServiceProvider();

            var window = new MainWindow {DataContext = serviceProvider.GetService<MainViewModel>()};
            window.Show();

            base.OnStartup(e);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSingleton<SitermDbContextFactory>();
            services.AddSingleton<TabViewModelCollectionFactory>();

            RegisterViewModels(services);
            RegisterTabItemViewModels(services);

            return services.BuildServiceProvider();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            var viewModels = Assembly.GetAssembly(typeof(BaseViewModel))?.GetTypes()
                .Where(t => t.IsClass && t.IsSubclassOf(typeof(BaseViewModel)));

            if (viewModels is null) return;

            foreach (var viewModel in viewModels) services.AddScoped(viewModel);
        }

        private static void RegisterTabItemViewModels(IServiceCollection services)
        {
            var viewModels = Assembly.GetAssembly(typeof(ITabItemViewModel))?.GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(ITabItemViewModel)));

            if (viewModels is null) return;

            foreach (var viewModel in viewModels) services.Add(new ServiceDescriptor(typeof(ITabItemViewModel), viewModel, ServiceLifetime.Scoped));
        }
    }
}
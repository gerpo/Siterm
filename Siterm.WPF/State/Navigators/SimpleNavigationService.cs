﻿using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Siterm.Support.Misc;

namespace Siterm.WPF.State.Navigators
{
    public class SimpleNavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleNavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync<T>(object parameter = null) where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            if (window is IActivable activableWindow) await activableWindow.ActivateAsync(parameter);

            window.Show();
        }

        public async Task<bool?> ShowDialogAsync<T>(object parameter = null)
            where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            if (window is IActivable activableWindow) await activableWindow.ActivateAsync(parameter);

            return window.ShowDialog();
        }
    }
}
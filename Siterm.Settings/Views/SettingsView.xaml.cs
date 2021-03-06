﻿using System.Threading.Tasks;
using Siterm.Settings.ViewModels;
using Siterm.Support.Misc;

namespace Siterm.Settings.Views
{
    public partial class SettingsView : IActivable
    {
        public SettingsView(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            return Task.CompletedTask;
        }
    }
}
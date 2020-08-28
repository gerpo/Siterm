using System.Collections.Generic;
using System.Windows;
using Siterm.Settings.Models;
using Siterm.Settings.Resources;
using Siterm.Settings.Services;
using Siterm.Support.Misc;

namespace Siterm.Settings.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly SettingsWriter _settingsWriter;
        private IReadOnlyList<SettingValidationError> _validationErrors;

        public SettingsViewModel(SettingsProvider settingsProvider, SettingsWriter settingsWriter)
        {
            _settingsWriter = settingsWriter;

            Settings = settingsProvider.GetAllSettings();

            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        public IReadOnlyList<SettingValidationError> ValidationErrors
        {
            get => _validationErrors;
            private set
            {
                SetField(ref _validationErrors, value);
                OnPropertyChanged("HasErrors");
            }
        }

        public RelayCommand SaveSettingsCommand { get; }

        public IReadOnlyList<Setting> Settings { get; }
        public string Header => SettingUiStrings.SettingsWindowHeader;

        public bool HasErrors => ValidationErrors != null && ValidationErrors.Count > 0;

        private async void SaveSettings(object o)
        {
            var t = await _settingsWriter.TrySavingSettings(Settings);
            ValidationErrors = t.Count > 0 ? t : null;

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
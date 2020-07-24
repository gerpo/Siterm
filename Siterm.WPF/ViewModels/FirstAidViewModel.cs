using System.Collections.Generic;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Support.Misc;

namespace Siterm.WPF.ViewModels
{
    public class FirstAidViewModel : BaseViewModel, ITabItemViewModel
    {
        private readonly SettingsWriter _settingsWriter;

        public FirstAidViewModel(SettingsProvider settingsProvider, SettingsWriter settingsWriter)
        {
            _settingsWriter = settingsWriter;

            Settings = settingsProvider.GetAllSettings();

            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        public RelayCommand SaveSettingsCommand { get; }

        public IReadOnlyList<Setting> Settings { get; }
        public string Header => UiStrings.FirstAidTabHeader;
        public int Position => 2;

        private async void SaveSettings(object o)
        {
            var t = await _settingsWriter.TrySavingSettings(Settings);
        }
    }
}
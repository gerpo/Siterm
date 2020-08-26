using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Siterm.Settings.Exceptions;
using Siterm.Settings.Models;

namespace Siterm.Settings.Services
{
    public class SettingsProvider
    {
        private readonly AppSettings _settings;

        public readonly IReadOnlyDictionary<SettingName, SettingType> SettingsMap =
            new Dictionary<SettingName, SettingType>
            {
                {SettingName.DatabaseConnectionString, SettingType.String},
                {SettingName.MainPath, SettingType.Folder},
                {SettingName.HomeInfoFile, SettingType.File},
                {SettingName.FirstResponderExcelFile, SettingType.File},
                {SettingName.FirstResponderInfoFile, SettingType.File},
                {SettingName.SubstancePath, SettingType.Folder},
                {SettingName.FacilityPath, SettingType.Folder},
                {SettingName.InstructionFolderName, SettingType.String},
                {SettingName.InstructionArchiveFolderName, SettingType.String},
                {SettingName.ServiceReportFolderName, SettingType.String},
                {SettingName.InstructionTemplateFile, SettingType.File},
            };

        public SettingsProvider(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public IReadOnlyList<Setting> GetAllSettings()
        {
            return SettingsMap.Keys.Select(GetSetting).ToList().AsReadOnly();
        }

        public Setting GetSetting(SettingName settingName)
        {
            if (!SettingsMap.ContainsKey(settingName)) throw new SettingHasNoMappedType();

            var settingsValue = typeof(AppSettings).GetProperty(settingName.ToString())?.GetValue(_settings, null)
                ?.ToString();
            return new Setting(settingName, settingsValue, SettingsMap[settingName]);
        }
    }
}
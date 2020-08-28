using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Siterm.Settings.Converter;
using Siterm.Settings.Models;

namespace Siterm.Settings.Services
{
    public class SettingsWriter
    {
        private readonly JsonSerializerOptions _options;
        private readonly SettingsValidator _settingsValidator;

        public SettingsWriter(SettingsValidator settingsValidator)
        {
            _settingsValidator = settingsValidator;
            _options = new JsonSerializerOptions
            {
                Converters = {new SettingJsonConverter()}
            };
        }

        public Task SaveSettingsAsync(IEnumerable<Setting> settings)
        {
            using var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"),
                FileMode.Create);
            var appSettings = new AppSettings();
            foreach (var setting in settings)
                typeof(AppSettings).GetProperty(setting.Name.ToString())?.SetValue(appSettings, setting.Value, null);

            // Need to wrap it in an anonymous class in order to get it serialized under "AppSettings".
            return JsonSerializer.SerializeAsync(stream, new {AppSettings = appSettings}, _options);
        }

        public Task<IReadOnlyList<SettingValidationError>> TrySavingSettings(IReadOnlyList<Setting> settings)
        {
            if (_settingsValidator.Validate(settings))
                SaveSettingsAsync(settings);

            return Task.FromResult(_settingsValidator.ValidationErrors);
        }
    }
}
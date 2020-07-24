using System;
using System.Collections.Generic;
using System.IO;
using Siterm.Settings.Models;
using Siterm.Settings.Resources;

namespace Siterm.Settings.Services
{
    public class SettingsValidator
    {
        private List<SettingValidationError> _validationErrors = new List<SettingValidationError>();

        public IReadOnlyList<SettingValidationError> ValidationErrors => _validationErrors;

        public bool Validate(IEnumerable<Setting> settings)
        {
            _validationErrors = new List<SettingValidationError>();
            foreach (var setting in settings) ValidateSetting(setting);

            return IsValid();
        }

        private void FileMustExist(Setting setting)
        {
            if (setting.Value != null && !File.Exists(setting.Value))
                _validationErrors.Add(new SettingValidationError(setting, ValidationMessages.FileMustExist));
        }

        private void FolderMustExist(Setting setting)
        {
            if (setting.Value != null && !Directory.Exists(setting.Value))
                _validationErrors.Add(new SettingValidationError(setting, ValidationMessages.FileMustExist));
        }

        private bool HasErrors()
        {
            return ValidationErrors.Count > 0;
        }

        private bool IsValid()
        {
            return !HasErrors();
        }

        private bool ValidateSetting(Setting setting)
        {
            switch (setting.Type)
            {
                case SettingType.File:
                    ValueMustBeNotNullOrEmpty(setting);
                    FileMustExist(setting);
                    break;
                case SettingType.Folder:
                    ValueMustBeNotNullOrEmpty(setting);
                    FolderMustExist(setting);
                    break;
                case SettingType.MultiFolder:
                    ValueMustBeNotNullOrEmpty(setting);
                    break;
                case SettingType.FolderFilter:
                    break;
                case SettingType.String:
                    ValueMustBeNotNullOrEmpty(setting);
                    break;
                case SettingType.Int:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return IsValid();
        }

        private void ValueMustBeNotNullOrEmpty(Setting setting)
        {
            if (string.IsNullOrEmpty(setting.Value))
                _validationErrors.Add(new SettingValidationError(setting, ValidationMessages.IsNullOrEmpty));
        }
    }
}
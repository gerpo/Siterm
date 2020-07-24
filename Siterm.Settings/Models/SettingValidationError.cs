using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Settings.Models
{
    public class SettingValidationError
    {
        public Setting Setting { get; }
        public string Message { get; }

        public SettingValidationError(Setting setting, string message)
        {
            Setting = setting;
            Message = message;
        }
    }
}

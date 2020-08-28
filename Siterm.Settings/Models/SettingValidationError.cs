namespace Siterm.Settings.Models
{
    public class SettingValidationError
    {
        public SettingValidationError(Setting setting, string message)
        {
            Setting = setting;
            Message = message;
        }

        public Setting Setting { get; }
        public string Message { get; }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace Siterm.WPF.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            try
            {
                var enumString = Enum.GetName(value.GetType(), value);
                if (enumString is null) return null;
                return UiStrings.ResourceManager.GetString(enumString, CultureInfo.CurrentUICulture) ?? enumString;
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
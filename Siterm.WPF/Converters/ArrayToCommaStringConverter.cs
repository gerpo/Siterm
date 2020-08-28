using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Siterm.WPF.Converters
{
    public class ArrayToCommaStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string[] strings) return string.Join(", ", strings);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string) value)?.Split(',').All(el =>
            {
                el.Trim();
                return true;
            });
        }
    }
}
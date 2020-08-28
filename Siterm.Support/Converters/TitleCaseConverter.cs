using System;
using System.Globalization;
using System.Windows.Data;

namespace Siterm.Support.Converters
{
    public class TitleCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is string stringValue) ? null : culture.TextInfo.ToTitleCase(stringValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
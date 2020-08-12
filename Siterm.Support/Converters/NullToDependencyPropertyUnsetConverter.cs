using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Siterm.Support.Converters
{
    public class NullToDependencyPropertyUnsetConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
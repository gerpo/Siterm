using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore.Internal;

namespace Siterm.WPF.Converters
{
    internal class NullToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is null || (value is IEnumerable o && o.IsNullOrEmpty())) ? "Collapsed" : "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
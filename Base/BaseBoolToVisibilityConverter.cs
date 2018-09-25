using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace SpchListBuilder.Extension
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object val, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? x = val as bool?;
            if ((x != null) && (x.Value))
                return Visibility.Visible;
            return Visibility.Hidden;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

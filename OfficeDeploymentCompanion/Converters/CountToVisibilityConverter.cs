using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OfficeDeploymentCompanion.Converters
{
    public class CountToVisibilityConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
                return (int)value > 0 ? Visibility.Visible : Visibility.Collapsed;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    public class InverseEnumVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EnumVisibilityConverter evc = new EnumVisibilityConverter();
            Visibility result = (Visibility)evc.Convert(value, targetType, parameter, culture);

            if (result == Visibility.Visible)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

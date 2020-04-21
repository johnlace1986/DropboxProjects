using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(ResizeMode), typeof(Visibility))]
    public class WindowControlButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResizeMode resizeMode = (ResizeMode)value;

            return (resizeMode == ResizeMode.NoResize ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

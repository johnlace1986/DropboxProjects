using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(ResizeMode), typeof(Boolean))]
    public class MaximizeButtonEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResizeMode resizeMode = (ResizeMode)value;

            return (resizeMode != ResizeMode.CanMinimize);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

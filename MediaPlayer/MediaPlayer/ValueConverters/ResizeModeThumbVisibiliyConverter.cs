using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(ResizeMode), typeof(Visibility))]
    public class ResizeModeThumbVisibiliyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ResizeMode resizeMode = (ResizeMode)value;

            switch (resizeMode)
            {
                case ResizeMode.CanMinimize:
                    return Visibility.Collapsed;

                case ResizeMode.CanResize:
                    return Visibility.Visible;

                case ResizeMode.CanResizeWithGrip:
                    return Visibility.Visible;

                case ResizeMode.NoResize:
                    return Visibility.Collapsed;

                default:
                    throw new ArgumentException("Unknown ResizeMode value");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

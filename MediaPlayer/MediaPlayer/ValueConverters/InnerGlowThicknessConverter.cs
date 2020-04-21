using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(Thickness), typeof(double))]
    public class InnerGlowThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            String side = (String)parameter;

            switch (side)
            {
                case "Left":
                    return thickness.Left;
                case "Right":
                    return thickness.Right;
                case "Top":
                    return thickness.Top;
                case "Bottom":
                    return thickness.Bottom;
                default:
                    throw new ArgumentException("Unknown parameter value specified: " + side);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

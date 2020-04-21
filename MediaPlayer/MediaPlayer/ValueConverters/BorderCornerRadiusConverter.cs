using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(CornerRadius), typeof(CornerRadius))]
    public class BorderCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CornerRadius corderRadius = (CornerRadius)value;
            String corner = parameter as String;

            switch (corner)
            {
                case "TopLeft":
                    return new CornerRadius(corderRadius.TopLeft, 0, 0, 0);

                case "TopRight":
                    return new CornerRadius(0, corderRadius.TopRight, 0, 0);

                case "BottomRight":
                    return new CornerRadius(0, 0, corderRadius.BottomRight, 0);

                case "BottomLeft":
                    return new CornerRadius(0, 0, 0, corderRadius.BottomLeft);

                default:
                    throw new ArgumentException("Unknown parameter: " + parameter);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                TimeSpan ts = (TimeSpan)value;
                return ts.TotalSeconds;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
                return TimeSpan.FromSeconds((double)value);

            return TimeSpan.FromSeconds(0);
        }
    }
}

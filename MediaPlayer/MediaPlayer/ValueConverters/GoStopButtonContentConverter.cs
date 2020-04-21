using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(Boolean), typeof(String))]
    public class GoStopButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Boolean isGoing = (Boolean)value;

            return (isGoing ? "Stop" : "Go");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

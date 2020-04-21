using System;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(TimeSpan), typeof(String))]
    public class TimeSpanStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                Boolean includeMilliseconds = System.Convert.ToBoolean(parameter.ToString());
                TimeSpan ts = (TimeSpan)value;
                return IntelligentString.FormatDuration(ts, includeMilliseconds);
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

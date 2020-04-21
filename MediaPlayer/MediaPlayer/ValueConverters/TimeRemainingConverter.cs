using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    public class TimeRemainingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 3)
            {
                if (values[0] is TimeSpan)
                {
                    TimeSpan duration = (TimeSpan)values[0];

                    if (values[1] is TimeSpan)
                    {
                        TimeSpan position = (TimeSpan)values[1];

                        if (values[2] is Boolean)
                        {
                            Boolean showTimeRemaining = (Boolean)values[2];

                            TimeSpan result = (showTimeRemaining ? duration - position : duration);

                            TimeSpanStringConverter tssc = new TimeSpanStringConverter();
                            return tssc.Convert(result, targetType, parameter, culture);
                        }
                    }
                }
            }

            return String.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

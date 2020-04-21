using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class PositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (values[0] is TimeSpan)
                {
                    TimeSpan ts = (TimeSpan)values[0];

                    MediaItem mediaItem = values[1] as MediaItem;

                    if (mediaItem != null)
                    {
                        TimeSpanStringConverter tssc = new TimeSpanStringConverter();
                        return tssc.Convert(ts, targetType, parameter, culture);
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

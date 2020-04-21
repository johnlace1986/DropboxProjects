using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(MediaItem), typeof(Video))]
    public class MediaItemToVideoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value as Video;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value as MediaItem;
        }
    }
}

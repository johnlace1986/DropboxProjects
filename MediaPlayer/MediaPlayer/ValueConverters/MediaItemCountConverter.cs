using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class MediaItemCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<MediaItem> mediaItems = value as IEnumerable<MediaItem>;

            if (mediaItems != null)
            {
                int count = mediaItems.Count();

                if (count == 1)
                    return "1 media item";
                else
                    return count.ToString() + " media items";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

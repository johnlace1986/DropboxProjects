using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class ModificationRulePropertyNamesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MediaItem[] mediaItems = (value as IEnumerable<MediaItem>).ToArray();

            MediaItemTypeEnum[] types =
                (from mediaItem in mediaItems
                 select mediaItem.Type).Distinct().ToArray();

            if (types.Length != 1)
                throw new ArgumentException(types.Length.ToString() + " MediaItemTypes specified");

            return mediaItems[0].ModifyProperties;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

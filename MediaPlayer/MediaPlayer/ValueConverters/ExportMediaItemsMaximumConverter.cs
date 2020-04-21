using MediaPlayer.Library.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    public class ExportMediaItemsMaximumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<MediaItem> mediaItems = value as IEnumerable<MediaItem>;

            return
                (from mediaItem in mediaItems
                 from part in mediaItem.Parts
                 select part.Size).Sum();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

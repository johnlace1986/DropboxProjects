using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using System.Collections.ObjectModel;
using System.Collections;

namespace MediaPlayer.ValueConverters
{
    public class MediaItemCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<MediaItem> items = new ObservableCollection<MediaItem>();
            
            if (value != null)
            {
                IEnumerable typedItems = value as IEnumerable;

                foreach (MediaItem item in typedItems)
                    items.Add(item);
            }

            return items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

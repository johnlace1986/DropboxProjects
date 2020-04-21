using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class MediaItemViewVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MediaItem)
            {
                MediaItem mediaItem = value as MediaItem;

                if (parameter != null)
                {
                    String type = parameter.ToString();

                    if (mediaItem.Type == (MediaItemTypeEnum)Enum.Parse(typeof(MediaItemTypeEnum), type, true))
                        return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

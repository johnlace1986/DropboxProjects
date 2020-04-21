using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using Utilities.Business;
using Utilities.Exception;

namespace MediaPlayer.ValueConverters
{
    public class MediaItemGenresConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (values[0] is MediaItem)
                {
                    MediaItem mediaItem = values[0] as MediaItem;
                    IMultiValueConverter valueConverter;

                    switch (mediaItem.Type)
                    {
                        case MediaItemTypeEnum.Video:
                            valueConverter = new VideoViewGenresConverter();
                            break;

                        case MediaItemTypeEnum.Song:
                            valueConverter = new SongViewGenresConverter();
                            break;

                        default:
                            throw new UnknownEnumValueException(mediaItem.Type);
                    }

                    return valueConverter.Convert(values, targetType, parameter, culture);
                }
            }

            return new IntelligentString[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

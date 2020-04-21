using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using Utilities.Exception;

namespace MediaPlayer.ValueConverters
{
    public class MediaItemTagsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (values[0] is MediaItem)
                {
                    MediaItem mediaItem = values[0] as MediaItem;
                    Boolean? showHidden = values[1] as Boolean?;
                    
                    switch (mediaItem.Type)
                    {
                        case MediaItemTypeEnum.Video:
                            
                            VideoTagsConverter vtc = new VideoTagsConverter();
                            return vtc.Convert(showHidden, targetType, parameter, culture);

                        case MediaItemTypeEnum.Song:
                            SongTagsConverter stc = new SongTagsConverter();
                            return stc.Convert(showHidden, targetType, parameter, culture);

                        default:
                            throw new UnknownEnumValueException(mediaItem.Type);
                    }
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

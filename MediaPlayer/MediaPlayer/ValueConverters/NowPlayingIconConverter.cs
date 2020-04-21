using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Business;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class NowPlayingIconConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 3)
            {
                if (values[0] is PlayStateEnum)
                {
                    PlayStateEnum playState = (PlayStateEnum)values[0];

                    if (playState != PlayStateEnum.Stopped)
                    {
                        if (values[1] is MediaItem)
                        {
                            MediaItem currentlyPlayingMediaItem = values[1] as MediaItem;

                            if (values[2] is MediaItem)
                            {
                                MediaItem selectedMediaItem = values[2] as MediaItem;

                                if (selectedMediaItem == currentlyPlayingMediaItem)
                                {
                                    return playState;
                                }
                            }
                        }
                    }
                }
            }

            return PlayStateEnum.Stopped;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

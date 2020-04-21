using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Business;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    public class PlayPauseIconStoppedVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (values[0] is PlayStateEnum)
                {
                    PlayStateEnum playState = (PlayStateEnum)values[0];

                    if (playState == PlayStateEnum.Stopped)
                    {
                        if (values[1] is Boolean)
                        {
                            Boolean visibileWhenStopped = (Boolean)values[1];

                            if (!visibileWhenStopped)
                                return Visibility.Collapsed;
                        }
                    }
                }
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

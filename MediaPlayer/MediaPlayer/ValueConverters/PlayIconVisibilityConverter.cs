using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Business;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    public class PlayIconVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (values[0] is PlayStateEnum)
                {
                    PlayStateEnum playState = (PlayStateEnum)values[0];

                    if (playState != PlayStateEnum.Stopped)
                    {
                        if (values[1] is Boolean)
                        {
                            Boolean showActionIcon = (Boolean)values[1];

                            Boolean visible =
                                (((playState == PlayStateEnum.Playing) && (!showActionIcon)) ||
                                ((playState == PlayStateEnum.Paused) && (showActionIcon)));

                            if (!visible)
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

using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using MediaPlayer.Business;

namespace MediaPlayer.ValueConverters
{
    public class ResizeMediaPlayerVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (values[0] is Boolean)
                {
                    Boolean showControls = (Boolean)values[0];

                    if (values[1] is PlayStateEnum)
                    {
                        PlayStateEnum playState = (PlayStateEnum)values[1];

                        if (showControls)
                        {
                            if (playState != PlayStateEnum.Stopped)
                            {
                                return Visibility.Visible;
                            }
                        }
                    }
                }
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

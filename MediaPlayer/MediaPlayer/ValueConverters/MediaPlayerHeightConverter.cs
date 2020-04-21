using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    public class MediaPlayerHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is double)
            {
                double height = (double)values[0];
                if (values[1] is double)
                {
                    double containerHeight = (double)values[1];
                    if (values[2] is Boolean)
                    {
                        Boolean stretchMediaPlayer = (Boolean)values[2];

                        if (stretchMediaPlayer)
                            return containerHeight;

                        if (height > containerHeight)
                            return containerHeight;

                        return height;
                    }
                }
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

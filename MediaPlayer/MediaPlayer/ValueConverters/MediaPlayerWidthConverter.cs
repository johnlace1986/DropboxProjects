using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    public class MediaPlayerWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is double)
            {
                double width = (double)values[0];
                if (values[1] is double)
                {
                    double containerWidth = (double)values[1];
                    if (values[2] is Boolean)
                    {
                        Boolean stretchMediaPlayer = (Boolean)values[2];

                        if (stretchMediaPlayer)
                            return containerWidth;

                        if (width > containerWidth)
                            return containerWidth;

                        return width;
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

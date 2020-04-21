using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace MediaPlayer.ValueConverters
{
    public class MediaPlayerStrechConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Boolean)
            {
                Boolean stretchMediaPlayer = (Boolean)value;

                if (stretchMediaPlayer)
                    return Stretch.Uniform;
            }

            return Stretch.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    public class SongViewAlbumsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                Boolean? showHidden = values[1] as Boolean?;

                MediaPlayerDialog mpd = Application.Current.MainWindow as MediaPlayerDialog;

                List<object> lstValues = new List<object>();
                lstValues.Add(mpd.Songs);
                lstValues.Add(showHidden);

                UnfilteredAlbumsConverter upc = new UnfilteredAlbumsConverter();
                return upc.Convert(lstValues.ToArray(), targetType, parameter, culture);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

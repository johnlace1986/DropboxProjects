using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(Int32), typeof(Int32[]))]
    public class LoadingBlobsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Int32 blobs = System.Convert.ToInt32(value);

            List<Int32> items = new List<Int32>();

            for (int i = 0; i < blobs; i++)
            {
                items.Add(i);
            }

            return items.ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

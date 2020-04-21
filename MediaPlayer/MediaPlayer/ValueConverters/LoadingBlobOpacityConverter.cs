using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    public class LoadingBlobOpacityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double index = System.Convert.ToDouble(values[0]);
            double blobs = System.Convert.ToDouble(values[1]);
            Int32 value = System.Convert.ToInt32(values[2]);

            double step = 1 / (blobs - 1);
            double opacity = index * step;

            for (int i = 0; i < value; i++)
                opacity -= step;

            if (opacity < 0)
                opacity += (1 + step);

            return opacity;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

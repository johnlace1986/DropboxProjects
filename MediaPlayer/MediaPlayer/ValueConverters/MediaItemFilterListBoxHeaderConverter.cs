using System;
using System.Collections;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;

namespace MediaPlayer.ValueConverters
{
    public class MediaItemFilterListBoxHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String result = String.Empty;

            if (value != null)
            {
                IEnumerable items = value as IEnumerable;
                Int64 count = items.Cast<IntelligentString>().Count() - 1; //subtract 1 for View All

                result = count.ToString();

                if (parameter != null)
                    result = parameter.ToString() + " - " + result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

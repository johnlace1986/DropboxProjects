using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(Int16), typeof(String))]
    public class ShortUpDownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Int16 shortValue = (Int16)value;

            if (shortValue == 0)
                return String.Empty;

            return shortValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String stringValue = System.Convert.ToString(value);

            if (String.IsNullOrEmpty(stringValue))
                return 0;

            Int16 shortValue;

            if (Int16.TryParse(stringValue, out shortValue))
                return shortValue;

            return String.Empty;
        }
    }
}

using System;
using System.Linq;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    public class PercentageValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String format = "0.00%";

            if (parameter is String)
                format = (String)parameter;

            if (values.Length == 2)
            {
                if (values[0] != null)
                {
                    String strValue1 = values[0].ToString();
                    double value1;

                    if (double.TryParse(strValue1, out value1))
                    {
                        if (values[1] != null)
                        {
                            String strValue2 = values[1].ToString();
                            double value2;

                            if (double.TryParse(strValue2, out value2))
                            {
                                return (value1 / value2).ToString(format);
                            }
                        }
                    }
                }
            }

            return (0).ToString(format);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

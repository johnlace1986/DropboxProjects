using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MediaPlayer.ValueConverters
{
    public class NumberGreateThanParameterVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                double number;

                if (double.TryParse(value.ToString(), out number))
                {
                    if (parameter != null)
                    {
                        double greaterThan;

                        if (double.TryParse(parameter.ToString(), out greaterThan))
                        {
                            if (number > greaterThan)
                                return Visibility.Visible;
                        }
                    }
                }
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Utilities.Business;

namespace Utilities.ValueConverters
{
    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Boolean actual = (Boolean)value;

            return GeneralMethods.ConvertBooleanToVisibility(actual);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility actual = (Visibility)value;

            return GeneralMethods.ConvertVisibilityToBoolean(actual);
        }
    }
}

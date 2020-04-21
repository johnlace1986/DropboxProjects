using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Utilities.Business;

namespace Utilities.ValueConverters
{
    public class InverseBooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Boolean actual = (Boolean)value;

            Visibility result = GeneralMethods.ConvertBooleanToVisibility(!actual);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility actual = (Visibility)value;

            return !GeneralMethods.ConvertVisibilityToBoolean(actual);
        }
    }
}

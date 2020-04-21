using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Utilities.Business;

namespace Utilities.ValueConverters
{
    [ValueConversion(typeof(IntelligentString), typeof(String))]
    public class IntelligentStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IntelligentString istr = (IntelligentString)value;
            return istr.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String str = value as String;
            IntelligentString istr = str;

            return istr;
        }
    }
}

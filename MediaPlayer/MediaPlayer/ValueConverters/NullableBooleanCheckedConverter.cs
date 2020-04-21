using System;
using System.Linq;
using System.Windows.Data;
using Utilities.Exception;

namespace MediaPlayer.ValueConverters
{
    public class NullableBooleanCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Boolean? isChecked = (Boolean?)value;
            String strParameter = (String)parameter;

            switch (strParameter)
            {
                case "Yes":
                    return ((isChecked.HasValue) && (isChecked.Value));
                case "No":
                    return ((isChecked.HasValue) && (!isChecked.Value));
                case "All":
                    return (!isChecked.HasValue);

                default:
                    throw new UnknownEnumValueException("parameter", strParameter);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String strParameter = (String)parameter;

            switch (strParameter)
            {
                case "Yes":
                    return true;
                case "No":
                    return false;
                case "All":
                    return null;

                default:
                    throw new UnknownEnumValueException("parameter", strParameter);
            }            
        }
    }
}

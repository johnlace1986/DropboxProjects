using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Utilities.Business;

namespace Utilities.ValueConverters
{
    public class TextValueDialogInputVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                TextValueDialogInputType item = (TextValueDialogInputType)value;

                if (parameter is String)
                {
                    if (Enum.IsDefined(typeof(TextValueDialogInputType), parameter))
                    {
                        TextValueDialogInputType type = (TextValueDialogInputType)Enum.Parse(typeof(TextValueDialogInputType), parameter as String, false);

                        if (item == type)
                            return Visibility.Visible;
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

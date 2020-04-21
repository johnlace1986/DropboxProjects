using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    public class EnumVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Enum)
            {
                Enum actual = value as Enum;
                
                if (parameter is String)
                {
                    String strDesired = (String)parameter;

                    try
                    {
                        Enum desired = (Enum)Enum.Parse(actual.GetType(), strDesired);

                        if (actual.Equals(desired))
                            return Visibility.Visible;
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

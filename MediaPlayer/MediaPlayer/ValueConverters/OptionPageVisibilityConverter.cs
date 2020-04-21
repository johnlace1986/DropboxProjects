using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;
using MediaPlayer.Business;
using MediaPlayer.Presentation.UserControls.ControlExtenders;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(OptionPageTreeViewItem), typeof(Visibility))]
    public class OptionPageVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            OptionPageTreeViewItem item = value as OptionPageTreeViewItem;

            if (item != null)
            {
                if (parameter is String)
                {
                    if (Enum.IsDefined(typeof(OptionsPageTypeEnum), parameter))
                    {
                        OptionsPageTypeEnum type = (OptionsPageTypeEnum)Enum.Parse(typeof(OptionsPageTypeEnum), parameter as String, false);

                        if (item.PageType == type)
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

using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;
using MediaPlayer.Library.Business;
using System.Collections;

namespace MediaPlayer.ValueConverters
{
    public class NextPreviousButtonVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if ((values[0] != null) && (values[0] != DependencyProperty.UnsetValue))
                {
                    MediaItem[] mediaItems = (values[0] as IEnumerable).Cast<MediaItem>().ToArray();

                    if ((values[1] != null) && (values[1] != DependencyProperty.UnsetValue))
                    {
                        Int32 selectedIndex = (Int32)values[1];

                        String nextOrPrevious = System.Convert.ToString(parameter);

                        switch (nextOrPrevious)
                        {
                            case "Previous":
                                return (selectedIndex == 0 ? Visibility.Hidden : Visibility.Visible);

                            case "Next":
                                return (selectedIndex < (mediaItems.Length - 1) ? Visibility.Visible : Visibility.Hidden);
                        }
                    }
                }
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

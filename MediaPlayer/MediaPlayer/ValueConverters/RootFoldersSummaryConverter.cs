using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class RootFoldersSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RootFolder[] RootFolders = value as RootFolder[];

            if (RootFolders != null)
            {
                if (RootFolders.Length == 1)
                    return "1 Root Folder";

                return RootFolders.Length.ToString() + " Root Folders";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

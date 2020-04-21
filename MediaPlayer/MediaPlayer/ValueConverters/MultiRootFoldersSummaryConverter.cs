using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class MultiRootFoldersSummaryConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 2)
                return String.Empty;

            RootFolder[] videoRootFolders = values[0] as RootFolder[];
            RootFolder[] songRootFolders = values[1] as RootFolder[];

            if ((videoRootFolders != null) && (songRootFolders != null))
            {
                int totalRootFolders = videoRootFolders.Length + songRootFolders.Length;

                if (totalRootFolders == 1)
                    return "1 Root Folder";

                return totalRootFolders.ToString() + " Root Folders";
            }

            return String.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

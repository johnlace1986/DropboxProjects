using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MediaPlayer.ValueConverters
{
    public class FileTypesSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<FileType> fileTypes = value as IEnumerable<FileType>;

            if (fileTypes != null)
            {
                if (fileTypes.Count() == 1)
                    return "1 File Type";

                return fileTypes.Count().ToString() + " File Types";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

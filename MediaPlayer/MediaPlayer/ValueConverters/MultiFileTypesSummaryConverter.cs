using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class MultiFileTypesSummaryConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int count = 0;

            foreach (object value in values)
            {
                IEnumerable<FileType> fileTypes = value as IEnumerable<FileType>;

                if (fileTypes != null)
                    count += fileTypes.Count();
            }

            if (count == 1)
                return "1 File Type";
            else
                return count.ToString() + " File Types";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;

namespace MediaPlayer.ValueConverters
{
    public class RootFolderSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 3)
            {
                if (values[0] is Boolean)
                {
                    Boolean isValidPath = (Boolean)values[0];

                    if (isValidPath)
                    {
                        if (values[1] is Int64)
                        {
                            double usedSpace = (Int64)values[1];

                            if (values[2] is Int64)
                            {
                                double totalSpace = (Int64)values[2];

                                double percentage = usedSpace / totalSpace;

                                return "Used " + IntelligentString.FormatSize(usedSpace).Value + " of " + IntelligentString.FormatSize(totalSpace).Value + " (" + percentage.ToString("0.00%") + ")";
                            }
                        }
                    }
                    else
                        return "Path does not exist";
                }
            }

            return String.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

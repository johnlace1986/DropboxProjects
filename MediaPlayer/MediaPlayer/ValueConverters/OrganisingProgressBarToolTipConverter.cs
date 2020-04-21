using System;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;
using MediaPlayer.Business;

namespace MediaPlayer.ValueConverters
{
    public class OrganisingProgressBarToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 4)
            {
                if (values[0] is Int64)
                {
                    Int64 progress = (Int64)values[0];

                    if (values[1] is Int64)
                    {
                        Int64 size = (Int64)values[1];

                        if (values[2] is OrganisingMediaItemPartStatus)
                        {
                            OrganisingMediaItemPartStatus status = (OrganisingMediaItemPartStatus)values[2];

                            if (values[3] is Int32)
                            {
                                Int32 errorCount = (Int32)values[3];

                                switch (status)
                                {
                                    case OrganisingMediaItemPartStatus.Waiting:
                                        return "Waiting...";
                                    case OrganisingMediaItemPartStatus.Organising:
                                        return IntelligentString.FormatSize(progress).Value + " / " + IntelligentString.FormatSize(size).Value;
                                    case OrganisingMediaItemPartStatus.Error:
                                        return "Not organised";
                                    case OrganisingMediaItemPartStatus.Organised:
                                        if (errorCount == 0)
                                            return "Successfully organised";
                                        else
                                        {
                                            if (errorCount == 1)
                                                return "Organised with 1 error";
                                            else
                                                return "Organised with " + errorCount.ToString() + " errors";
                                        }
                                }
                            }
                        }
                    }
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

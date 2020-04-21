using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Utilities.Business;

namespace MediaPlayer.ValueConverters
{
    public class TrackerTimeRemainingToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String toolTip = String.Empty;

            if (values.Length == 2)
            {
                if (values[0] is TimeSpan)
                {
                    TimeSpan duration = (TimeSpan)values[0];

                    if (duration > new TimeSpan(0))
                    {
                        if (values[1] is TimeSpan)
                        {
                            TimeSpan ellapsed = (TimeSpan)values[1];
                            TimeSpan remaining = duration - ellapsed;

                            DateTime finishTime = DateTime.Now + remaining;

                            toolTip += "Duration: " + IntelligentString.FormatDuration(duration, false);
                            toolTip += Environment.NewLine + "Ellapsed: " + IntelligentString.FormatDuration(ellapsed, false);
                            toolTip += Environment.NewLine + "Remaining: " + IntelligentString.FormatDuration(remaining, false);
                            toolTip += Environment.NewLine + "Finish time: " + finishTime.ToString("h:mm tt");
                        }
                    }
                }
            }

            return toolTip;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

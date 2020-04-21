using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;
using MediaPlayer.Library.Business;

namespace MediaPlayer.ValueConverters
{
    public class MediaItemsSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IntelligentString summary = IntelligentString.Empty;
            String strShowSize = "True";
            Boolean showSize = true;

            if (parameter != null)
            {
                strShowSize = parameter.ToString();
                Boolean.TryParse(strShowSize, out showSize);
            }

            if (value is IEnumerable<MediaItem>)
            {
                MediaItem[] mediaItems = (value as IEnumerable<MediaItem>).ToArray();

                int numberOfItems = mediaItems.Length;
                Int64 size = 0;
                TimeSpan duration = new TimeSpan();

                String type = "Media Item";

                if (numberOfItems > 0)
                {
                    type = mediaItems[0].Type.ToString();
                }

                foreach (MediaItem mediaItem in mediaItems)
                {
                    size += mediaItem.Parts.Size;
                    duration += mediaItem.Parts.Duration;

                    if (type != mediaItem.Type.ToString())
                        type = "Media Item";
                }

                if (numberOfItems == 1)
                    summary += "1 " + type + " - ";
                else
                    summary += numberOfItems.ToString() + " " + type + "s - ";

                if (showSize)
                    summary += IntelligentString.FormatSize(size) + " - ";

                summary += IntelligentString.FormatDuration(duration, false);
            }

            return summary;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

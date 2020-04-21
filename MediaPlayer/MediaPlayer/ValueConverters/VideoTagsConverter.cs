using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    public class VideoTagsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<IntelligentString> tags = new List<IntelligentString>();

            Boolean? showHidden = value as Boolean?;

            MediaPlayerDialog mpd = Application.Current.MainWindow as MediaPlayerDialog;
            List<object> lstValues = new List<object>();

            lstValues.Add(mpd.Videos);
            lstValues.Add(showHidden);

            VideoViewGenresConverter videoGenres = new VideoViewGenresConverter();
            tags.AddRange((IEnumerable<IntelligentString>)videoGenres.Convert(lstValues.ToArray(), targetType, parameter, culture));

            VideoViewProgramsConverter videoPrograms = new VideoViewProgramsConverter();
            tags.AddRange((IEnumerable<IntelligentString>)videoPrograms.Convert(lstValues.ToArray(), targetType, parameter, culture));

            tags.Sort();
            return tags.ToArray();
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

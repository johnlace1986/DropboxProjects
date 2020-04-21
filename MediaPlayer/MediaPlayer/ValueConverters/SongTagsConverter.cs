using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    public class SongTagsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<IntelligentString> tags = new List<IntelligentString>();

            Boolean? showHidden = value as Boolean?;

            MediaPlayerDialog mpd = Application.Current.MainWindow as MediaPlayerDialog;
            List<object> lstValues = new List<object>();

            lstValues.Add(mpd.Songs);
            lstValues.Add(showHidden);

            SongViewGenresConverter songGenres = new SongViewGenresConverter();
            tags.AddRange((IEnumerable<IntelligentString>)songGenres.Convert(lstValues.ToArray(), targetType, parameter, culture));

            SongViewArtistsConverter songArtists = new SongViewArtistsConverter();
            tags.AddRange((IEnumerable<IntelligentString>)songArtists.Convert(lstValues.ToArray(), targetType, parameter, culture));

            SongViewAlbumsConverter songAlbums = new SongViewAlbumsConverter();
            tags.AddRange((IEnumerable<IntelligentString>)songAlbums.Convert(lstValues.ToArray(), targetType, parameter, culture));

            tags.Sort();
            return tags.ToArray();
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

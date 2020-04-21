using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using Utilities.Business;
using System.Collections.ObjectModel;
using System.Collections;

namespace MediaPlayer.ValueConverters
{
    public class UnfilteredAlbumsConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the album of the specified song passes throught the filter and should be displayed
        /// </summary>
        /// <param name="song">Song being checked</param>
        /// <param name="showHidden">Value determining whether or not hidden songs should be displayed</param>
        /// <returns>True if the album of the specified song passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassAlbum(Song song, Boolean? showHidden)
        {
            if (IntelligentString.IsNullOrEmpty(song.Album))
                return false;

            if (showHidden.HasValue)
                if (song.IsHidden != showHidden.Value)
                    return false;

            return true;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable mediaItems = values[0] as IEnumerable;
            Boolean? showHidden = (Boolean?)values[1];

            if (mediaItems != null)
            {
                List<Song> lstSongs = new List<Song>(mediaItems.Cast<Song>());

                List<IntelligentString> albums = new List<IntelligentString>(
                    (from song in lstSongs
                     where PassAlbum(song, showHidden)
                     select song.Album).Distinct());

                albums.Sort();
                return new ObservableCollection<IntelligentString>(albums);
            }

            return new ObservableCollection<IntelligentString>();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

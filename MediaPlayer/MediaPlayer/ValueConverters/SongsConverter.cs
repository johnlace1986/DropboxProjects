using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using Utilities.Business;
using System.Collections.ObjectModel;
using System.Windows;
using MediaPlayer.Business;

namespace MediaPlayer.ValueConverters
{
    public class SongsConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the specified song passes throught the filter and should be displayed
        /// </summary>
        /// <param name="song">Song being checked</param>
        /// <param name="viewAllText">Text used to view all media items</param>
        /// <param name="filter">Filter used to filter songs</param>
        /// <param name="selectedGenres">Genres selected in the filter</param>
        /// <param name="selectedArtists">Artists selected in the filter</param>
        /// <param name="selectedAlbum">Album selected in the filter</param>
        /// <returns>True if the specified song passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassSong(Song song, IntelligentString viewAllText, MediaItemFilter filter, IntelligentString[] selectedGenres, IntelligentString[] selectedArtists, IntelligentString[] selectedAlbum)
        {
            if ((!selectedGenres.Contains(viewAllText)) && (!selectedGenres.Contains(song.Genre)))
                return false;

            if ((!selectedArtists.Contains(viewAllText)) && (!selectedArtists.Contains(song.Artist)))
                return false;

            if ((!selectedAlbum.Contains(viewAllText)) && (!selectedAlbum.Contains(song.Album)))
                return false;

            if (!filter.PassMediaItem(song))
                return false;

            return true;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<Song> allSongs = values[0] as ObservableCollection<Song>;

            if (allSongs != null)
            {
                if (allSongs.Count > 0)
                {
                    if ((values[1] != null) && (values[1] != DependencyProperty.UnsetValue))
                    {
                        IntelligentString viewAllText = (IntelligentString)values[1];

                        MediaItemFilter filter = values[2] as MediaItemFilter;

                        ObservableCollection<IntelligentString> selectedGenres = values[3] as ObservableCollection<IntelligentString>;

                        if (selectedGenres != null)
                        {
                            ObservableCollection<IntelligentString> selectedArtists = values[4] as ObservableCollection<IntelligentString>;

                            if (selectedArtists != null)
                            {
                                ObservableCollection<IntelligentString> selectedAlbum = values[5] as ObservableCollection<IntelligentString>;

                                if (selectedAlbum != null)
                                {
                                    ObservableCollection<MediaItem> filteredSongs = new ObservableCollection<MediaItem>(
                                        (from song in allSongs
                                         where PassSong(song, viewAllText, filter, selectedGenres.ToArray(), selectedArtists.ToArray(), selectedAlbum.ToArray())
                                         select song));

                                    return filteredSongs;
                                }
                            }
                        }
                    }
                }
            }

            return new ObservableCollection<MediaItem>();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

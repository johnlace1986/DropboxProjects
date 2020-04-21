﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using MediaPlayer.Library.Business;
using Utilities.Business;
using System.Windows;
using MediaPlayer.Business;

namespace MediaPlayer.ValueConverters
{
    public class AlbumsConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the series of the specified song passes throught the filter and should be displayed
        /// </summary>
        /// <param name="song">Song being checked</param>
        /// <param name="viewAllText">Text used to view all media items</param>
        /// <param name="filter">Filter used to filter songs</param>
        /// <param name="selectedGenres">Genres selected in the filter</param>
        /// <param name="selectedArtists">Artists selected in the filter</param>
        /// <returns>True if the series of the specified song passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassAlbums(Song song, IntelligentString viewAllText, MediaItemFilter filter, IntelligentString[] selectedGenres, IntelligentString[] selectedArtists)
        {
            if (song.Album == IntelligentString.Empty)
                return false;

            if ((!selectedGenres.Contains(viewAllText)) && (!selectedGenres.Contains(song.Genre)))
                return false;

            if ((!selectedArtists.Contains(viewAllText)) && (!selectedArtists.Contains(song.Artist)))
                return false;

            if (!filter.PassMediaItem(song))
                return false;

            return true;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<Song> songs = values[0] as ObservableCollection<Song>;

            if (songs != null)
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
                            List<IntelligentString> series = new List<IntelligentString>(
                                (from song in songs
                                 where PassAlbums(song, viewAllText, filter, selectedGenres.ToArray(), selectedArtists.ToArray())
                                 select IntelligentString.FromString(song.Album.ToString())).Distinct());

                            series.Sort();
                            series.Insert(0, viewAllText);

                            return new ObservableCollection<IntelligentString>(series);
                        }
                    }
                }
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

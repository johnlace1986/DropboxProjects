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
    public class VideosConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the specified video passes throught the filter and should be displayed
        /// </summary>
        /// <param name="video">Video being checked</param>
        /// <param name="viewAllText">Text used to view all media items</param>
        /// <param name="filter">Filter used to filter videos</param>
        /// <param name="selectedGenres">Genres selected in the filter</param>
        /// <param name="selectedPrograms">Programs selected in the filter</param>
        /// <param name="selectedSeries">Series selected in the filter</param>
        /// <returns>True if the specified video passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassVideo(Video video, IntelligentString viewAllText, MediaItemFilter filter, IntelligentString[] selectedGenres, IntelligentString[] selectedPrograms, IntelligentString[] selectedSeries)
        {
            if ((!selectedGenres.Contains(viewAllText)) && (!selectedGenres.Contains(video.Genre)))
                return false;

            if ((!selectedPrograms.Contains(viewAllText)) && (!selectedPrograms.Contains(video.Program)))
                return false;

            if ((!selectedSeries.Contains(viewAllText)) && (!selectedSeries.Contains(video.Series.ToString())))
                return false;

            if (!filter.PassMediaItem(video))
                return false;

            return true;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<Video> allVideos = values[0] as ObservableCollection<Video>;

            if (allVideos != null)
            {
                if ((values[1] != null) && (values[1] != DependencyProperty.UnsetValue))
                {
                    IntelligentString viewAllText = (IntelligentString)values[1];

                    MediaItemFilter filter = values[2] as MediaItemFilter;

                    ObservableCollection<IntelligentString> selectedGenres = values[3] as ObservableCollection<IntelligentString>;

                    if (selectedGenres != null)
                    {
                        ObservableCollection<IntelligentString> selectedPrograms = values[4] as ObservableCollection<IntelligentString>;

                        if (selectedPrograms != null)
                        {
                            ObservableCollection<IntelligentString> selectedSeries = values[5] as ObservableCollection<IntelligentString>;

                            if (selectedSeries != null)
                            {
                                ObservableCollection<MediaItem> filteredVideos = new ObservableCollection<MediaItem>(
                                    (from video in allVideos
                                     where PassVideo(video, viewAllText, filter, selectedGenres.ToArray(), selectedPrograms.ToArray(), selectedSeries.ToArray())
                                     select video));

                                return filteredVideos;
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

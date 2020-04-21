using System;
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
    public class ProgramsConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the program of the specified video passes throught the filter and should be displayed
        /// </summary>
        /// <param name="video">Video being checked</param>
        /// <param name="viewAllText">Text used to view all videos</param>
        /// <param name="filter">Filter used to filter videos</param>
        /// <param name="selectedGenres">Genres selected in the filter</param>
        /// <returns>True if the program of the specified video passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassProgram(Video video, IntelligentString viewAllText, MediaItemFilter filter, IntelligentString[] selectedGenres)
        {
            if (IntelligentString.IsNullOrEmpty(video.Program))
                return false;

            if ((!selectedGenres.Contains(viewAllText)) && (!selectedGenres.Contains(video.Genre)))
                return false;

            if (!filter.PassMediaItem(video))
                return false;

            return true;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<Video> videos = values[0] as ObservableCollection<Video>;

            if (videos != null)
            {
                if ((values[1] != null) && (values[1] != DependencyProperty.UnsetValue))
                {
                    IntelligentString viewAllText = (IntelligentString)values[1];

                    MediaItemFilter filter = values[2] as MediaItemFilter;

                    ObservableCollection<IntelligentString> selectedGenres = values[3] as ObservableCollection<IntelligentString>;

                    if (selectedGenres != null)
                    {
                        List<IntelligentString> programs = new List<IntelligentString>(
                            (from video in videos
                             where PassProgram(video, viewAllText, filter, selectedGenres.ToArray())
                             select video.Program).Distinct());

                        programs.Sort();
                        programs.Insert(0, viewAllText);

                        return new ObservableCollection<IntelligentString>(programs);
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

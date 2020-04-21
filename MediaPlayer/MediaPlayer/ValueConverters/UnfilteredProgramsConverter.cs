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
    public class UnfilteredProgramsConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the program of the specified video passes throught the filter and should be displayed
        /// </summary>
        /// <param name="video">Video being checked</param>
        /// <param name="showHidden">Value determining whether or not hidden videos should be displayed</param>
        /// <returns>True if the program of the specified video passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassProgram(Video video, Boolean? showHidden)
        {
            if (IntelligentString.IsNullOrEmpty(video.Program))
                return false;

            if (showHidden.HasValue)
                if (video.IsHidden != showHidden.Value)
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
                List<Video> lstVideos = new List<Video>(mediaItems.Cast<Video>());

                List<IntelligentString> programs = new List<IntelligentString>(
                    (from video in lstVideos
                     where PassProgram(video, showHidden)
                     select video.Program).Distinct());

                programs.Sort();
                return new ObservableCollection<IntelligentString>(programs);
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

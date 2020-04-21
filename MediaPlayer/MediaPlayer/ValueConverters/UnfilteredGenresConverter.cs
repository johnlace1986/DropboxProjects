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
    public class UnfilteredGenresConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the genre of the specified media item passes throught the filter and should be displayed
        /// </summary>
        /// <param name="mediaItem">Media item being checked</param>
        /// <param name="showHidden">Value determining whether or not hidden videos should be displayed</param>
        /// <returns>True if the genre of the specified media item passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassGenre(MediaItem mediaItem, Boolean? showHidden)
        {
            if (IntelligentString.IsNullOrEmpty(mediaItem.Genre))
                return false;

            if (showHidden.HasValue)
                if (mediaItem.IsHidden != showHidden.Value)
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
                List<MediaItem> lstMediaItems = new List<MediaItem>();

                foreach (MediaItem mediaItem in mediaItems)
                    lstMediaItems.Add(mediaItem);

                List<IntelligentString> genres = new List<IntelligentString>(
                    (from mediaItem in lstMediaItems
                     where PassGenre(mediaItem, showHidden)
                     select mediaItem.Genre).Distinct());

                genres.Sort();
                return new ObservableCollection<IntelligentString>(genres);
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

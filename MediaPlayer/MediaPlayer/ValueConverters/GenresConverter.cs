using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using MediaPlayer.Library.Business;
using Utilities.Business;
using System.Collections;
using System.Windows;
using MediaPlayer.Business;

namespace MediaPlayer.ValueConverters
{
    public class GenresConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Determines whether or not the genre of the specified media item passes throught the filter and should be displayed
        /// </summary>
        /// <param name="mediaItem">Media item being checked</param>
        /// <param name="filter">Filter used to filter media items</param>
        /// <returns>True if the genre of the specified media item passes throught the filter and should be displayed, false if not</returns>
        private static Boolean PassGenre(MediaItem mediaItem, MediaItemFilter filter)
        {
            if (IntelligentString.IsNullOrEmpty(mediaItem.Genre))
                return false;

            if (!filter.PassMediaItem(mediaItem))
                return false;

            return true;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable mediaItems = values[0] as IEnumerable;

            if (mediaItems != null)
            {
                if ((values[1] != null) && (values[1] != DependencyProperty.UnsetValue))
                {
                    IntelligentString viewAllText = (IntelligentString)values[1];

                    MediaItemFilter filter = values[2] as MediaItemFilter;

                    List<MediaItem> lstMediaItems = new List<MediaItem>();

                    foreach (MediaItem mediaItem in mediaItems)
                        lstMediaItems.Add(mediaItem);

                    List<IntelligentString> genres = new List<IntelligentString>(
                        (from mediaItem in lstMediaItems
                         where PassGenre(mediaItem, filter)
                         select mediaItem.Genre).Distinct());

                    genres.Sort();
                    genres.Insert(0, viewAllText);

                    return new ObservableCollection<IntelligentString>(genres);
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

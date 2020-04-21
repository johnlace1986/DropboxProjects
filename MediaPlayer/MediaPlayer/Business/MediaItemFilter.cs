using System;
using System.Linq;
using Utilities.Business;
using MediaPlayer.Library.Business;

namespace MediaPlayer.Business
{
    public class MediaItemFilter : NotifyPropertyChangedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the search text used to filter videos
        /// </summary>
        private String searchText;

        /// <summary>
        /// Gets or sets a value determining whether or not hidden media items should pass the filter
        /// </summary>
        private Boolean? showHidden;

        /// <summary>
        /// Gets or sets a value determining whether or not media items that have a play count of at least 1 should pass the filter
        /// </summary>
        private Boolean? showPlayed;

        /// <summary>
        /// Gets or sets a value determining whether or not media items containing parts with physical files that exist on the system should pass the filter
        /// </summary>
        private Boolean? showPartsExist;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the search text used to filter videos
        /// </summary>
        public String SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not hidden media items should pass the filter
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return showHidden; }
            set
            {
                showHidden = value;
                OnPropertyChanged("ShowHidden");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not media items that have a play count of at least 1 should pass the filter
        /// </summary>
        public Boolean? ShowPlayed
        {
            get { return showPlayed; }
            set
            {
                showPlayed = value;
                OnPropertyChanged("ShowPlayed");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not media items containing parts with physical files that exist on the system should pass the filter
        /// </summary>
        public Boolean? ShowPartsExist
        {
            get { return showPartsExist; }
            set
            {
                showPartsExist = value;
                OnPropertyChanged("ShowPartsExist");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemFilter class
        /// </summary>
        public MediaItemFilter()
            : base()
        {
            SearchText = String.Empty;
            ShowHidden = false;
            ShowPlayed = null;
            ShowPartsExist = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Determines whether or not the specified media item passes the filter
        /// </summary>
        /// <param name="mediaItem">Media item being filtered</param>
        /// <returns>True if the specified media item passes the filter, false if not</returns>
        public Boolean PassMediaItem(MediaItem mediaItem)
        {
            if (ShowHidden.HasValue)
            {
#if !DEBUG
                if (ShowHidden.Value)
                {
                    if ((!mediaItem.IsHidden) || (mediaItem.UserName.ToLower() != Environment.UserDomainName.ToLower() + "\\" + Environment.UserName.ToLower()))
                        return false;
                }
                else
                {
#endif
                    if (mediaItem.IsHidden != ShowHidden.Value)
                    {
                        return false;
                    }
#if !DEBUG
                }
#endif
            }

            if (ShowPlayed.HasValue)
            {
                if (ShowPlayed.Value)
                {
                    if (mediaItem.PlayCount == 0)
                        return false;
                }
                else
                {
                    if (mediaItem.PlayCount > 0)
                        return false;
                }
            }

            if (ShowPartsExist.HasValue)
                if (mediaItem.Parts.PartsExist != ShowPartsExist)
                    return false;

            if (!mediaItem.ContainsSearchText(SearchText))
                return false;

            return true;
        }

        #endregion
    }
}

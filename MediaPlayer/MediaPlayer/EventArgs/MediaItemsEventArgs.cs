using System;
using System.Collections.Generic;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void MediaItemsEventHandler(object sender, MediaItemsEventArgs e);

    public class MediaItemsEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the media items
        /// </summary>
        private readonly List<MediaItem> mediaItems;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the media items
        /// </summary>
        public List<MediaItem> MediaItems
        {
            get { return mediaItems; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemsEventArgs class
        /// </summary>
        /// <param name="mediaItems">Media items</param>
        public MediaItemsEventArgs(IEnumerable<MediaItem> mediaItems)
        {
            this.mediaItems = new List<MediaItem>(mediaItems);
        }

        #endregion
    }
}

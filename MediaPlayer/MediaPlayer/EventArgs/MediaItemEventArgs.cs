using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void MediaItemEventHandler(object sender, MediaItemEventArgs e);

    public class MediaItemEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the meda item
        /// </summary>
        private readonly MediaItem mediaItem;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the media item
        /// </summary>
        public MediaItem MediaItem
        {
            get { return mediaItem; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemEventArgs class
        /// </summary>
        /// <param name="mediaItem">Media item</param>
        public MediaItemEventArgs(MediaItem mediaItem)
        {
            this.mediaItem = mediaItem;
        }

        #endregion
    }
}

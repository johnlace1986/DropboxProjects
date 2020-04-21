using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void PlayMediaItemsEventHandler(object sender, PlayMediaItemsEventArgs e);

    public class PlayMediaItemsEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the media items that are to be played
        /// </summary>
        private readonly MediaItem[] mediaItems;

        /// <summary>
        /// Gets or sets the index in the collection of the first media item to be played
        /// </summary>
        private readonly Int32 selectedIndex;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the media items that are to be played
        /// </summary>
        public MediaItem[] MediaItems
        {
            get { return mediaItems; }
        }

        /// <summary>
        /// Gets the index in the collection of the first media item to be played
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return selectedIndex; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the PlayMediaItemsEventArgs class
        /// </summary>
        /// <param name="mediaItems">Media items that are to be played</param>
        /// <param name="selectedIndex">Index in the collection of the first media item to be played</param>
        public PlayMediaItemsEventArgs(MediaItem[] mediaItems, Int32 selectedIndex)
        {
            this.mediaItems = mediaItems;
            this.selectedIndex = selectedIndex;
        }

        #endregion
    }
}

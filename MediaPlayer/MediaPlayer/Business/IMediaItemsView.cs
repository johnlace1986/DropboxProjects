using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.Business
{
    public interface IMediaItemsView
    {
        #region Properties

        /// <summary>
        /// Gets the media items displayed in the view
        /// </summary>
        MediaItem[] MediaItems { get; }

        /// <summary>
        /// Gets the media items that are currently selected in the view
        /// </summary>
        MediaItem[] SelectedMediaItems { get; }

        /// <summary>
        /// Gets the index of the currently selected media item
        /// </summary>
        Int32 SelectedIndex { get; }

        #endregion
    }
}

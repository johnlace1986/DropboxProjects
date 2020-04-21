using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void CancelMediaItemsOperationEventHandler(object sender, CancelMediaItemsOperationEventArgs e);

    public class CancelMediaItemsOperationEventArgs : MediaItemsEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value determining whether or not the operation should be cancelled
        /// </summary>
        public Boolean Cancel { get; set; }

        /// <summary>
        /// Gets or sets a description of the reason the operation was cancelled
        /// </summary>
        public String ReasonForCancel { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemsEventArgs class
        /// </summary>
        /// <param name="mediaItems">Media items</param>
        public CancelMediaItemsOperationEventArgs(MediaItem[] mediaItems)
            : base(mediaItems)
        {
            Cancel = false;
            ReasonForCancel = String.Empty;
        }

        #endregion
    }
}

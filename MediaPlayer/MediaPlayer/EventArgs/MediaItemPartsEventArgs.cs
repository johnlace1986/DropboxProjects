using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void MediaItemPartsEventHandler(object sender, MediaItemPartsEventArgs e);

    public class MediaItemPartsEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the parts
        /// </summary>
        private readonly MediaItemPart[] parts;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parts
        /// </summary>
        public MediaItemPart[] Parts
        {
            get { return parts; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemPartsEventArgs class
        /// </summary>
        /// <param name="parts">Parts</param>
        public MediaItemPartsEventArgs(MediaItemPart[] parts)
        {
            this.parts = parts;
        }

        #endregion
    }
}

using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void MediaItemPartEventHandler(object sender, MediaItemPartEventArgs e);

    public class MediaItemPartEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the part
        /// </summary>
        private readonly MediaItemPart part;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the part
        /// </summary>
        public MediaItemPart Part
        {
            get { return part; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemPartEventArgs class
        /// </summary>
        /// <param name="part">Part</param>
        public MediaItemPartEventArgs(MediaItemPart part)
        {
            this.part = part;
        }

        #endregion
    }
}

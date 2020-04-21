using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void RootFolderEventHandler(object sender, RootFolderEventArgs e);

    public class RootFolderEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the root folder
        /// </summary>
        private readonly RootFolder rootFolder;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root folder
        /// </summary>
        public RootFolder RootFolder
        {
            get { return rootFolder; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RootFolderEventArgs class
        /// </summary>
        /// <param name="rootFolder">Root folder being passed to the event</param>
        public RootFolderEventArgs(RootFolder rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        #endregion
    }
}

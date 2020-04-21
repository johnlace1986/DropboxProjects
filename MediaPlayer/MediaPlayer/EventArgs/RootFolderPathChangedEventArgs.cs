using System;
using System.Linq;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void RootFolderPathChangedEventHandler(object sender, RootFolderPathChangedEventArgs e);

    public class RootFolderPathChangedEventArgs : RootFolderEventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the previous path of the root folder
        /// </summary>
        private readonly IntelligentString previousPath;        

        #endregion

        #region Properties


        /// <summary>
        /// Gets the previous path of the root folder
        /// </summary>
        public IntelligentString PreviousPath
        {
            get { return previousPath; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RootFolderPathChangedEventArgs class
        /// </summary>
        /// <param name="rootFolder">Root folder whose path changed</param>
        /// <param name="previousPath">Previous path of the root folder</param>
        public RootFolderPathChangedEventArgs(RootFolder rootFolder, IntelligentString previousPath)
            : base(rootFolder)
        {
            this.previousPath = previousPath;
        }

        #endregion
    }
}

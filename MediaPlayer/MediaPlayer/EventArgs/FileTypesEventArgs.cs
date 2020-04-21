using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void FileTypesEventHandler(object sender, FileTypesEventArgs e);

    public class FileTypesEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the file types
        /// </summary>
        private readonly FileType[] fileTypes;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file types
        /// </summary>
        public FileType[] FileTypes
        {
            get { return fileTypes; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileTypesEventArgs class
        /// </summary>
        /// <param name="fileTypes">The file types</param>
        public FileTypesEventArgs(FileType[] fileTypes)
        {
            this.fileTypes = fileTypes;
        }

        #endregion
    }
}

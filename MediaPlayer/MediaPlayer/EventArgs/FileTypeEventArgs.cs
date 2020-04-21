using System;
using System.Linq;
using MediaPlayer.Library.Business;

namespace MediaPlayer.EventArgs
{
    public delegate void FileTypeEventHandler(object sender, FileTypeEventArgs e);

    public class FileTypeEventArgs : System.EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the file type
        /// </summary>
        private readonly FileType fileType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file type
        /// </summary>
        public FileType FileType
        {
            get { return fileType; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileTypeEventArgs class
        /// </summary>
        /// <param name="fileType">The file type</param>
        public FileTypeEventArgs(FileType fileType)
        {
            this.fileType = fileType;
        }

        #endregion
    }
}

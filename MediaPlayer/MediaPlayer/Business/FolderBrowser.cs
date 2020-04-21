using System;
using System.Linq;
using System.Threading;
using System.IO;
using MediaPlayer.Library.Business;
using MediaPlayer.EventArgs;
using System.Windows.Threading;
using Utilities.Exception;
using Utilities.Business;

namespace MediaPlayer.Business
{
    public class FolderBrowser : NotifyPropertyChangedObject
    {
        #region Events

        /// <summary>
        /// Fires when a media item is found
        /// </summary>
        public event MediaItemEventHandler FoundMediaItem;

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the folder path to browse for media files
        /// </summary>
        private String folderPath;

        /// <summary>
        /// Gets or sets the path to the folder currently being browsed
        /// </summary>
        private String selectedFolderPath;
        
        /// <summary>
        /// Gets or sets a value determining whether or not the browser is currently searching for media items
        /// </summary>
        private Boolean isSearching;
        
        /// <summary>
        /// Gets or sets the file types used to determine which files are media files
        /// </summary>
        private FileType[] fileTypes;

        /// <summary>
        /// Gets or sets the thread used to browse for folders
        /// </summary>
        private Thread workerThread;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the folder path to browse for media files
        /// </summary>
        public String FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }

        /// <summary>
        /// Gets or sets the thread used to browse for folders
        /// </summary>
        public String SelectedFolderPath
        {
            get { return selectedFolderPath; }
            private set
            {
                selectedFolderPath = value;
                OnPropertyChanged("SelectedFolderPath");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the browser is currently searching for media items
        /// </summary>
        public Boolean IsSearching
        {
            get { return isSearching; }
            private set 
            {
                isSearching = value;
                OnPropertyChanged("IsSearching");
            }
        }

        /// <summary>
        /// Gets or sets the file types used to determine which files are media files
        /// </summary>
        public FileType[] FileTypes
        {
            get { return fileTypes; }
            set
            {
                fileTypes = value;
                OnPropertyChanged("FileTypes");
            }
        }

        /// <summary>
        /// Get or sets the dispatcher this folder browser is associated with
        /// </summary>
        private Dispatcher Dispatcher { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FolderBrowser class
        /// </summary>
        /// <param name="fileTypes">File types used to determine which files are media files</param>
        public FolderBrowser(FileType[] fileTypes, Dispatcher dispatcher)
        {
            FolderPath = String.Empty;
            SelectedFolderPath = String.Empty;
            IsSearching = false;
            FileTypes = fileTypes;
            Dispatcher = dispatcher;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the FoundMediaItem event
        /// </summary>
        /// <param name="mediaItem">The media item that was found</param>
        private void OnFoundMediaItem(MediaItem mediaItem)
        {
            if (FoundMediaItem != null)
                GeneralMethods.ExecuteDelegateOnGuiThread(Dispatcher, () => FoundMediaItem(this, new MediaItemEventArgs(mediaItem)));
        }

        /// <summary>
        /// Asynchronously browses the specified folder for media files
        /// </summary>
        public void BrowseAsync()
        {
            if (!IsSearching)
            {
                if (!Directory.Exists(FolderPath))
                {
                    GeneralMethods.MessageBoxApplicationError("\"" + FolderPath + "\" does not exist");
                    return;
                }

                IsSearching = true;

                workerThread = new Thread(new ThreadStart(Browse));
                workerThread.Start();
            }
        }

        /// <summary>
        /// Browses the specified folder for media files
        /// </summary>
        private void Browse()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(FolderPath);
                TraverseFolder(di);
            }
            catch (ThreadAbortException)
            {
                SelectedFolderPath = String.Empty;
            }

            SelectedFolderPath = String.Empty;
            IsSearching = false;
        }

        /// <summary>
        /// Travserse the specified folder and looks for media files in it's sub folders
        /// </summary>
        /// <param name="folder">Folder to search</param>
        private void TraverseFolder(DirectoryInfo folder)
        {
            try
            {
                SelectedFolderPath = folder.FullName;

                foreach (FileType fileType in FileTypes)
                {
                    foreach (IntelligentString extension in fileType.Extensions)
                    {
                        foreach (FileInfo fi in folder.GetFiles("*" + extension.Value))
                        {
                            if (!MediaItem.MediaItemPartLocationExists(fi.FullName))
                            {
                                MediaItem mediaItem;

                                switch (fileType.MediaItemType)
                                {
                                    case MediaItemTypeEnum.Song:
                                        mediaItem = new Song();
                                        break;

                                    case MediaItemTypeEnum.Video:
                                        mediaItem = new Video();
                                        break;

                                    default:
                                        throw new UnknownEnumValueException(fileType.MediaItemType);
                                }

                                mediaItem.ParseFilename(fi.FullName);
                                mediaItem.Parts.Add(fi.FullName, fi.Length, MediaItem.GetDuration(fi.FullName));

                                OnFoundMediaItem(mediaItem);
                            }
                        }
                    }
                }

                foreach (DirectoryInfo subFolder in folder.GetDirectories())
                {
                    TraverseFolder(subFolder);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        /// <summary>
        /// Stops the browser from browsing for media files
        /// </summary>
        public void StopBrowsing()
        {
            if (IsSearching)
            {
                workerThread.Abort();
                workerThread = null;

                IsSearching = false;
            }
        }

        #endregion
    }
}

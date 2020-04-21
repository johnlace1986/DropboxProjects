using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Business;
using MediaPlayer.Properties;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace MediaPlayer.Business
{
    [Serializable]
    public class Options : NotifyPropertyChangedObject, ICloneable
    {
        #region Fields

        /// <summary>
        /// Gets or sets the text used to represent "view all" when filtering media items
        /// </summary>
        private IntelligentString viewAllText;

        /// <summary>
        /// Gets or sets a value determining whether or not the media player should be stretched to fit the window it is contained in
        /// </summary>
        private Boolean stretchMediaPlayer;

        /// <summary>
        /// Gets or sets the filenames to ignore when organising a root folder
        /// </summary>
        private ObservableCollection<String> rootFolderFileExceptions;

        /// <summary>
        /// Gets or sets the sub folders to ignore when organising a root folder
        /// </summary>
        private ObservableCollection<String> rootFolderDirectoryExceptions;

        /// <summary>
        /// Gets or sets the volume of the media player
        /// </summary>
        private Double volume;
        
        /// <summary>
        /// Gets or sets a value determining whether or not the remaining time should be displayed rather than the length of the selected media item
        /// </summary>
        private Boolean showTimeRemaining;

        /// <summary>
        /// Gets or sets a value determining whether or not media items should be removed from the Now Playing playlist when they end
        /// </summary>
        private Boolean removeMediaItemFromNowPlayingOnFinish;

        /// <summary>
        /// Gets or sets a value determining whether or not media items that have missing parts should be organised
        /// </summary>
        private Boolean organiseMissingMediaItems;

        /// <summary>
        /// Gets or sets the name of SQL Server service
        /// </summary>
        private String sqlServerServiceName;

        /// <summary>
        /// Gets or sets the prefix of the search string used to search for torrents
        /// </summary>
        private String torrentSearchPrefix;

        /// <summary>
        /// Gets or sets the suffix of the search string used to search for torrents
        /// </summary>
        private String torrentSearchSuffix;

        /// <summary>
        /// Gets or sets the URL of the website used to search for torrents
        /// </summary>
        private String torrentSearchUrl;

        /// <summary>
        /// Gets or sets the character(s) used to represent spaces in the torrent search string
        /// </summary>
        private String torrentSearchSpaceCharacter;

        /// <summary>
        /// Gets or sets a value determining whether or not the media player should pause if the PC is locked
        /// </summary>
        private Boolean pauseOnLock;

        /// <summary>
        /// Gets or sets a value determining whether or not the media player should unpause when PC is unlocked
        /// </summary>
        private Boolean unpauseOnUnlock;

        /// <summary>
        /// Gets or sets a value determining whether or not the window should be minimized when the PC is locked
        /// </summary>
        private Boolean minimizeOnLock;

        /// <summary>
        /// Gets or sets a value determining whether or not the window should be restored when the PC is unlocked
        /// </summary>
        private Boolean restoreOnUnlock;

        /// <summary>
        /// Gets or sets the directories that will be listed as options for exporting media items to
        /// </summary>
        private ObservableCollection<String> exportDirectories;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text used to represent "view all" when filtering media items
        /// </summary>
        public IntelligentString ViewAllText
        {
            get { return viewAllText; }
            set
            {
                viewAllText = value;
                OnPropertyChanged("ViewAllText");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the media player should be stretched to fit the window it is contained in
        /// </summary>
        public Boolean StretchMediaPlayer
        {
            get { return stretchMediaPlayer; }
            set
            {
                stretchMediaPlayer = value;
                OnPropertyChanged("StretchMediaPlayer");
            }
        }

        /// <summary>
        /// Gets or sets the filenames to ignore when organising a root folder
        /// </summary>
        public ObservableCollection<String> RootFolderFileExceptions
        {
            get { return rootFolderFileExceptions; }
            private set
            {
                rootFolderFileExceptions = value;
                OnPropertyChanged("RootFolderFileExceptions");
            }
        }

        /// <summary>
        /// Gets or sets the sub folders to ignore when organising a root folder
        /// </summary>
        public ObservableCollection<String> RootFolderDirectoryExceptions
        {
            get { return rootFolderDirectoryExceptions; }
            private set
            {
                rootFolderDirectoryExceptions = value;
                OnPropertyChanged("RootFolderDirectoryExceptions");
            }
        }

        /// <summary>
        /// Gets or sets the volume of the media player
        /// </summary>
        public Double Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                OnPropertyChanged("Volume");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the remaining time should be displayed rather than the length of the selected media item
        /// </summary>
        public Boolean ShowTimeRemaining
        {
            get { return showTimeRemaining; }
            set
            {
                showTimeRemaining = value;
                OnPropertyChanged("ShowTimeRemaining");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not media items should be removed from the Now Playing playlist when they end
        /// </summary>
        public Boolean RemoveMediaItemFromNowPlayingOnFinish
        {
            get { return removeMediaItemFromNowPlayingOnFinish; }
            set
            {
                removeMediaItemFromNowPlayingOnFinish = value;
                OnPropertyChanged("RemoveMediaItemFromNowPlayingOnFinish");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not media items that have missing parts should be organised
        /// </summary>
        public Boolean OrganiseMissingMediaItems
        {
            get { return organiseMissingMediaItems; }
            set
            {
                organiseMissingMediaItems = value;
                OnPropertyChanged("OrganiseMissingMediaItems");
            }
        }

        /// <summary>
        /// Gets or sets the name of SQL Server service
        /// </summary>
        public String SqlServerServiceName
        {
            get { return sqlServerServiceName; }
            set
            {
                sqlServerServiceName = value;
                OnPropertyChanged("SqlServerServiceName");
            }
        }

        /// <summary>
        /// Gets or sets the URL of the website used to search for torrents
        /// </summary>
        public String TorrentSearchUrl
        {
            get { return torrentSearchUrl; }
            set
            {
                torrentSearchUrl = value;
                OnPropertyChanged("TorrentSearchUrl");
            }
        }

        /// <summary>
        /// Gets or sets the prefix of the search string used to search for torrents
        /// </summary>
        public String TorrentSearchPrefix
        {
            get { return torrentSearchPrefix; }
            set
            {
                torrentSearchPrefix = value;
                OnPropertyChanged("TorrentSearchPrefix");
            }
        }

        /// <summary>
        /// Gets or sets the suffix of the search string used to search for torrents
        /// </summary>
        public String TorrentSearchSuffix
        {
            get { return torrentSearchSuffix; }
            set
            {
                torrentSearchSuffix = value;
                OnPropertyChanged("TorrentSearchSuffix");
            }
        }

        /// <summary>
        /// Gets or sets the character(s) used to represent spaces in the torrent search string
        /// </summary>
        public String TorrentSearchSpaceCharacter
        {
            get { return torrentSearchSpaceCharacter; }
            set
            {
                torrentSearchSpaceCharacter = value;
                OnPropertyChanged("TorrentSearchSpaceCharacter");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the media player should pause if the PC is locked
        /// </summary>
        public Boolean PauseOnLock
        {
            get { return pauseOnLock; }
            set
            {
                pauseOnLock = value;
                OnPropertyChanged("PauseOnLock");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the media player should unpause when PC is unlocked
        /// </summary>
        public Boolean UnpauseOnUnlock
        {
            get { return unpauseOnUnlock; }
            set
            {
                unpauseOnUnlock = value;
                OnPropertyChanged("UnpauseOnUnlock");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the window should be minimized when the PC is locked
        /// </summary>
        public Boolean MinimizeOnLock
        {
            get { return minimizeOnLock; }
            set
            {
                minimizeOnLock = value;
                OnPropertyChanged("MinimizeOnLock");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the window should be restored when the PC is unlocked
        /// </summary>
        public Boolean RestoreOnUnlock
        {
            get { return restoreOnUnlock; }
            set
            {
                restoreOnUnlock = value;
                OnPropertyChanged("RestoreOnUnlock");
            }
        }

        /// <summary>
        /// Gets or sets the directories that will be listed as options for exporting media items to
        /// </summary>
        public ObservableCollection<String> ExportDirectories
        {
            get { return exportDirectories; }
            private set
            {
                exportDirectories = value;
                OnPropertyChanged("ExportDirectories");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Options class
        /// </summary>
        public Options()
        {
            ViewAllText = Settings.Default.ViewAllText;
            StretchMediaPlayer = Settings.Default.StretchMediaPlayer;
            RootFolderFileExceptions = (Settings.Default.RootFolderFileExceptions == null ? new ObservableCollection<String>() : new ObservableCollection<String>(Settings.Default.RootFolderFileExceptions.ToEnumerable()));
            RootFolderDirectoryExceptions = (Settings.Default.RootFolderDirectoryExceptions == null ? new ObservableCollection<String>() : new ObservableCollection<String>(Settings.Default.RootFolderDirectoryExceptions.ToEnumerable()));
            Volume = Settings.Default.Volume;
            ShowTimeRemaining = Settings.Default.ShowTimeRemaining;
            RemoveMediaItemFromNowPlayingOnFinish = Settings.Default.RemoveMediaItemFromNowPlayingOnFinish;
            OrganiseMissingMediaItems = Settings.Default.OrganiseMissingMediaItems;
            SqlServerServiceName = Settings.Default.SqlServerServiceName;
            TorrentSearchUrl = Settings.Default.TorrentSearchUrl;
            TorrentSearchPrefix = Settings.Default.TorrentSearchPrefix;
            TorrentSearchSuffix = Settings.Default.TorrentSearchSuffix;
            TorrentSearchSpaceCharacter = Settings.Default.TorrentSearchSpaceCharacter;
            PauseOnLock = Settings.Default.PauseOnLock;
            UnpauseOnUnlock = Settings.Default.UnpauseOnUnlock;
            MinimizeOnLock = Settings.Default.MinimizeOnLock;
            RestoreOnUnlock = Settings.Default.RestoreOnUnlock;
            ExportDirectories = (Settings.Default.ExportDirectories == null ? new ObservableCollection<String>() : new ObservableCollection<String>(Settings.Default.ExportDirectories.ToEnumerable()));
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Saves the settings
        /// </summary>
        public void Save()
        {
            Settings.Default.ViewAllText = ViewAllText.Value;
            Settings.Default.StretchMediaPlayer = StretchMediaPlayer;
            Settings.Default.RootFolderFileExceptions = RootFolderFileExceptions.ToStringCollection();
            Settings.Default.RootFolderDirectoryExceptions = RootFolderDirectoryExceptions.ToStringCollection();
            Settings.Default.Volume = Volume;
            Settings.Default.ShowTimeRemaining = ShowTimeRemaining;
            Settings.Default.RemoveMediaItemFromNowPlayingOnFinish = RemoveMediaItemFromNowPlayingOnFinish;
            Settings.Default.OrganiseMissingMediaItems = OrganiseMissingMediaItems;
            Settings.Default.SqlServerServiceName = SqlServerServiceName;
            Settings.Default.TorrentSearchUrl = TorrentSearchUrl;
            Settings.Default.TorrentSearchPrefix = TorrentSearchPrefix;
            Settings.Default.TorrentSearchSuffix = TorrentSearchSuffix;
            Settings.Default.TorrentSearchSpaceCharacter = TorrentSearchSpaceCharacter;
            Settings.Default.PauseOnLock = PauseOnLock;
            Settings.Default.UnpauseOnUnlock = UnpauseOnUnlock;
            Settings.Default.MinimizeOnLock = MinimizeOnLock;
            Settings.Default.RestoreOnUnlock = RestoreOnUnlock;
            Settings.Default.ExportDirectories = ExportDirectories.ToStringCollection();

            Settings.Default.Save();
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return GeneralMethods.Clone<Options>(this);
        }

        #endregion
    }

    static class StringCollectionExtensions
    {
        public static IEnumerable<String> ToEnumerable(this StringCollection collection)
        {
            return collection.Cast<String>();
        }

        public static StringCollection ToStringCollection(this IEnumerable<String> enumberable)
        {
            List<String> list = new List<String>(enumberable);

            StringCollection collection = new StringCollection();
            collection.AddRange(list.ToArray());

            return collection;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MediaPlayer.Business;
using System.Collections.ObjectModel;
using MediaPlayer.Library.Business;
using System.DirectoryServices.AccountManagement;
using Utilities.Exception;
using MediaPlayer.EventArgs;
using System.ComponentModel;
using MediaPlayer.Presentation.Windows;
using System.Windows.Interop;
using Utilities.Business;
using Utilities.Presentation.WPF.Windows;
using System.ServiceProcess;
using System.Diagnostics;
using MediaPlayer.Properties;
using System.Threading;
using iTunesLib;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MediaPlayer.xaml
    /// </summary>
    public partial class MediaPlayerDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Options), typeof(MediaPlayerDialog), new PropertyMetadata(new Options(), new PropertyChangedCallback(OnOptionsPropertyChanged)));

        public static readonly DependencyProperty VideosProperty = DependencyProperty.Register("Videos", typeof(ObservableCollection<Video>), typeof(MediaPlayerDialog), new PropertyMetadata(new ObservableCollection<Video>()));

        public static readonly DependencyProperty SongsProperty = DependencyProperty.Register("Songs", typeof(ObservableCollection<Song>), typeof(MediaPlayerDialog), new PropertyMetadata(new ObservableCollection<Song>()));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(MediaItemFilter), typeof(MediaPlayerDialog), new PropertyMetadata(new MediaItemFilter()));

        public static readonly DependencyProperty VideoRootFoldersProperty = DependencyProperty.Register("VideoRootFolders", typeof(RootFolder[]), typeof(MediaPlayerDialog), new PropertyMetadata(new RootFolder[0]));

        public static readonly DependencyProperty SongRootFoldersProperty = DependencyProperty.Register("SongRootFolders", typeof(RootFolder[]), typeof(MediaPlayerDialog), new PropertyMetadata(new RootFolder[0]));

        public static readonly DependencyProperty VideoFileTypesProperty = DependencyProperty.Register("VideoFileTypes", typeof(FileType[]), typeof(MediaPlayerDialog), new PropertyMetadata(new FileType[0]));

        public static readonly DependencyProperty SongFileTypesProperty = DependencyProperty.Register("SongFileTypes", typeof(FileType[]), typeof(MediaPlayerDialog), new PropertyMetadata(new FileType[0]));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(MediaPlayerDialog));

        public static readonly DependencyProperty PlayStateProperty = DependencyProperty.Register("PlayState", typeof(PlayStateEnum), typeof(MediaPlayerDialog));

        public static readonly DependencyProperty CanSkipPreviousProperty = DependencyProperty.Register("CanSkipPrevious", typeof(Boolean), typeof(MediaPlayerDialog));

        public static readonly DependencyProperty CanSkipNextProperty = DependencyProperty.Register("CanSkipNext", typeof(Boolean), typeof(MediaPlayerDialog));

        public static readonly DependencyProperty CurrentlyPlayingMediaItemProperty = DependencyProperty.Register("CurrentlyPlayingMediaItem", typeof(MediaItem), typeof(MediaPlayerDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(TimeSpan), typeof(MediaPlayerDialog));

        public static readonly DependencyProperty DisableTrackerTimerProperty = DependencyProperty.Register("DisableTrackerTimer", typeof(Boolean), typeof(MediaPlayerDialog));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets a value determining whether or not a new media item should have all of it's properties extracted from the filename
        /// </summary>
        private Boolean fullParseFilename = true;

        /// <summary>
        /// Gets or sets the collection of iTunes IDs of songs that have been deleted from the database since the last sync with iTunes
        /// </summary>
        private DeletedSongITunesIdCollection deletedSongITunesIDs;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the options currently saved in the system
        /// </summary>
        public Options Options
        {
            get { return GetValue(MediaPlayerDialog.OptionsProperty) as Options; }
            set { SetValue(MediaPlayerDialog.OptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the videos in the system
        /// </summary>
        public ObservableCollection<Video> Videos
        {
            get { return GetValue(MediaPlayerDialog.VideosProperty) as ObservableCollection<Video>; }
            set { SetValue(MediaPlayerDialog.VideosProperty, value); }
        }

        /// <summary>
        /// Gets or sets the songs in the system
        /// </summary>
        public ObservableCollection<Song> Songs
        {
            get { return GetValue(MediaPlayerDialog.SongsProperty) as ObservableCollection<Song>; }
            set { SetValue(MediaPlayerDialog.SongsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the global filter used to filter all media items in the system
        /// </summary>
        public MediaItemFilter Filter
        {
            get { return GetValue(FilterProperty) as MediaItemFilter; }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root folders associated with videos currently in the system
        /// </summary>
        public RootFolder[] VideoRootFolders
        {
            get { return GetValue(MediaPlayerDialog.VideoRootFoldersProperty) as RootFolder[]; }
            set { SetValue(MediaPlayerDialog.VideoRootFoldersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root folders associated with songs currently in the system
        /// </summary>
        public RootFolder[] SongRootFolders
        {
            get { return GetValue(MediaPlayerDialog.SongRootFoldersProperty) as RootFolder[]; }
            set { SetValue(MediaPlayerDialog.SongRootFoldersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the file types associated with videos currently in the system
        /// </summary>
        public FileType[] VideoFileTypes
        {
            get { return GetValue(VideoFileTypesProperty) as FileType[]; }
            set { SetValue(VideoFileTypesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the file types associated with songs currently in the system
        /// </summary>
        public FileType[] SongFileTypes
        {
            get { return GetValue(SongFileTypesProperty) as FileType[]; }
            set { SetValue(SongFileTypesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the system is currently organising media items
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(MediaPlayerDialog.IsOrganisingProperty); }
            set { SetValue(MediaPlayerDialog.IsOrganisingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not there is a media item after the selected one in the collection
        /// </summary>
        public Boolean CanSkipPrevious
        {
            get { return (Boolean)GetValue(MediaPlayerDialog.CanSkipPreviousProperty); }
            private set { SetValue(MediaPlayerDialog.CanSkipPreviousProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not there is a media item before the selected one in the collection
        /// </summary>
        public Boolean CanSkipNext
        {
            get { return (Boolean)GetValue(MediaPlayerDialog.CanSkipNextProperty); }
            private set { SetValue(MediaPlayerDialog.CanSkipNextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current play state of the application
        /// </summary>
        public PlayStateEnum PlayState
        {
            get { return (PlayStateEnum)GetValue(PlayStateProperty); }
            set { SetValue(PlayStateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the media item that is currently playing in the application
        /// </summary>
        public MediaItem CurrentlyPlayingMediaItem
        {
            get { return GetValue(CurrentlyPlayingMediaItemProperty) as MediaItem; }
            set { SetValue(CurrentlyPlayingMediaItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets current position of progress through the currently playing media item's playback time
        /// </summary>
        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(MediaPlayerDialog.PositionProperty); }
            set { SetValue(MediaPlayerDialog.PositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the tracker time should be disabled
        /// </summary>
        public Boolean DisableTrackerTimer
        {
            get { return (Boolean)GetValue(DisableTrackerTimerProperty); }
            set { SetValue(DisableTrackerTimerProperty, value); }
        }

        public IMediaItemsView SelectedMediaItemsView
        {
            get
            {
                TabItem selectedTabItem = tabPages.SelectedItem as TabItem;

                if (selectedTabItem != null)
                    return selectedTabItem.Content as IMediaItemsView;

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaPlayerDialog class
        /// </summary>
        public MediaPlayerDialog()
        {
            InitializeComponent();

            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Checks the SQL Server service is running and gives the user the option to start it if it isn't
        /// </summary>
        /// <returns>True if the SQL Server service is running, false if not</returns>
        private Boolean CheckSqlServerServiceRunning()
        {
            ServiceController service = new ServiceController(Settings.Default.SqlServerServiceName, Environment.MachineName);

            if (service.Status == ServiceControllerStatus.Stopped)
            {
                if (MessageBox.Show("SQL Server service is not running. Would you like to start it?", Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.Verb = "runas";
                    proc.FileName = "net";
                    proc.Arguments = "start " + Settings.Default.SqlServerServiceName;

                    Process.Start(proc);

                    Process[] processes;

                    do
                    {
                        processes = Process.GetProcessesByName("net", Environment.MachineName);
                    }
                    while (processes.Length > 0);
                }
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks the SQL Server service is stopped and gives the user the option to stop it if it isn't
        /// </summary>
        /// <returns>Value determining whether or not the application should continue closing</returns>
        private Boolean CheckSqlServerServiceStopped()
        {
            ServiceController service = new ServiceController(Settings.Default.SqlServerServiceName, Environment.MachineName);

            if (service.Status != ServiceControllerStatus.Stopped)
            {
                MessageBoxResult result = MessageBox.Show("SQL Server service is running. Would you like to stop it?", Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        return false;

                    case MessageBoxResult.Yes:

                        ProcessStartInfo proc = new ProcessStartInfo();
                        proc.Verb = "runas";
                        proc.FileName = "net";
                        proc.Arguments = "stop " + Settings.Default.SqlServerServiceName;

                        Process.Start(proc);

                        Process[] processes;

                        do
                        {
                            processes = Process.GetProcessesByName("net", Environment.MachineName);
                        }
                        while (processes.Length > 0);
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds a new media item to the system
        /// </summary>
        /// <param name="mediaItem">Media item being added to the system</param>
        /// <returns>True if the media item was successfully added to the system, false if not</returns>
        private Boolean AddMediaItem(MediaItem mediaItem)
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before adding any media items");
                    return false;
                }

                MediaItemDialog mediaItemDialog = new MediaItemDialog(fullParseFilename, mediaItem);
                mediaItemDialog.Owner = Application.Current.MainWindow;
                mediaItemDialog.ShowHidden = Filter.ShowHidden;
                mediaItemDialog.FileTypeAdded += element_FileTypeAdded;

                if (GeneralMethods.GetNullableBoolValue(mediaItemDialog.ShowDialog()))
                {
                    mediaItemDialog.SelectedMediaItem.Save();
                    mediaItem.CopyFromClone(mediaItemDialog.SelectedMediaItem);

                    switch (mediaItem.Type)
                    {
                        case MediaItemTypeEnum.Video:

                            //remember sort descriptions
                            List<SortDescription> videoDescriptions = new List<SortDescription>(pgVideos.SortDescriptions);

                            List<Video> videos = new List<Video>(Videos);
                            videos.Add(mediaItemDialog.SelectedMediaItem as Video);
                            videos.Sort();

                            Videos = new ObservableCollection<Video>(videos);

                            pgVideos.SortDescriptions.Clear();

                            foreach (SortDescription sortDescription in videoDescriptions)
                                pgVideos.SortDescriptions.Add(sortDescription);

                            break;

                        case MediaItemTypeEnum.Song:

                            //remember sort descriptions
                            List<SortDescription> songDescriptions = new List<SortDescription>(pgSongs.SortDescriptions);

                            List<Song> songs = new List<Song>(Songs);
                            songs.Add(mediaItemDialog.SelectedMediaItem as Song);
                            songs.Sort();

                            Songs = new ObservableCollection<Song>(songs);

                            pgSongs.SortDescriptions.Clear();

                            foreach (SortDescription sortDescription in songDescriptions)
                                pgSongs.SortDescriptions.Add(sortDescription);

                            break;

                        default:
                            throw new UnknownEnumValueException(mediaItem.Type);
                    }

                    return true;
                }
            }
            catch (System.Exception ex)
            {
                GeneralMethods.MessageBoxException(ex, "Could not add " + mediaItem.Type.ToString().ToLower() + ": ");
            }

            return false;
        }

        /// <summary>
        /// Adds the media files found in a specified folder to the system
        /// </summary>
        private void AddFolder()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before adding any media items");
                    return;
                }

                //remember sort descriptions
                List<SortDescription> videoDescriptions = new List<SortDescription>(pgVideos.SortDescriptions);

                //merge file types
                List<FileType> fileTypes = new List<FileType>();

                fileTypes.AddRange(VideoFileTypes);
                fileTypes.AddRange(SongFileTypes);

                AddFolderDialog afd = new AddFolderDialog(fileTypes.ToArray());
                afd.Owner = Application.Current.MainWindow;
                afd.Filter = Filter;
                afd.FileTypeAdded += new FileTypeEventHandler(element_FileTypeAdded);

                if (GeneralMethods.GetNullableBoolValue(afd.ShowDialog()))
                {
                    List<MediaItem> addedMediaItems = new List<MediaItem>(afd.AddedMediaItems);

                    if (afd.AddedMediaItems.Any(p => p.Type == MediaItemTypeEnum.Video))
                    {
                        List<Video> videos = new List<Video>(Videos);

                        foreach (MediaItem mediaItem in afd.AddedMediaItems.Where(p => p.Type == MediaItemTypeEnum.Video))
                        {
                            mediaItem.Save();
                            videos.Add(mediaItem as Video);
                            addedMediaItems.Remove(mediaItem);
                        }

                        videos.Sort();
                        Videos = new ObservableCollection<Video>(videos);

                        pgVideos.SortDescriptions.Clear();

                        foreach (SortDescription sortDescription in videoDescriptions)
                            pgVideos.SortDescriptions.Add(sortDescription);
                    }

                    if (afd.AddedMediaItems.Any(p => p.Type == MediaItemTypeEnum.Song))
                    {
                        List<Song> songs = new List<Song>(Songs);

                        foreach (MediaItem mediaItem in afd.AddedMediaItems.Where(p => p.Type == MediaItemTypeEnum.Song))
                        {
                            mediaItem.Save();
                            songs.Add(mediaItem as Song);
                            addedMediaItems.Remove(mediaItem);
                        }

                        songs.Sort();
                        Songs = new ObservableCollection<Song>(songs);
                    }

                    if (addedMediaItems.Count != 0)
                        throw new UnknownEnumValueException(addedMediaItems[0].Type);
                }
            }
            catch (System.Exception ex)
            {
                GeneralMethods.MessageBoxException(ex, "Could not add folder: ");
            }
        }

        /// <summary>
        /// Prompts the user for their windows password before setting the value of the ShowHidden property
        /// </summary>
        /// <param name="desiredShowHiddenValue">Value to set ShowHidden to if the password is correct</param>
        /// <returns>True if the ShowHidden property was set, false if not</returns>
        private Boolean CheckPasswordSetShowHidden(Boolean? desiredShowHiddenValue)
        {
            PrincipalContext pc = null;

            try
            {
                //ask for password if hidden videos are NOT currently shown (ShowHidden == false)
                if (Filter.ShowHidden == false)
                {
                    do
                    {
                        GetTextValueDialog gtvd = new GetTextValueDialog(TextValueDialogInputType.PasswordBox);
                        gtvd.Style = (Style)FindResource("dialogWindow");
                        gtvd.Owner = Application.Current.MainWindow;
                        gtvd.Title = "Enter Password";
                        gtvd.Header = "Enter your Windows password:";
                        gtvd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                        gtvd.Width = 400;

                        if (GeneralMethods.GetNullableBoolValue(gtvd.ShowDialog()))
                        {
                            if (pc == null)
                                pc = new PrincipalContext(UserPrincipal.Current.ContextType, Environment.UserDomainName);

                            Boolean isValid = pc.ValidateCredentials(Environment.UserName, gtvd.Value);

                            if (isValid)
                            {
                                Filter.ShowHidden = desiredShowHiddenValue;
                                return true;
                            }
                            else
                                GeneralMethods.MessageBoxApplicationError("Incorrect password");
                        }
                        else
                            return false;
                    }
                    while (true);
                }

                return true;
            }
            finally
            {
                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Plays the specified media items
        /// </summary>
        /// <param name="mediaItems">Media items to be played</param>
        /// <param name="selectedIndex">Index within the collection of media items to start playing from</param>
        private void PlayMediaItems(MediaItem[] mediaItems, Int32 selectedIndex)
        {
            pgNowPlaying.LoadMediaItems(mediaItems, selectedIndex);
            pgNowPlaying.Play();

            if (PlayState == PlayStateEnum.Playing)
                tabNowPlaying.IsSelected = true;
        }

        /// <summary>
        /// Adds the media items to the "Now Playing..." playlist
        /// </summary>
        /// <param name="mediaItems">Media items being added to the "Now Playing..." playlist</param>
        private void AddToNowPlaying(IEnumerable<MediaItem> mediaItems)
        {
            pgNowPlaying.AddMediaItems(mediaItems);
        }

        /// <summary>
        /// Skips to the previous media item in the Now Playing playlist
        /// </summary>
        private void SkipPrevious()
        {
            pgNowPlaying.SkipPrevious();
        }

        /// <summary>
        /// Toggles the play state of the media player between play and pause
        /// </summary>
        private void TogglePlayPause()
        {
            if ((CurrentlyPlayingMediaItem != null) || (pgNowPlaying.MediaItems.Count != 0))
                pgNowPlaying.TogglePlayPause();
            else
            {
                IMediaItemsView selectedMediaItemsView = SelectedMediaItemsView;

                if (selectedMediaItemsView != null)
                {
                    MediaItem[] selectedMediaItems = selectedMediaItemsView.SelectedMediaItems;
                    Int32 selectedIndex = selectedMediaItemsView.SelectedIndex;

                    if (selectedMediaItems.Length > 1)
                        selectedIndex = 0;
                    else
                        selectedMediaItems = selectedMediaItemsView.MediaItems;

                    PlayMediaItems(selectedMediaItems, selectedIndex);
                }
            }
        }

        /// <summary>
        /// Stops the media player
        /// </summary>
        private void Stop()
        {
            pgNowPlaying.Stop();
        }

        /// <summary>
        /// Skips to the next media item in the Now Playing playlist
        /// </summary>
        private void SkipNext()
        {
            pgNowPlaying.SkipNext();
        }

        /// <summary>
        /// Saves the media item to the database
        /// </summary>
        /// <param name="mediaItem">Media item being saved to the database</param>
        private void SaveMediaItem(MediaItem mediaItem)
        {
            SaveMediaItems(new MediaItem[1] { mediaItem });
        }

        /// <summary>
        /// Saves the media items to the database
        /// </summary>
        /// <param name="mediaItems">Media items being saved to the database</param>
        private void SaveMediaItems(IEnumerable<MediaItem> mediaItems)
        {
            Dictionary<MediaItemTypeEnum, List<MediaItem>> current = new Dictionary<MediaItemTypeEnum, List<MediaItem>>();
            current.Add(MediaItemTypeEnum.Video, new List<MediaItem>(Videos));
            current.Add(MediaItemTypeEnum.Song, new List<MediaItem>(Songs));

            Dictionary<MediaItemTypeEnum, Boolean> changed = new Dictionary<MediaItemTypeEnum, Boolean>();
            changed.Add(MediaItemTypeEnum.Video, false);
            changed.Add(MediaItemTypeEnum.Song, false);

            foreach (MediaItem mediaItem in mediaItems)
            {
                if (!current.ContainsKey(mediaItem.Type))
                    throw new UnknownEnumValueException(mediaItem.Type);

                mediaItem.Save();

                if (!current[mediaItem.Type].Contains(mediaItem))
                    current[mediaItem.Type].Add(mediaItem);

                changed[mediaItem.Type] = true;
            }

            if (changed[MediaItemTypeEnum.Video])
            {
                current[MediaItemTypeEnum.Video].Sort();
                Videos = new ObservableCollection<Video>(current[MediaItemTypeEnum.Video].Cast<Video>());
            }

            if (changed[MediaItemTypeEnum.Song])
            {
                current[MediaItemTypeEnum.Song].Sort();
                Songs = new ObservableCollection<Song>(current[MediaItemTypeEnum.Song].Cast<Song>());
            }
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Boolean keepChecking = true;

            while (keepChecking)
            {
                try
                {
                    Options = new Options();

                    //check SQL Server service is running before loading data from the database
                    if (CheckSqlServerServiceRunning())
                    {
                        deletedSongITunesIDs = new DeletedSongITunesIdCollection();

                        MediaItem[] items = MediaItem.GetMediaItems();

                        IEnumerable<MediaItem> videos = items.Where(p => p.Type == MediaItemTypeEnum.Video);
                        Videos = new ObservableCollection<Video>(videos.Cast<Video>());

                        IEnumerable<MediaItem> songs = items.Where(p => p.Type == MediaItemTypeEnum.Song);
                        Songs = new ObservableCollection<Song>(songs.Cast<Song>());

                        VideoRootFolders = RootFolder.GetRootFoldersByType(MediaItemTypeEnum.Video);
                        SongRootFolders = RootFolder.GetRootFoldersByType(MediaItemTypeEnum.Song);

                        FileType[] fileTypes = FileType.GetFileTypes();

                        IEnumerable<FileType> videoFileTypes = fileTypes.Where(p => p.MediaItemType == MediaItemTypeEnum.Video);
                        VideoFileTypes = videoFileTypes.ToArray();

                        IEnumerable<FileType> songFileTypes = fileTypes.Where(p => p.MediaItemType == MediaItemTypeEnum.Song);
                        SongFileTypes = songFileTypes.ToArray();
                    }

                    keepChecking = false;
                }
                catch (System.Exception ex)
                {
                    if (GeneralMethods.MessageBox("Could not load library: " + ex.Message + Environment.NewLine + Environment.NewLine + "Would you like to retry?", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No)
                        keepChecking = false;
                }
            }
        }
        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (IsOrganising)
            {
                if (GeneralMethods.MessageBox("The library is currently being organised. Do you wish to cancel and close?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    pgOrganising.LibraryOrganiser.StopOrganising();
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (PlayState == PlayStateEnum.Playing)
            {
                if (GeneralMethods.MessageBox("Are you sure you want to quit while a media item is playing?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    pgNowPlaying.Stop();
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (!CheckSqlServerServiceStopped())
            {
                e.Cancel = true;
                return;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            DependencyObject originalSource = e.OriginalSource as DependencyObject;

            if ((originalSource is TextBoxBase) || (VisualTreeHelpers.FindAncestor<TextBox>(originalSource) != null))
                return;

            if (sender == this)
            {
                switch (e.Key)
                {
                    case Key.Left:
                    case Key.MediaPreviousTrack:
                        SkipPrevious();
                        break;

                    case Key.Space:
                    case Key.MediaPlayPause:
                        TogglePlayPause();
                        break;

                    case Key.MediaStop:
                        Stop();
                        break;

                    case Key.Right:
                    case Key.MediaNextTrack:
                        SkipNext();
                        break;
                }
            }
        }

        private void Options_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Options.Save();
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:

                    if (PlayState == PlayStateEnum.Playing)
                        if (Options.PauseOnLock)
                            pgNowPlaying.TogglePlayPause();

                    if (Options.MinimizeOnLock)
                        WindowState = System.Windows.WindowState.Minimized;

                    break;

                case SessionSwitchReason.SessionUnlock:

                    if (PlayState == PlayStateEnum.Paused)
                        if (Options.UnpauseOnUnlock)
                            pgNowPlaying.TogglePlayPause();

                    if (Options.RestoreOnUnlock)
                        WindowState = System.Windows.WindowState.Normal;

                    break;
            }
        }

        private void miNewVideo_Click(object sender, RoutedEventArgs e)
        {
            Video video = new Video();
            AddMediaItem(video);
        }

        private void miNewSong_Click(object sender, RoutedEventArgs e)
        {
            Song song = new Song();
            AddMediaItem(song);
        }

        private void miNewFolder_Click(object sender, RoutedEventArgs e)
        {
            AddFolder();
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void miHiddenYes_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!CheckPasswordSetShowHidden(true))
                e.Handled = true;
        }

        private void miHiddenAll_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!CheckPasswordSetShowHidden(null))
                e.Handled = true;
        }

        private void miOrganiseLibrary_Click(object sender, RoutedEventArgs e)
        {
            if (pgOrganising.PartsToOrganise.Length == 0)
            {
                pgOrganising.CleanUp();
                GeneralMethods.MessageBox("The library is organised", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                pgOrganising.Start();
                tabOrganising.IsSelected = true;
            }
        }

        private void miViewMissingEpisodes_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Video> filteredVideos = Videos.Where(p => Filter.PassMediaItem(p));
            List<Video> missing = new List<Video>();

            var genres =
                from video in filteredVideos
                group video by video.Genre into groups
                select new
                {
                    Key = groups.Key,
                    Value = groups
                };

            foreach (var genre in genres)
            {
                var programs =
                    from video in genre.Value
                    group video by video.Program into groups
                    select new
                    {
                        Key = groups.Key,
                        Value = groups
                    };

                foreach (var program in programs)
                {
                    var series =
                        from video in program.Value
                        group video by video.Series into groups
                        select new
                        {
                            Key = groups.Key,
                            Value = groups
                        };

                    foreach (var currentSeries in series)
                    {
                        IEnumerable<Video> videos = currentSeries.Value.Where(p => p.NumberOfEpisodes != 0);

                        if (videos.Count() > 0)
                        {
                            Int16[] maxGroup =
                                (from video in videos
                                 group video by video.NumberOfEpisodes into groups
                                 select groups.Key).OrderBy(p => p).Reverse().ToArray<Int16>();

                            Int16 numberOfEpisodes = maxGroup[0];

                            for (Int16 i = 1; i <= numberOfEpisodes; i++)
                            {
                                if (!videos.Any(p => p.Episode == i))
                                {
                                    missing.Add(new Video()
                                    {
                                         Genre = genre.Key,
                                         Program = program.Key,
                                         Series = currentSeries.Key,
                                         Episode = i,
                                         NumberOfEpisodes = numberOfEpisodes,
                                         Name = "Episode " + i.ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }

            missing.Sort();

            Style dataGridNumberCellStyle = (Style)FindResource("dataGridNumberCellStyle");

            MissingMediaItemsDialog missingEpisodes = new MissingMediaItemsDialog(missing, Options);
            missingEpisodes.Owner = this;
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name") { Mode = BindingMode.OneWay } });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Program", Binding = new Binding("Program") { Mode = BindingMode.OneWay } });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Episode", Binding = new Binding("EpisodeOfString") { Mode = BindingMode.OneWay }, CellStyle = dataGridNumberCellStyle });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Series", Binding = new Binding("SeriesString") { Mode = BindingMode.OneWay }, CellStyle = dataGridNumberCellStyle });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Genre", Binding = new Binding("Genre") { Mode = BindingMode.OneWay } });
            missingEpisodes.Columns.Add((DataGridTemplateColumn)missingEpisodes.FindResource("dgtcTorrentSearch"));
            missingEpisodes.Browse += missingEpisodes_Browse;

            missingEpisodes.ShowDialog();
        }

        private void miViewMissingTracks_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Song> filteredSongs = Songs.Where(p => Filter.PassMediaItem(p));
            List<Song> missing = new List<Song>();

            var genres =
                from song in filteredSongs
                group song by song.Genre into groups
                select new
                {
                    Key = groups.Key,
                    Value = groups
                };

            foreach (var genre in genres)
            {
                var artists =
                    from song in genre.Value
                    group song by song.Artist into groups
                    select new
                    {
                        Key = groups.Key,
                        Value = groups
                    };

                foreach (var artist in artists)
                {
                    var albums =
                        from song in artist.Value
                        group song by song.Album into groups
                        select new
                        {
                            Key = groups.Key,
                            Value = groups
                        };

                    foreach (var album in albums)
                    {
                        var disks =
                            from song in album.Value
                            group song by song.DiskNumber into groups
                            select new
                            {
                                Key = groups.Key,
                                Value = groups
                            };

                        foreach (var disk in disks)
                        {
                            IEnumerable<Song> songs = disk.Value.Where(p => p.NumberOfTracks != 0);

                            if (songs.Count() > 0)
                            {
                                Int16[] maxGroup =
                                    (from song in songs
                                     group song by song.NumberOfTracks into groups
                                     select groups.Key).OrderBy(p => p).Reverse().ToArray<Int16>();

                                Int16 numerOfTracks = maxGroup[0];

                                for (Int16 i = 1; i <= numerOfTracks; i++)
                                {
                                    if (!songs.Any(p => p.TrackNumber == i))
                                    {
                                        missing.Add(new Song()
                                        {
                                            Genre = genre.Key,
                                            Artist = artist.Key,
                                            Album = album.Key,
                                            DiskNumber = disk.Key,
                                            TrackNumber = i,
                                            NumberOfTracks = numerOfTracks,
                                            Name = "Track " + i.ToString()
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            missing.Sort();

            Style dataGridNumberCellStyle = (Style)FindResource("dataGridNumberCellStyle");

            MissingMediaItemsDialog missingEpisodes = new MissingMediaItemsDialog(missing, Options);
            missingEpisodes.Owner = this;
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name") { Mode = BindingMode.OneWay } });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Artist", Binding = new Binding("Artist") { Mode = BindingMode.OneWay } });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Album", Binding = new Binding("Album") { Mode = BindingMode.OneWay } });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Track", Binding = new Binding("TrackNumberOfString") { Mode = BindingMode.OneWay }, CellStyle = dataGridNumberCellStyle });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Disk", Binding = new Binding("DiskNumberOfString") { Mode = BindingMode.OneWay }, CellStyle = dataGridNumberCellStyle });
            missingEpisodes.Columns.Add(new DataGridTextColumn { Header = "Genre", Binding = new Binding("Genre") { Mode = BindingMode.OneWay } });
            missingEpisodes.Columns.Add((DataGridTemplateColumn)missingEpisodes.FindResource("dgtcTorrentSearch"));
            missingEpisodes.Browse += missingEpisodes_Browse;

            missingEpisodes.ShowDialog();
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsDialog od = new OptionsDialog(Options.Clone() as Options);
                od.Owner = Application.Current.MainWindow;

                if (GeneralMethods.GetNullableBoolValue(od.ShowDialog()))
                {
                    Options = od.Options;
                    Options.Save();
                }
            }
            catch (System.Exception ex)
            {
                GeneralMethods.MessageBoxException(ex, "Could not set options:");
            }
        }

        private void btnSkipPrevious_Click(object sender, RoutedEventArgs e)
        {
            SkipPrevious();
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            TogglePlayPause();
        }

        private void btnSkipNext_Click(object sender, RoutedEventArgs e)
        {
            SkipNext();
        }

        private void element_FileTypeAdded(object sender, FileTypeEventArgs e)
        {
            switch (e.FileType.MediaItemType)
            {
                case MediaItemTypeEnum.Song:
                    List<FileType> songFileTypes = new List<FileType>(SongFileTypes);
                    songFileTypes.Add(e.FileType);
                    songFileTypes.Sort();

                    SongFileTypes = songFileTypes.ToArray();
                    break;

                case MediaItemTypeEnum.Video:
                    List<FileType> videoFileTypes = new List<FileType>(VideoFileTypes);
                    videoFileTypes.Add(e.FileType);
                    videoFileTypes.Sort();

                    VideoFileTypes = videoFileTypes.ToArray();
                    break;

                default:
                    throw new UnknownEnumValueException(e.FileType.MediaItemType);
            }
        }

        private void element_PlayMediaItems(object sender, PlayMediaItemsEventArgs e)
        {
            if (e.MediaItems.Length > 0)
                PlayMediaItems(e.MediaItems, e.SelectedIndex);
        }

        private void element_AddedToNowPlaying(object sender, MediaItemsEventArgs e)
        {
            if (e.MediaItems.Count > 0)
                AddToNowPlaying(e.MediaItems);
        }

        private void element_MediaItemSaved(object sender, MediaItemsEventArgs e)
        {
            SaveMediaItems(e.MediaItems);
        }

        private void element_MergingSelectedMediaItems(object sender, CancelMediaItemsOperationEventArgs e)
        {
            foreach (MediaItem mediaItem in e.MediaItems)
            {
                if (pgNowPlaying.MediaItems.Contains(mediaItem))
                {
                    if (GeneralMethods.MessageBox(mediaItem.Name + " is currently playing. Are you sure you want to merge it's parts with another " + mediaItem.Type.ToString().ToLower() + "? This will stop the media player.", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        pgNowPlaying.Stop();
                    else
                    {
                        e.Cancel = true;
                        e.ReasonForCancel = "Cannot merge the parts of " + mediaItem.Name + " because it is playing or in the \"Now Playing\" playlist";

                        return;
                    }
                }
            }
        }

        private void element_ExtractingPartFromMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            foreach (MediaItem mediaItem in e.MediaItems)
            {
                if (pgNowPlaying.MediaItems.Contains(mediaItem))
                {
                    if (GeneralMethods.MessageBox(mediaItem.Name + " is currently playing. Are you sure you want to extract a part from it? This will stop the media player.", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        pgNowPlaying.Stop();
                    else
                    {
                        e.Cancel = true;
                        e.ReasonForCancel = "Cannot extract a part from " + mediaItem.Name + " because it is playing or in the \"Now Playing\" playlist";

                        return;
                    }
                }
            }
        }

        private void element_MediaItemsDeleted(object sender, MediaItemsEventArgs e)
        {
            pgNowPlaying.RemoveMediaItems(e.MediaItems.Where(p => pgNowPlaying.MediaItems.Contains(p)));

            foreach (MediaItem mediaItem in e.MediaItems)
            {
                if (mediaItem.Type == MediaItemTypeEnum.Song)
                {
                    Song song = mediaItem as Song;

                    if (song.iTunesId != 0)
                        deletedSongITunesIDs.Add(song.iTunesId);
                }
            }
        }

        private void element_DeletingPart(object sender, CancelMediaItemsOperationEventArgs e)
        {
            foreach (MediaItem mediaItem in e.MediaItems)
            {
                if (pgNowPlaying.MediaItems.Contains(mediaItem))
                {
                    if (GeneralMethods.MessageBox(mediaItem.Name + " is currently playing. Are you sure you want to delete the part(s) from it? This will stop the media player.", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        pgNowPlaying.Stop();
                    else
                    {
                        e.Cancel = true;
                        e.ReasonForCancel = "Cannot delete part(s) from " + mediaItem.Name + " because it is playing or in the \"Now Playing\" playlist";

                        return;
                    }
                }
            }
        }

        private void pgNowPlaying_OpeningMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            if (IsOrganising)
            {
                if (pgOrganising.LibraryOrganiser.SelectedPart != null)
                {
                    foreach (MediaItem mediaItem in e.MediaItems)
                    {
                        if (mediaItem == pgOrganising.LibraryOrganiser.SelectedPart.MediaItem)
                        {
                            e.Cancel = true;
                            e.ReasonForCancel = "Cannot play " + mediaItem.Name + " because is currently being organised";

                            return;
                        }
                    }
                }
            }
        }

        private void pgOrganising_OrganisingMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            foreach (MediaItem mediaItem in e.MediaItems)
            {
                if (mediaItem == CurrentlyPlayingMediaItem)
                {
                    e.Cancel = true;
                    e.ReasonForCancel = mediaItem.Name + " is currently playing";

                    return;
                }
            }
        }

        private void pgOrganising_FinishedOrganising(object sender, System.EventArgs e)
        {
            if (tabPages.SelectedItem == tabOrganising)
                tabPages.SelectedItem = tabRootFolders;
        }

        private void pgRootFolders_PathChanged(object sender, RootFolderPathChangedEventArgs e)
        {
            IEnumerable<MediaItem> mediaItems;

            switch (e.RootFolder.MediaItemType)
            {
                case MediaItemTypeEnum.Video:
                    mediaItems = Videos;
                    break;

                case MediaItemTypeEnum.Song:
                    mediaItems = Songs;
                    break;

                default:
                    throw new UnknownEnumValueException(e.RootFolder.MediaItemType);
            }

            IntelligentString previousPath = e.PreviousPath;

            if (!previousPath.EndsWith("\\"))
                previousPath += "\\";

            IntelligentString rootFolderPath = e.RootFolder.Path;

            if (!rootFolderPath.EndsWith("\\"))
                rootFolderPath += "\\";

            Boolean libraryModified = false;

            foreach (MediaItem mediaItem in mediaItems)
            {
                Boolean modified = false;

                foreach (MediaItemPart part in mediaItem.Parts)
                {
                    IntelligentString organisedPath = mediaItem.Parts.GetPartOrganisedPath(mediaItem.OrganisedPath, part.Index);

                    if (part.Location == previousPath + organisedPath)
                    {
                        part.Location = rootFolderPath + organisedPath;
                        modified = true;
                        libraryModified = true;
                    }
                }

                if (modified)
                    mediaItem.Save();
            }

            if (libraryModified)
            {
                switch (e.RootFolder.MediaItemType)
                {
                    case MediaItemTypeEnum.Video:
                        Videos = new ObservableCollection<Video>(mediaItems.Cast<Video>());
                        break;

                    case MediaItemTypeEnum.Song:
                        Songs = new ObservableCollection<Song>(mediaItems.Cast<Song>());
                        break;

                    default:
                        throw new UnknownEnumValueException(e.RootFolder.MediaItemType);
                }
            }
        }

        private void missingEpisodes_Browse(object sender, CancelMediaItemsOperationEventArgs e)
        {
            fullParseFilename = false;

            try
            {
                foreach (MediaItem mediaItem in e.MediaItems)
                {
                    if (!AddMediaItem(mediaItem))
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            finally
            {
                fullParseFilename = true;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fires when the value of the OptionsProperty dependency property changes
        /// </summary>
        /// <param name="d">Dependency object containing the property that changed</param>
        /// <param name="e">Arguments passed to the event</param>
        private static void OnOptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaPlayerDialog mpd = d as MediaPlayerDialog;

            if (mpd.Options != null)
            {
                mpd.Options.PropertyChanged -= mpd.Options_PropertyChanged;
                mpd.Options.PropertyChanged += new PropertyChangedEventHandler(mpd.Options_PropertyChanged);
            }
        }

        #endregion

        #region Windows Members

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            //stops media played from crashing when moving to another monitor
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            HwndTarget hwndTarget = hwndSource.CompositionTarget;
            hwndTarget.RenderMode = RenderMode.SoftwareOnly;
            pgNowPlaying.mipPlayer.meNowPlaying.Play();
            base.OnSourceInitialized(e);
        }

        #endregion

        private void miSyncSongsWithITunes_Click(object sender, RoutedEventArgs e)
        {
            Boolean closeITunesOnCompletion = false;
            Process[] processes = Process.GetProcessesByName("iTunes");

            if (processes.Length != 1)
            {
                if (MessageBox.Show("iTunes needs to be open before you can sync your songs. Would you like to open iTunes now?", Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                else
                    closeITunesOnCompletion = true;
            }
            
            //load the iTunes library
            iTunesApp itunes = new iTunesApp();

            IEnumerable<IITFileOrCDTrack> current = GetTracksFromITunes(itunes);

            //filter to files with known extensions
            current = current.Where(track =>
                String.IsNullOrEmpty(track.Location) ||
                SongFileTypes.Any(f => f.Extensions.Any(ex => ex.ToLower() == Path.GetExtension(track.Location).ToLower())));

            //only sync with songs that pass the current filter
            IEnumerable<Song> songs = Songs.Where(s =>
                (!Filter.ShowHidden.HasValue || Filter.ShowHidden.Value == s.IsHidden) &&
                (!Filter.ShowPartsExist.HasValue || Filter.ShowPartsExist.Value == s.Parts.PartsExist));

            String message = "Tracks:" + Environment.NewLine;
            for (int i = 0; i < current.Count(); i++)
            {
                IITFileOrCDTrack track = current.ElementAt(i);
                message += track.Name + ": " + track.TrackDatabaseID + Environment.NewLine;
            }
            
            message += Environment.NewLine + "Songs:" + Environment.NewLine;
            for (int i = 0; i < songs.Count(); i++)
            {
                Song song = songs.ElementAt(i);
                message += song.Name + ": " + song.iTunesId + Environment.NewLine;
            }

            if (MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            //update the songs that are currently synced with each other
            IEnumerable<IITFileOrCDTrack> syncedTracks = current.Where(t => Songs.Any(s => s.iTunesId == t.TrackDatabaseID));
            IEnumerable<Song> syncedSongs = songs.Where(s => current.Any(t => t.TrackDatabaseID == s.iTunesId));

            UpdateSyncedSongs(
                syncedTracks,
                syncedSongs);

            //update the songs that are not currently synced
            IEnumerable<IITFileOrCDTrack> nonSyncedTracks = current.Where(t => !Songs.Any(s => s.iTunesId == t.TrackDatabaseID));
            IEnumerable<Song> nonSyncedSongs = songs.Where(s => !current.Any(t => t.TrackDatabaseID == s.iTunesId));

            UpdateNonSyncedSongs(
                nonSyncedTracks,
                nonSyncedSongs,
                ref itunes);

            if (closeITunesOnCompletion)
                itunes.Quit();

            Marshal.ReleaseComObject(itunes);
            itunes = null;
            GC.Collect();
        }

        private IEnumerable<IITFileOrCDTrack> GetTracksFromITunes(iTunesApp itunes)
        {
            IITLibraryPlaylist mainLibrary = itunes.LibraryPlaylist;

            //get the tracks in the main library
            IEnumerable<IITTrack> tracks = mainLibrary.Tracks.Cast<IITTrack>();

            //filter to file or CD tracks
            IEnumerable<IITFileOrCDTrack> current = tracks.Where(p => p.Kind == ITTrackKind.ITTrackKindFile).Cast<IITFileOrCDTrack>();

            return current;
        }

        private void UpdateSyncedSongs(IEnumerable<IITFileOrCDTrack> tracks, IEnumerable<Song> songs)
        {
            foreach (Song song in songs)
            {
                IITFileOrCDTrack track = tracks.Single(p => p.TrackDatabaseID == song.iTunesId);
                CopySongToITunesTrack(song, track);
            }
        }

        private void UpdateNonSyncedSongs(IEnumerable<IITFileOrCDTrack> tracks, IEnumerable<Song> songs, ref iTunesApp itunes)
        {
            List<IITFileOrCDTrack> current = new List<IITFileOrCDTrack>(tracks);
            List<Song> expected = new List<Song>(songs);
            List<Song> allSongs = new List<Song>(Songs);

            while (current.Count > 0)
            {
                IITFileOrCDTrack track = current[0];
                Int16 iTunesId = Convert.ToInt16(track.TrackDatabaseID);

                if (deletedSongITunesIDs.Contains(iTunesId))
                {
                    track.Delete();
                    deletedSongITunesIDs.Remove(iTunesId);
                }
                else
                {
                    Song song = new Song();
                    CopyITunesTrackToSong(track, song);

                    if (String.IsNullOrEmpty(track.Location))
                    {
                        //file does not exist
                    }
                    else
                    {
                        song.Save();
                        allSongs.Add(song);
                    }
                }

                current.RemoveAt(0);
            }

            Dictionary<String, Song> addedSongs = new Dictionary<String, Song>();

            while (expected.Count > 0)
            {
                Song song = expected[0];

                if (song.Parts.Count == 1)
                {
                    if (!song.IsHidden)
                    {
                        if (song.iTunesId == 0)
                        {
                            //song has not been added to iTunes
                            IITOperationStatus operationStatus = itunes.LibraryPlaylist.AddFile(song.Parts[0].Location.Value);                            
                            IEnumerable<IITTrack> newTracks = operationStatus.Tracks.Cast<IITTrack>();
                            IITFileOrCDTrack track = newTracks.ElementAt(0) as IITFileOrCDTrack;

                            CopySongToITunesTrack(song, track);

                            addedSongs.Add(track.Location, song);
                        }
                        else
                        {
                            //song was previously added to iTunes so must be deleted
                            song.Delete();
                            allSongs.Remove(song);
                        }
                    }
                }

                expected.RemoveAt(0);
            }

            if (addedSongs.Count > 0)
            {
                //close and reopen iTunes to comit database ID
                itunes.Quit();
                Marshal.ReleaseComObject(itunes);
                itunes = null;
                GC.Collect();

                itunes = new iTunesApp();
                IEnumerable<IITFileOrCDTrack> reloadedTracks = GetTracksFromITunes(itunes);

                foreach (KeyValuePair<String, Song> addedSong in addedSongs)
                {
                    IITFileOrCDTrack track = reloadedTracks.Single(p => p.Location == addedSong.Key);
                    addedSong.Value.iTunesId = Convert.ToInt16(track.TrackDatabaseID);
                    addedSong.Value.Save();
                }
            }

            allSongs.Sort();
            Songs = new ObservableCollection<Song>(allSongs);
        }

        private static void CopyITunesTrackToSong(IITFileOrCDTrack track, Song song)
        {
            song.Name = track.Name;
            song.Genre = track.Genre;
            song.DateCreated = track.DateAdded;
            song.Artist = track.Artist;
            song.Album = track.Album;
            song.DiskNumber = Convert.ToInt16(track.DiscNumber);
            song.NumberOfDisks = Convert.ToInt16(track.DiscCount);
            song.TrackNumber = Convert.ToInt16(track.TrackNumber);
            song.NumberOfTracks = Convert.ToInt16(track.TrackCount);
            song.Year = Convert.ToInt16(track.Year);
            song.iTunesId = Convert.ToInt16(track.TrackDatabaseID);
            
            if (!String.IsNullOrEmpty(track.Location))
                song.Parts.Add(track.Location, track.Size, track.Duration * 1000);

            SyncPlayCounts(song, track);
        }

        private static void CopySongToITunesTrack(Song song, IITFileOrCDTrack track)
        {
            //location must be set first because a track without a location cannot have any other property set
            track.Location = song.Parts[0].Location.Value;

            track.Name = song.Name.Value;
            track.Genre = song.Genre.Value;
            track.Artist = song.Artist.Value;
            track.Album = song.Album.Value;
            track.DiscNumber = song.DiskNumber;
            track.DiscCount = song.NumberOfDisks;
            track.TrackNumber = song.TrackNumber;
            track.TrackCount = song.NumberOfTracks;
            track.Year = song.Year;
            track.AlbumArtist = String.Empty;
            track.Category = String.Empty;
            track.Compilation = false;
            track.Composer = String.Empty;
            track.Description = String.Empty;
            track.Grouping = String.Empty;
            track.LongDescription = String.Empty;
            track.SortAlbum = String.Empty;
            track.SortAlbumArtist = String.Empty;
            track.SortArtist = String.Empty;
            track.SortComposer = String.Empty;
            track.SortName = String.Empty;

            SyncPlayCounts(song, track);
        }

        private static void SyncPlayCounts(Song song, IITFileOrCDTrack track)
        {
            if (song.DateLastPlayed > track.PlayedDate)
            {
                track.PlayedDate = song.DateLastPlayed;
                track.PlayedCount = song.PlayCount;
            }
            else
            {
                Int32 offset = 0;

                while (song.PlayCount < track.PlayedCount)
                {
                    song.Played(track.PlayedDate.Subtract(TimeSpan.FromMinutes(offset)));
                    offset++;
                }
            }
        }
    }
}

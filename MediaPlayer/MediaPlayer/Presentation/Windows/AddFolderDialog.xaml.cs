using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MediaPlayer.Library.Business;
using System.Collections.ObjectModel;
using MediaPlayer.Business;
using MediaPlayer.EventArgs;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for AddFolderDialog.xaml
    /// </summary>
    public partial class AddFolderDialog : Window
    {
        #region Events

        /// <summary>
        /// Fires when a new file type is added to the system
        /// </summary>
        public event FileTypeEventHandler FileTypeAdded;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty FolderBrowserProperty = DependencyProperty.Register("FolderBrowser", typeof(FolderBrowser), typeof(AddFolderDialog));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<MediaItem>), typeof(AddFolderDialog));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(MediaItemFilter), typeof(AddFolderDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the media items that have been added to the system using the window
        /// </summary>
        private ObservableCollection<MediaItem> ItemsSource
        {
            get { return GetValue(AddFolderDialog.ItemsSourceProperty) as ObservableCollection<MediaItem>; }
            set { SetValue(AddFolderDialog.ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets the media items that have been added to the system using the window
        /// </summary>
        public MediaItem[] AddedMediaItems
        {
            get { return ItemsSource.ToArray(); }
        }

        /// <summary>
        /// Gets the browser used to search for media files
        /// </summary>
        public FolderBrowser FolderBrowser
        {
            get { return GetValue(AddFolderDialog.FolderBrowserProperty) as FolderBrowser; }
        }

        /// <summary>
        /// Gets or sets the filter used to filter media items
        /// </summary>
        public MediaItemFilter Filter
        {
            get { return GetValue(FilterProperty) as MediaItemFilter; }
            set { SetValue(FilterProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the AddFolderDialog class
        /// </summary>
        /// <param name="fileTypes">File types used to determine which files are media files</param>
        public AddFolderDialog(FileType[] fileTypes)
        {
            InitializeComponent();

            ItemsSource = new ObservableCollection<MediaItem>();

            SetValue(FolderBrowserProperty, new FolderBrowser(fileTypes, Dispatcher));
            FolderBrowser.FoundMediaItem += new MediaItemEventHandler(FolderBrowser_FoundMediaItem);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the FileTypeAdded event
        /// </summary>
        /// <param name="fileType">The file type that was added</param>
        private void OnFileTypeAdded(FileType fileType)
        {
            if (FileTypeAdded != null)
                FileTypeAdded(this, new FileTypeEventArgs(fileType));
        }

        /// <summary>
        /// Saves the media item to the database
        /// </summary>
        /// <param name="mediaItem">Media item that was saved</param>
        private void MediaItemSaved(MediaItem mediaItem)
        {
            //mediaItem.Save();

            List<MediaItem> mediaItems = new List<MediaItem>(ItemsSource);

            if (!mediaItems.Contains(mediaItem))
                mediaItems.Add(mediaItem);

            ItemsSource = new ObservableCollection<MediaItem>(mediaItems);
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(FolderBrowser.FolderPath))
                fbtPath.Browse();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (FolderBrowser.IsSearching)
                FolderBrowser.StopBrowsing();
            else
                FolderBrowser.BrowseAsync();
        }

        private void FolderBrowser_FoundMediaItem(object sender, MediaItemEventArgs e)
        {
            if (!ItemsSource.Any(m => m.Parts.Any(p => p.Location == e.MediaItem.Parts[0].Location)))
            {
                List<MediaItem> items = new List<MediaItem>(ItemsSource);
                items.Add(e.MediaItem);

                ItemsSource = new ObservableCollection<MediaItem>(items);
            }
        }

        private void miItems_MediaItemSaved(object sender, MediaItemEventArgs e)
        {
            MediaItemSaved(e.MediaItem);
        }

        private void miItems_MediaItemsSaved(object sender, MediaItemsEventArgs e)
        {
            foreach (MediaItem mediaItem in e.MediaItems)
                MediaItemSaved(mediaItem);
        }

        private void miItems_MediaItemsDeleted(object sender, MediaItemsEventArgs e)
        {
            List<MediaItem> mediaItems = new List<MediaItem>(ItemsSource);

            foreach (MediaItem mediaItem in e.MediaItems)
                mediaItems.Remove(mediaItem);

            ItemsSource = new ObservableCollection<MediaItem>(mediaItems);
        }

        private void miItems_FileTypeAdded(object sender, FileTypeEventArgs e)
        {
            OnFileTypeAdded(e.FileType);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion
    }
}

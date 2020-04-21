using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business;
using MediaPlayer.EventArgs;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for MissingMediaItemsDialog.xaml
    /// </summary>
    public partial class MissingMediaItemsDialog : Window
    {   
        #region Events

        /// <summary>
        /// Fires when the user chooses to browse for a missing media item
        /// </summary>
        public event CancelMediaItemsOperationEventHandler Browse;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(IEnumerable<MediaItem>), typeof(MissingMediaItemsDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Options), typeof(MissingMediaItemsDialog));
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the media items displayed in the window
        /// </summary>
        public IEnumerable<MediaItem> MediaItems
        {
            get { return GetValue(MediaItemsProperty) as IEnumerable<MediaItem>; }
            set { SetValue(MediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the options currently saved in the system
        /// </summary>
        public Options Options
        {
            get { return GetValue(OptionsProperty) as Options; }
            set { SetValue(OptionsProperty, value); }
        }

        /// <summary>
        /// Gets the columns of the grid
        /// </summary>
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return dgMissingMediaItems.Columns; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MissingMediaItemsDialog class
        /// </summary>
        /// <param name="mediaItems">Media items displayed in the window</param>
        /// <param name="options">Options currently saved in the system</param>
        public MissingMediaItemsDialog(IEnumerable<MediaItem> mediaItems, Options options)
        {
            InitializeComponent();

            MediaItems = mediaItems;
            Options = options;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the Browse event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnBrowse(CancelMediaItemsOperationEventArgs e)
        {
            if (Browse != null)
                Browse(this, e);
        }

        /// <summary>
        /// Searchs for a torrent for the specified media item
        /// </summary>
        /// <param name="mediaItem">Media item to search for a torrent for</param>
        private void TorrentSearch(MediaItem mediaItem)
        {
            if (String.IsNullOrEmpty(Options.TorrentSearchUrl.Trim()))
                MessageBox.Show("Please set the torrent search options to search for torrents.", Title, MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    IntelligentString searchString = IntelligentString.Empty;

                    if (!String.IsNullOrEmpty(Options.TorrentSearchPrefix.Trim()))
                        searchString += Options.TorrentSearchPrefix.Trim() + " ";

                    searchString += mediaItem.GetSearchString();

                    if (!String.IsNullOrEmpty(Options.TorrentSearchSuffix.Trim()))
                        searchString += " " + Options.TorrentSearchSuffix.Trim();

                    if (!String.IsNullOrEmpty(Options.TorrentSearchSpaceCharacter.Trim()))
                        searchString = searchString.Replace(" ", Options.TorrentSearchSpaceCharacter.Trim());

                    searchString = Options.TorrentSearchUrl.Trim() + searchString;

                    if (!searchString.StartsWith("http"))
                        searchString = "http://" + searchString;

                    Process.Start(searchString.Value);
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Cannot search for torrents for " + mediaItem.Type.ToString() + " files.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        #endregion

        #region Event Handlers

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Button btnBrowse = sender as Button;
            MediaItem missingMediaItem = btnBrowse.DataContext as MediaItem;

            CancelMediaItemsOperationEventArgs cmioea = new CancelMediaItemsOperationEventArgs(new MediaItem[1] { missingMediaItem });
            OnBrowse(cmioea);

            if (!cmioea.Cancel)
                MediaItems = MediaItems.Where(p => !p.IsInDatabase);
        }

        private void btnTorrentSearch_Click(object sender, RoutedEventArgs e)
        {
            Button btnTorrentSearch = sender as Button;
            MediaItem missingMediaItem = btnTorrentSearch.DataContext as MediaItem;

            TorrentSearch(missingMediaItem);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}

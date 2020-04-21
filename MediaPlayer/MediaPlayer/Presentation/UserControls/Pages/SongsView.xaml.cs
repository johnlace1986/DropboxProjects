using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MediaPlayer.Business;
using MediaPlayer.EventArgs;
using MediaPlayer.Library.Business;
using MediaPlayer.Presentation.UserControls.MediaItemViews;
using Utilities.Business;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for SongsView.xaml
    /// </summary>
    public partial class SongsView : UserControl, IMediaItemsView
    {
        #region Events

        /// <summary>
        /// Fires when a new file type is added to the system
        /// </summary>
        public event FileTypeEventHandler FileTypeAdded;

        /// <summary>
        /// Fires when the user selects to play one or more media items
        /// </summary>
        public event PlayMediaItemsEventHandler PlayMediaItems;

        /// <summary>
        /// Fires when one or more songs are added to the "Now Playing..." playlist
        /// </summary>
        public event MediaItemsEventHandler AddedToNowPlaying;

        /// <summary>
        /// Fires when multiple media items are saved
        /// </summary>
        public event MediaItemsEventHandler MediaItemsSaved;

        /// <summary>
        /// Fires prior to one or more media items being deleted
        /// </summary>
        public event CancelMediaItemsOperationEventHandler MediaItemsDeleting;

        /// <summary>
        /// Fires when one or more media item is deleted
        /// </summary>
        public event MediaItemsEventHandler MediaItemsDeleted;

        /// <summary>
        /// Fires prior to two or more media items being merged
        /// </summary>
        public event CancelMediaItemsOperationEventHandler MergingSelectedMediaItems;

        /// <summary>
        /// Fires prior to a part of a media item being extracted to another media item
        /// </summary>
        public event CancelMediaItemsOperationEventHandler ExtractingPartFromMediaItem;

        /// <summary>
        /// Fires prior to a part being deleted from a media item
        /// </summary>
        public event CancelMediaItemsOperationEventHandler DeletingPart;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(SongsView));

        public static readonly DependencyProperty SongsProperty = DependencyProperty.Register("Songs", typeof(ObservableCollection<Song>), typeof(SongsView), new PropertyMetadata(new ObservableCollection<Song>()));

        public static readonly DependencyProperty SelectedGenresProperty = DependencyProperty.Register("SelectedGenres", typeof(ObservableCollection<IntelligentString>), typeof(SongsView), new PropertyMetadata(new ObservableCollection<IntelligentString>()));

        public static readonly DependencyProperty SelectedArtistsProperty = DependencyProperty.Register("SelectedArtists", typeof(ObservableCollection<IntelligentString>), typeof(SongsView), new PropertyMetadata(new ObservableCollection<IntelligentString>()));

        public static readonly DependencyProperty SelectedAlbumsProperty = DependencyProperty.Register("SelectedAlbums", typeof(ObservableCollection<IntelligentString>), typeof(SongsView), new PropertyMetadata(new ObservableCollection<IntelligentString>()));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(MediaItemFilter), typeof(SongsView), new PropertyMetadata(new MediaItemFilter(), new PropertyChangedCallback(OnFilterPropertyChanged)));

        public static readonly DependencyProperty GenresProperty = DependencyProperty.Register("Genres", typeof(ObservableCollection<IntelligentString>), typeof(SongsView));

        public static readonly DependencyProperty ArtistsProperty = DependencyProperty.Register("Artists", typeof(ObservableCollection<IntelligentString>), typeof(SongsView));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(SongsView));

        public static readonly DependencyProperty PlayStateProperty = DependencyProperty.Register("PlayState", typeof(PlayStateEnum), typeof(SongsView));

        public static readonly DependencyProperty CurrentlyPlayingMediaItemProperty = DependencyProperty.Register("CurrentlyPlayingMediaItem", typeof(MediaItem), typeof(SongsView), new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the options currently saved in the database
        /// </summary>
        public Business.Options Options
        {
            get { return GetValue(SongsView.OptionsProperty) as Business.Options; }
            set { SetValue(SongsView.OptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the songs being displayed in the view
        /// </summary>
        public ObservableCollection<Song> Songs
        {
            get { return GetValue(SongsView.SongsProperty) as ObservableCollection<Song>; }
            set { SetValue(SongsView.SongsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the genres that have currently been selected
        /// </summary>
        public ObservableCollection<IntelligentString> SelectedGenres
        {
            get { return GetValue(SongsView.SelectedGenresProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(SongsView.SelectedGenresProperty, value); }
        }

        /// <summary>
        /// Gets or sets the artists that have currently been selected
        /// </summary>
        public ObservableCollection<IntelligentString> SelectedArtists
        {
            get { return GetValue(SongsView.SelectedArtistsProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(SongsView.SelectedArtistsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the albums that have currently been selected
        /// </summary>
        public ObservableCollection<IntelligentString> SelectedAlbums
        {
            get { return GetValue(SongsView.SelectedAlbumsProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(SongsView.SelectedAlbumsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the filter used to filter all songs in the system
        /// </summary>
        public MediaItemFilter Filter
        {
            get { return GetValue(FilterProperty) as MediaItemFilter; }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the genres of all the unfiltered songs in the view
        /// </summary>
        public ObservableCollection<IntelligentString> Genres
        {
            get { return GetValue(SongsView.GenresProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(SongsView.GenresProperty, value); }
        }

        /// <summary>
        /// Gets or sets the artists of all the unfiltered songs in the view
        /// </summary>
        public ObservableCollection<IntelligentString> Artists
        {
            get { return GetValue(SongsView.ArtistsProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(SongsView.ArtistsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the library is currently being organised
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(SongsView.IsOrganisingProperty); }
            set { SetValue(SongsView.IsOrganisingProperty, value); }
        }

        /// <summary>
        /// Gets the sort descriptions in the control
        /// </summary>
        public SortDescriptionCollection SortDescriptions
        {
            get { return mivSongs.SortDescriptions; }
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SongsView class
        /// </summary>
        public SongsView()
        {
            DataContext = this;

            InitializeComponent();
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
        /// Fires the PlayMediaItems event
        /// </summary>
        /// <param name="mediaItems">Media items that are to be played</param>
        /// <param name="selectedIndex">Index in the collection of the first media item to be played</param>
        public void OnPlayMediaItems(MediaItem[] mediaItems, Int32 selectedIndex)
        {
            if (PlayMediaItems != null)
                PlayMediaItems(this, new PlayMediaItemsEventArgs(mediaItems, selectedIndex));
        }

        /// <summary>
        /// Fires the AddedToNowPlaying event
        /// </summary>
        /// <param name="mediaItems">Media items that were added the the "Now Playing..." playlist</param>
        private void OnAddedToNowPlaying(MediaItem[] mediaItems)
        {
            if (AddedToNowPlaying != null)
                AddedToNowPlaying(this, new MediaItemsEventArgs(mediaItems));
        }

        /// <summary>
        /// Fires the MediaItemsSaved event
        /// </summary>
        /// <param name="mediaItems">Media items being saved</param>
        private void OnMediaItemsSaved(IEnumerable<MediaItem> mediaItems)
        {
            if (MediaItemsSaved != null)
                MediaItemsSaved(this, new MediaItemsEventArgs(mediaItems));
        }

        /// <summary>
        /// Fires the MediaItemsDeleting event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnMediaItemsDeleting(CancelMediaItemsOperationEventArgs e)
        {
            if (MediaItemsDeleting != null)
                MediaItemsDeleting(this, e);
        }

        /// <summary>
        /// Fires the MediaItemsDeleted event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnMediaItemsDeleted(MediaItemsEventArgs e)
        {
            if (MediaItemsDeleted != null)
                MediaItemsDeleted(this, e);
        }

        /// <summary>
        /// Fires the MergingSelectedMediaItems event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnMergingSelectedMediaItems(CancelMediaItemsOperationEventArgs e)
        {
            if (MergingSelectedMediaItems != null)
                MergingSelectedMediaItems(this, e);
        }

        /// <summary>
        /// Fires the ExtractingPartFromMediaItem event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnExtractingPartFromMediaItem(CancelMediaItemsOperationEventArgs e)
        {
            if (ExtractingPartFromMediaItem != null)
                ExtractingPartFromMediaItem(this, e);
        }

        /// <summary>
        /// Fires the DeletingPart event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnDeletingPart(CancelMediaItemsOperationEventArgs e)
        {
            if (DeletingPart != null)
                DeletingPart(this, e);
        }

        /// <summary>
        /// Saves the song to the database
        /// </summary>
        /// <param name="song">Song being saved</param>
        private void SaveSong(Song song)
        {
            SaveSongs(new Song[1] { song });
        }

        /// <summary>
        /// Saves the songs to the database
        /// </summary>
        /// <param name="songs">Songs being saved</param>
        private void SaveSongs(IEnumerable<Song> songs)
        {
            List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

            OnMediaItemsSaved(songs);

            SortDescriptions.Clear();

            foreach (SortDescription sortDescription in sortDescriptions)
                SortDescriptions.Add(sortDescription);

            mivSongs.UpdateLayout();
        }

        #endregion

        #region Event Handlers

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MultiBindingExpression genres = BindingOperations.GetMultiBindingExpression(lstGenres, ListBox.ItemsSourceProperty);
            genres.UpdateTarget();

            MultiBindingExpression artists = BindingOperations.GetMultiBindingExpression(lstArtists, ListBox.ItemsSourceProperty);
            artists.UpdateTarget();

            MultiBindingExpression albums = BindingOperations.GetMultiBindingExpression(lstAlbums, ListBox.ItemsSourceProperty);
            albums.UpdateTarget();

            MultiBindingExpression songs = BindingOperations.GetMultiBindingExpression(mivSongs, MediaItemsView.MediaItemsProperty);
            songs.UpdateTarget();

            BindingExpression summary = lblSummary.GetBindingExpression(Label.ContentProperty);
            summary.UpdateTarget();
        }

        private void lstGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstGenres.SelectedItem == null)
            {
                lstGenres.SelectionChanged -= lstGenres_SelectionChanged;
                lstGenres.SelectedItem = Options.ViewAllText;
                SelectedGenres = new ObservableCollection<IntelligentString>() { Options.ViewAllText };
                lstGenres.SelectionChanged += lstGenres_SelectionChanged;
            }
            else
            {
                List<IntelligentString> selectedGenres = new List<IntelligentString>(SelectedGenres);

                List<IntelligentString> current = new List<IntelligentString>(lstGenres.SelectedItems.Cast<IntelligentString>());
                List<IntelligentString> expected = new List<IntelligentString>(selectedGenres);

                foreach (IntelligentString selectedGenre in lstGenres.SelectedItems)
                {
                    if (!expected.Contains(selectedGenre))
                        selectedGenres.Add(selectedGenre);

                    expected.Remove(selectedGenre);
                    current.Remove(selectedGenre);
                }

                foreach (IntelligentString unselectedGenre in expected)
                    selectedGenres.Remove(unselectedGenre);

                SelectedGenres = new ObservableCollection<IntelligentString>(selectedGenres);
            }
        }

        private void lstArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstArtists.SelectedItem == null)
            {
                lstArtists.SelectionChanged -= lstArtists_SelectionChanged;
                lstArtists.SelectedItem = Options.ViewAllText;
                SelectedArtists = new ObservableCollection<IntelligentString>() { Options.ViewAllText };
                lstArtists.SelectionChanged += lstArtists_SelectionChanged;
            }
            else
            {
                List<IntelligentString> selectedArtists = new List<IntelligentString>(SelectedArtists);

                List<IntelligentString> current = new List<IntelligentString>(lstArtists.SelectedItems.Cast<IntelligentString>());
                List<IntelligentString> expected = new List<IntelligentString>(selectedArtists);

                foreach (IntelligentString selectedArtist in lstArtists.SelectedItems)
                {
                    if (!expected.Contains(selectedArtist))
                        selectedArtists.Add(selectedArtist);

                    expected.Remove(selectedArtist);
                    current.Remove(selectedArtist);
                }

                foreach (IntelligentString unselectedArtist in expected)
                    selectedArtists.Remove(unselectedArtist);

                SelectedArtists = new ObservableCollection<IntelligentString>(selectedArtists);
            }
        }

        private void lstAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstAlbums.SelectedItem == null)
            {
                lstAlbums.SelectionChanged -= lstAlbums_SelectionChanged;
                lstAlbums.SelectedItem = Options.ViewAllText;
                SelectedAlbums = new ObservableCollection<IntelligentString>() { Options.ViewAllText };
                lstAlbums.SelectionChanged += lstAlbums_SelectionChanged;
            }
            else
            {
                List<IntelligentString> selectedAlbum = new List<IntelligentString>(SelectedAlbums);

                List<IntelligentString> current = new List<IntelligentString>(lstAlbums.SelectedItems.Cast<IntelligentString>());
                List<IntelligentString> expected = new List<IntelligentString>(selectedAlbum);

                foreach (IntelligentString albums in lstAlbums.SelectedItems)
                {
                    if (!expected.Contains(albums))
                        selectedAlbum.Add(albums);

                    expected.Remove(albums);
                    current.Remove(albums);
                }

                foreach (IntelligentString unselectedAlbum in expected)
                    selectedAlbum.Remove(unselectedAlbum);

                SelectedAlbums = new ObservableCollection<IntelligentString>(selectedAlbum);
            }
        }

        private void mivSongs_MediaItemSaved(object sender, MediaItemEventArgs e)
        {
            SaveSong(e.MediaItem as Song);
        }

        private void mivSongs_MediaItemsSaved(object sender, MediaItemsEventArgs e)
        {
            SaveSongs(e.MediaItems.Cast<Song>());
        }

        private void mivSongs_MediaItemsDeleting(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnMediaItemsDeleting(e);
        }

        private void mivSongs_MediaItemsDeleted(object sender, EventArgs.MediaItemsEventArgs e)
        {
            List<Song> songs = new List<Song>(Songs);

            foreach (Song song in e.MediaItems)
                songs.Remove(song);

            Songs = new ObservableCollection<Song>(songs);

            OnMediaItemsDeleted(e);
        }

        private void mivSongs_PlayMediaItems(object sender, PlayMediaItemsEventArgs e)
        {
            OnPlayMediaItems(e.MediaItems, e.SelectedIndex);
        }

        private void mivSongs_AddedToNowPlaying(object sender, MediaItemsEventArgs e)
        {
            OnAddedToNowPlaying(SelectedMediaItems.ToArray());
        }

        private void mivSongs_MergingSelectedMediaItems(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnMergingSelectedMediaItems(e);
        }

        private void mivSongs_ExtractingPartFromMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnExtractingPartFromMediaItem(e);
        }

        private void mivSongs_DeletingPart(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnDeletingPart(e);
        }

        private void element_FileTypeAdded(object sender, FileTypeEventArgs e)
        {
            OnFileTypeAdded(e.FileType);
        }

        private void btnPartsDoNotExist_Click(object sender, RoutedEventArgs e)
        {
            Button btnRemovedMediaItem = sender as Button;
            MediaItem mediaItem = btnRemovedMediaItem.DataContext as MediaItem;

            IntelligentString message = "The following parts belonging to " + mediaItem.Name + " do not exist:" + Environment.NewLine + Environment.NewLine;

            foreach (MediaItemPart part in mediaItem.Parts)
                if (!File.Exists(part.Location.Value))
                    message += part.IndexString + ": " + part.Location + Environment.NewLine;

            message += Environment.NewLine + "Delete " + mediaItem.Name + "?";

            if (GeneralMethods.MessageBox(message.Value, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                mivSongs.RemoveMediaItems(mediaItem);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fires when the value of the FilterProperty changes
        /// </summary>
        /// <param name="d">Dependency object containing the dependency property that changed</param>
        /// <param name="e">Events passed to the argument</param>
        private static void OnFilterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SongsView songsView = d as SongsView;

            if (songsView.Filter != null)
            {
                songsView.Filter.PropertyChanged -= songsView.Filter_PropertyChanged;
                songsView.Filter.PropertyChanged += songsView.Filter_PropertyChanged;
            }
        }

        #endregion

        #region IMediaItemsView Members

        /// <summary>
        /// Gets the media items displayed in the view
        /// </summary>
        public MediaItem[] MediaItems
        {
            get { return mivSongs.MediaItems.ToArray(); }
        }

        /// <summary>
        /// Gets the media items that are currently selected in the view
        /// </summary>
        public MediaItem[] SelectedMediaItems
        {
            get { return mivSongs.SelectedMediaItems; }
        }

        /// <summary>
        /// Gets the index of the currently selected media item
        /// </summary>
        public int SelectedIndex
        {
            get { return mivSongs.SelectedIndex; }
        }

        #endregion
    }
}

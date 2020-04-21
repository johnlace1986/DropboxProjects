using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using System.Collections.ObjectModel;
using MediaPlayer.Business;
using Utilities.Business;
using MediaPlayer.EventArgs;
using System.ComponentModel;
using System.IO;
using MediaPlayer.Presentation.UserControls.MediaItemViews;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for VideosView.xaml
    /// </summary>
    public partial class VideosView : UserControl, IMediaItemsView
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
        /// Fires when one or more videos are added to the "Now Playing..." playlist
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

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(VideosView));

        public static readonly DependencyProperty VideosProperty = DependencyProperty.Register("Videos", typeof(ObservableCollection<Video>), typeof(VideosView), new PropertyMetadata(new ObservableCollection<Video>()));

        public static readonly DependencyProperty SelectedGenresProperty = DependencyProperty.Register("SelectedGenres", typeof(ObservableCollection<IntelligentString>), typeof(VideosView), new PropertyMetadata(new ObservableCollection<IntelligentString>()));

        public static readonly DependencyProperty SelectedProgramsProperty = DependencyProperty.Register("SelectedPrograms", typeof(ObservableCollection<IntelligentString>), typeof(VideosView), new PropertyMetadata(new ObservableCollection<IntelligentString>()));

        public static readonly DependencyProperty SelectedSeriesProperty = DependencyProperty.Register("SelectedSeries", typeof(ObservableCollection<IntelligentString>), typeof(VideosView), new PropertyMetadata(new ObservableCollection<IntelligentString>()));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(MediaItemFilter), typeof(VideosView), new PropertyMetadata(new MediaItemFilter(), new PropertyChangedCallback(OnFilterPropertyChanged)));

        public static readonly DependencyProperty GenresProperty = DependencyProperty.Register("Genres", typeof(ObservableCollection<IntelligentString>), typeof(VideosView));

        public static readonly DependencyProperty ProgramsProperty = DependencyProperty.Register("Programs", typeof(ObservableCollection<IntelligentString>), typeof(VideosView));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(VideosView));

        public static readonly DependencyProperty PlayStateProperty = DependencyProperty.Register("PlayState", typeof(PlayStateEnum), typeof(VideosView));

        public static readonly DependencyProperty CurrentlyPlayingMediaItemProperty = DependencyProperty.Register("CurrentlyPlayingMediaItem", typeof(MediaItem), typeof(VideosView), new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the options currently saved in the database
        /// </summary>
        public Business.Options Options
        {
            get { return GetValue(VideosView.OptionsProperty) as Business.Options; }
            set { SetValue(VideosView.OptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the videos being displayed in the view
        /// </summary>
        public ObservableCollection<Video> Videos
        {
            get { return GetValue(VideosView.VideosProperty) as ObservableCollection<Video>; }
            set { SetValue(VideosView.VideosProperty, value); }
        }

        /// <summary>
        /// Gets or sets the genres that have currently been selected
        /// </summary>
        public ObservableCollection<IntelligentString> SelectedGenres
        {
            get { return GetValue(VideosView.SelectedGenresProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(VideosView.SelectedGenresProperty, value); }
        }

        /// <summary>
        /// Gets or sets the programs that have currently been selected
        /// </summary>
        public ObservableCollection<IntelligentString> SelectedPrograms
        {
            get { return GetValue(VideosView.SelectedProgramsProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(VideosView.SelectedProgramsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the series that have currently been selected
        /// </summary>
        public ObservableCollection<IntelligentString> SelectedSeries
        {
            get { return GetValue(VideosView.SelectedSeriesProperty) as ObservableCollection<IntelligentString>; }
            set { SetValue(VideosView.SelectedSeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the filter used to filter all videos in the system
        /// </summary>
        public MediaItemFilter Filter
        {
            get { return GetValue(FilterProperty) as MediaItemFilter; }
            set { SetValue(FilterProperty, value); }
        }

        ///// <summary>
        ///// Gets or sets the genres of all the unfiltered videos in the view
        ///// </summary>
        //public ObservableCollection<IntelligentString> Genres
        //{
        //    get { return GetValue(VideosView.GenresProperty) as ObservableCollection<IntelligentString>; }
        //    set { SetValue(VideosView.GenresProperty, value); }
        //}

        ///// <summary>
        ///// Gets or sets the programs of all the unfiltered videos in the view
        ///// </summary>
        //public ObservableCollection<IntelligentString> Programs
        //{
        //    get { return GetValue(VideosView.ProgramsProperty) as ObservableCollection<IntelligentString>; }
        //    set { SetValue(VideosView.ProgramsProperty, value); }
        //}

        /// <summary>
        /// Gets or sets a value determining whether or not the library is currently being organised
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(VideosView.IsOrganisingProperty); }
            set { SetValue(VideosView.IsOrganisingProperty, value); }
        }

        /// <summary>
        /// Gets the sort descriptions in the control
        /// </summary>
        public SortDescriptionCollection SortDescriptions
        {
            get { return mivVideos.SortDescriptions; }
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
        /// Initialises a new instance of the VideosView class
        /// </summary>
        public VideosView()
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
        /// Saves the video to the database
        /// </summary>
        /// <param name="video">Video being saved</param>
        private void SaveVideo(Video video)
        {
            SaveVideos(new Video[1] { video });
        }

        /// <summary>
        /// Saves the videos to the database
        /// </summary>
        /// <param name="videos">Videos being saved</param>
        private void SaveVideos(IEnumerable<Video> videos)
        {
            List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

            OnMediaItemsSaved(videos);

            SortDescriptions.Clear();

            foreach (SortDescription sortDescription in sortDescriptions)
                SortDescriptions.Add(sortDescription);

            mivVideos.UpdateLayout();
        }

        #endregion

        #region Event Handlers

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MultiBindingExpression genres = BindingOperations.GetMultiBindingExpression(lstGenres, ListBox.ItemsSourceProperty);
            genres.UpdateTarget();

            MultiBindingExpression programs = BindingOperations.GetMultiBindingExpression(lstPrograms, ListBox.ItemsSourceProperty);
            programs.UpdateTarget();

            MultiBindingExpression series = BindingOperations.GetMultiBindingExpression(lstSeries, ListBox.ItemsSourceProperty);
            series.UpdateTarget();

            MultiBindingExpression videos = BindingOperations.GetMultiBindingExpression(mivVideos, MediaItemsView.MediaItemsProperty);
            videos.UpdateTarget();

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

        private void lstPrograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPrograms.SelectedItem == null)
            {
                lstPrograms.SelectionChanged -= lstPrograms_SelectionChanged;
                lstPrograms.SelectedItem = Options.ViewAllText;
                SelectedPrograms = new ObservableCollection<IntelligentString>() { Options.ViewAllText };
                lstPrograms.SelectionChanged += lstPrograms_SelectionChanged;
            }
            else
            {
                List<IntelligentString> selectedPrograms = new List<IntelligentString>(SelectedPrograms);

                List<IntelligentString> current = new List<IntelligentString>(lstPrograms.SelectedItems.Cast<IntelligentString>());
                List<IntelligentString> expected = new List<IntelligentString>(selectedPrograms);

                foreach (IntelligentString selectedProgram in lstPrograms.SelectedItems)
                {
                    if (!expected.Contains(selectedProgram))
                        selectedPrograms.Add(selectedProgram);

                    expected.Remove(selectedProgram);
                    current.Remove(selectedProgram);
                }

                foreach (IntelligentString unselectedProgram in expected)
                    selectedPrograms.Remove(unselectedProgram);

                SelectedPrograms = new ObservableCollection<IntelligentString>(selectedPrograms);
            }
        }

        private void lstSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSeries.SelectedItem == null)
            {
                lstSeries.SelectionChanged -= lstSeries_SelectionChanged;
                lstSeries.SelectedItem = Options.ViewAllText;
                SelectedSeries = new ObservableCollection<IntelligentString>() { Options.ViewAllText };
                lstSeries.SelectionChanged += lstSeries_SelectionChanged;
            }
            else
            {
                List<IntelligentString> selectedSeries = new List<IntelligentString>(SelectedSeries);

                List<IntelligentString> current = new List<IntelligentString>(lstSeries.SelectedItems.Cast<IntelligentString>());
                List<IntelligentString> expected = new List<IntelligentString>(selectedSeries);

                foreach (IntelligentString series in lstSeries.SelectedItems)
                {
                    if (!expected.Contains(series))
                        selectedSeries.Add(series);

                    expected.Remove(series);
                    current.Remove(series);
                }

                foreach (IntelligentString unselectedSeries in expected)
                    selectedSeries.Remove(unselectedSeries);

                SelectedSeries = new ObservableCollection<IntelligentString>(selectedSeries);
            }
        }

        private void mivVideos_MediaItemSaved(object sender, MediaItemEventArgs e)
        {
            SaveVideo(e.MediaItem as Video);
        }

        private void mivVideos_MediaItemsSaved(object sender, MediaItemsEventArgs e)
        {
            SaveVideos(e.MediaItems.Cast<Video>());
        }

        private void mivVideos_MediaItemsDeleting(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnMediaItemsDeleting(e);
        }

        private void mivVideos_MediaItemsDeleted(object sender, EventArgs.MediaItemsEventArgs e)
        {
            List<Video> videos = new List<Video>(Videos);

            foreach (Video video in e.MediaItems)
                videos.Remove(video);

            Videos = new ObservableCollection<Video>(videos);

            OnMediaItemsDeleted(e);
        }

        private void mivVideos_PlayMediaItems(object sender, PlayMediaItemsEventArgs e)
        {
            OnPlayMediaItems(e.MediaItems, e.SelectedIndex);
        }

        private void mivVideos_AddedToNowPlaying(object sender, MediaItemsEventArgs e)
        {
            OnAddedToNowPlaying(SelectedMediaItems.ToArray());
        }

        private void mivVideos_MergingSelectedMediaItems(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnMergingSelectedMediaItems(e);
        }

        private void mivVideos_ExtractingPartFromMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnExtractingPartFromMediaItem(e);
        }

        private void mivVideos_DeletingPart(object sender, CancelMediaItemsOperationEventArgs e)
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
                mivVideos.RemoveMediaItems(mediaItem);
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
            VideosView videosView = d as VideosView;

            if (videosView.Filter != null)
            {
                videosView.Filter.PropertyChanged -= videosView.Filter_PropertyChanged;
                videosView.Filter.PropertyChanged += videosView.Filter_PropertyChanged;
            }
        }

        #endregion

        #region IMediaItemsView Members

        /// <summary>
        /// Gets the media items displayed in the view
        /// </summary>
        public MediaItem[] MediaItems
        {
            get { return mivVideos.MediaItems.ToArray(); }
        }

        /// <summary>
        /// Gets the media items that are currently selected in the view
        /// </summary>
        public MediaItem[] SelectedMediaItems
        {
            get { return mivVideos.SelectedMediaItems; }
        }

        /// <summary>
        /// Gets the index of the currently selected media item
        /// </summary>
        public int SelectedIndex
        {
            get { return mivVideos.SelectedIndex; }
        }

        #endregion
    }
}

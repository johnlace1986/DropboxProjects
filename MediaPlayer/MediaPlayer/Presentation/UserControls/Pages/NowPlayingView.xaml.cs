using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MediaPlayer.Business;
using MediaPlayer.Library.Business;
using System.IO;
using System.Windows.Threading;
using MediaPlayer.EventArgs;
using MediaPlayer.Presentation.Windows;
using Utilities.Business;
using System.Windows.Controls.Primitives;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for NowPlayingView.xaml
    /// </summary>
    public partial class NowPlayingView : UserControl
    {
        #region Events

        /// <summary>
        /// Fires prior to a media item opening
        /// </summary>
        public event CancelMediaItemsOperationEventHandler OpeningMediaItem;

        /// <summary>
        /// Fires when multiple media items are saved
        /// </summary>
        public event MediaItemsEventHandler MediaItemsSaved;

        /// <summary>
        /// Fires when a new file type is added to the system
        /// </summary>
        public event FileTypeEventHandler FileTypeAdded;

        /// <summary>
        /// Fires prior to two or more media items being merged
        /// </summary>
        public event CancelMediaItemsOperationEventHandler MergingSelectedMediaItems;

        /// <summary>
        /// Fires prior to a part of a media item being extracted to another media item
        /// </summary>
        public event CancelMediaItemsOperationEventHandler ExtractingPartFromMediaItem;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(ObservableCollection<MediaItem>), typeof(NowPlayingView), new PropertyMetadata(new ObservableCollection<MediaItem>(), new PropertyChangedCallback(OnMediaItemsPropertyChanged)));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(NowPlayingView), new PropertyMetadata(-1));

        public static readonly DependencyProperty SelectedMediaItemProperty = DependencyProperty.Register("SelectedMediaItem", typeof(MediaItem), typeof(NowPlayingView));

        public static readonly DependencyProperty SelectedMediaItemPartIndexProperty = DependencyProperty.Register("SelectedMediaItemPartIndex", typeof(Int32), typeof(NowPlayingView));

        public static readonly DependencyProperty SelectedMediaItemPartProperty = DependencyProperty.Register("SelectedMediaItemPart", typeof(MediaItemPart), typeof(NowPlayingView));

        public static readonly DependencyProperty CanSkipPreviousProperty = DependencyProperty.Register("CanSkipPrevious", typeof(Boolean), typeof(NowPlayingView));

        public static readonly DependencyProperty CanSkipNextProperty = DependencyProperty.Register("CanSkipNext", typeof(Boolean), typeof(NowPlayingView));

        public static readonly DependencyProperty PlayStateProperty = DependencyProperty.Register("PlayState", typeof(PlayStateEnum), typeof(NowPlayingView));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(TimeSpan), typeof(NowPlayingView));

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(NowPlayingView));

        public static readonly DependencyProperty DisableTrackerTimerProperty = DependencyProperty.Register("DisableTrackerTimer", typeof(Boolean), typeof(NowPlayingView), new PropertyMetadata(false));

        public static readonly DependencyProperty IsPlaylistOpenProperty = DependencyProperty.Register("IsPlaylistOpen", typeof(Boolean), typeof(NowPlayingView), new PropertyMetadata(false));

        public static readonly DependencyProperty ShowControlsProperty = DependencyProperty.Register("ShowControls", typeof(Boolean), typeof(NowPlayingView), new PropertyMetadata(false));

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(NowPlayingView));

        public static readonly DependencyProperty PlaylistWidthProperty = DependencyProperty.Register("PlaylistWidth", typeof(double), typeof(NowPlayingView), new PropertyMetadata((double)360));

        public static readonly DependencyProperty PlaylistMinWidthProperty = DependencyProperty.Register("PlaylistMinWidth", typeof(double), typeof(NowPlayingView), new PropertyMetadata((double)200));

        public static readonly DependencyProperty MediaPlayerWidthProperty = DependencyProperty.Register("MediaPlayerWidth", typeof(double), typeof(NowPlayingView), new PropertyMetadata((double)0));

        public static readonly DependencyProperty MediaPlayerHeightProperty = DependencyProperty.Register("MediaPlayerHeight", typeof(double), typeof(NowPlayingView), new PropertyMetadata((double)0));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(MediaItemFilter), typeof(NowPlayingView), new PropertyMetadata(new MediaItemFilter(), new PropertyChangedCallback(OnFilterPropertyChanged)));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the timer used to hide the controls
        /// </summary>
        private DispatcherTimer controlsTimer = new DispatcherTimer();

        /// <summary>
        /// Gets or sets the date and time of the last mouse move
        /// </summary>
        private DateTime lastMouseMoveTime = DateTime.Now;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the media items currently playing in the view
        /// </summary>
        public ObservableCollection<MediaItem> MediaItems
        {
            get { return GetValue(MediaItemsProperty) as ObservableCollection<MediaItem>; }
            private set { SetValue(MediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the media item in the collection that is currently playing
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return (Int32)GetValue(SelectedIndexProperty); }
            private set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the media item that is currently playing in the view
        /// </summary>
        public MediaItem SelectedMediaItem
        {
            get { return GetValue(SelectedMediaItemProperty) as MediaItem; }
            set { SetValue(SelectedMediaItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index in the selected media item of the part currently playing
        /// </summary>
        public Int32 SelectedMediaItemPartIndex
        {
            get { return (Int32)GetValue(SelectedMediaItemPartIndexProperty); }
            private set { SetValue(SelectedMediaItemPartIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the media item part currently playing
        /// </summary>
        public MediaItemPart SelectedMediaItemPart
        {
            get { return GetValue(SelectedMediaItemPartProperty) as MediaItemPart; }
            private set { SetValue(SelectedMediaItemPartProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not there is a media item after the selected one in the collection
        /// </summary>
        public Boolean CanSkipPrevious
        {
            get { return (Boolean)GetValue(CanSkipPreviousProperty); }
            set { SetValue(CanSkipPreviousProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not there is a media item before the selected one in the collection
        /// </summary>
        public Boolean CanSkipNext
        {
            get { return (Boolean)GetValue(CanSkipNextProperty); }
            set { SetValue(CanSkipNextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current play state of the view
        /// </summary>
        public PlayStateEnum PlayState
        {
            get { return (PlayStateEnum)GetValue(PlayStateProperty); }
            set { SetValue(PlayStateProperty, value); }
        }

        /// <summary>
        /// Gets or sets current position of progress through the media item's playback time
        /// </summary>
        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the volume of the control
        /// </summary>
        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the tracker time should be disabled
        /// </summary>
        public Boolean DisableTrackerTimer
        {
            get { return (Boolean)GetValue(DisableTrackerTimerProperty); }
            set { SetValue(DisableTrackerTimerProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the playist list box is open
        /// </summary>
        public Boolean IsPlaylistOpen
        {
            get { return (Boolean)GetValue(IsPlaylistOpenProperty); }
            set { SetValue(IsPlaylistOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the controls are shown
        /// </summary>
        public Boolean ShowControls
        {
            get { return (Boolean)GetValue(ShowControlsProperty); }
            set { SetValue(ShowControlsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the options currently saved in the system
        /// </summary>
        public Business.Options Options
        {
            get { return GetValue(OptionsProperty) as Business.Options; }
            set { SetValue(OptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the playlist box
        /// </summary>
        public double PlaylistWidth
        {
            get { return (double)GetValue(PlaylistWidthProperty); }
            set { SetValue(PlaylistWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum width the playlist box can be
        /// </summary>
        public double PlaylistMinWidth
        {
            get { return (double)GetValue(PlaylistMinWidthProperty); }
            set { SetValue(PlaylistMinWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the media player
        /// </summary>
        public double MediaPlayerWidth
        {
            get { return (double)GetValue(MediaPlayerWidthProperty); }
            set { SetValue(MediaPlayerWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the media player
        /// </summary>
        public double MediaPlayerHeight
        {
            get { return (double)GetValue(MediaPlayerHeightProperty); }
            set { SetValue(MediaPlayerHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the filter used to filter all videos in the system
        /// </summary>
        public MediaItemFilter Filter
        {
            get { return GetValue(FilterProperty) as MediaItemFilter; }
            set { SetValue(FilterProperty, value); }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Initialises a new instance of the NowPlayingView class
        /// </summary>
        public NowPlayingView()
        {
            InitializeComponent();

            controlsTimer.Interval = TimeSpan.FromSeconds(0.5);
            controlsTimer.Tick += new EventHandler(playlistTimer_Tick);
            controlsTimer.Start();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the OpeningMediaItem event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnOpeningMediaItem(CancelMediaItemsOperationEventArgs e)
        {
            if (OpeningMediaItem != null)
                OpeningMediaItem(this, e);
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
        /// Fires the FileTypeAdded event
        /// </summary>
        /// <param name="fileType">The file type that was added</param>
        private void OnFileTypeAdded(FileType fileType)
        {
            if (FileTypeAdded != null)
                FileTypeAdded(this, new FileTypeEventArgs(fileType));
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
        /// Plays the specified media items, starting from the selected index
        /// </summary>
        /// <param name="mediaItems">Media items to play</param>
        /// <param name="selectedIndex">Index of the first media item in the collection to play</param>
        public void LoadMediaItems(MediaItem[] mediaItems, Int32 selectedIndex)
        {
            mipPlayer.LoadMediaItems(mediaItems, selectedIndex);
        }
        
        /// <summary>
        /// Adds media items to the playlist
        /// </summary>
        /// <param name="mediaItems">Media items being added to the playlist</param>
        public void AddMediaItems(IEnumerable<MediaItem> mediaItems)
        {
            mipPlayer.AddMediaItems(mediaItems);
        }

        /// <summary>
        /// Removes media items from the playlist
        /// </summary>
        /// <param name="mediaItem">Media item being removed</param>
        public void RemoveMediaItems(MediaItem mediaItem)
        {
            RemoveMediaItems(new MediaItem[1] { mediaItem });
        }

        /// <summary>
        /// Removes media items from the playlist
        /// </summary>
        /// <param name="mediaItems">Media items being removed</param>
        public void RemoveMediaItems(IEnumerable<MediaItem> mediaItems)
        {
            mipPlayer.RemoveMediaItems(mediaItems);
        }

        /// <summary>
        /// Plays or pauses the view depending on it's play state
        /// </summary>
        public void TogglePlayPause()
        {
            mipPlayer.TogglePlayPause();
        }

        /// <summary>
        /// Plays the current media item
        /// </summary>
        public void Play()
        {
            mipPlayer.Play();
        }

        /// <summary>
        /// Stops the current media item
        /// </summary>
        public void Stop()
        {
            mipPlayer.Stop();
        }

        /// <summary>
        /// Skips to the next media item
        /// </summary>
        public void SkipNext()
        {
            mipPlayer.SkipNext();
        }

        /// <summary>
        /// Skips to the previous media item
        /// </summary>
        public void SkipPrevious()
        {
            mipPlayer.SkipPrevious();
        }

        /// <summary>
        /// Opens the currently playing media item in full screen mode
        /// </summary>
        public void FullScreen()
        {
            Boolean playOnLoad = false;
            TimeSpan position = Position;
            Int32 selectedIndex = SelectedIndex;

            switch (PlayState)
            {
                case PlayStateEnum.Stopped:
                    return;

                case PlayStateEnum.Playing:
                    mipPlayer.Stop();
                    playOnLoad = true;
                    break;

                case PlayStateEnum.Paused:
                    mipPlayer.Stop();
                    break;
            }

            FullScreenDialog fsd = new FullScreenDialog(playOnLoad, position);
            fsd.Owner = Application.Current.MainWindow;
            fsd.MediaItems = MediaItems;
            fsd.SelectedIndex = selectedIndex;
            fsd.Volume = Volume;
            fsd.Options = Options;
            fsd.OpeningMediaItem += new CancelMediaItemsOperationEventHandler(player_OpeningMediaItem);
            fsd.OpeningMediaItemPart += new MediaItemPartEventHandler(player_OpeningMediaItemPart);
            fsd.MediaItemSaved += new MediaItemEventHandler(element_MediaItemSaved);
            fsd.MediaItemEnded += new MediaItemEventHandler(player_MediaItemEnded);
            fsd.PlayStateChanged += player_PlayStateChanged;
            
            fsd.ShowDialog();

            Volume = fsd.Volume;
            Options.ShowTimeRemaining = fsd.Options.ShowTimeRemaining;

            if (fsd.SelectedIndex == -1)
                mipPlayer.Stop();
            else
            {
                mipPlayer.Play();

                SelectedIndex = fsd.SelectedIndex;
                Position = fsd.Position;
            }
        }

        /// <summary>
        /// Resizes the media player uniformly by the specified change
        /// </summary>
        /// <param name="change">Change in the size of the media player</param>
        private void DragMediaPlayer(float change)
        {
            if (change != 0)
            {
                float factor = (float)MediaPlayerHeight / (float)MediaPlayerWidth;

                float newWidth = (float)MediaPlayerWidth + change;

                if (newWidth > 0)
                {
                    float newHeight = (float)MediaPlayerHeight + (change * factor);

                    if (newHeight > 0)
                    {
                        SizeF newSize = GeneralMethods.ResizeBounds(newWidth, newHeight, (float)brdMediaPlayer.ActualWidth, (float)brdMediaPlayer.ActualHeight);

                        const float minWidth = 200;

                        MediaPlayerWidth = (newSize.Width < minWidth ? minWidth : newSize.Width);
                        MediaPlayerHeight = (newSize.Width < minWidth ? minWidth * factor : newSize.Height);
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            ShowControls = true;
            lastMouseMoveTime = DateTime.Now;

            if (!controlsTimer.IsEnabled)
                controlsTimer.Start();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeF newSize = GeneralMethods.ResizeBounds((float)MediaPlayerWidth, (float)MediaPlayerHeight, (float)brdMediaPlayer.ActualWidth, (float)brdMediaPlayer.ActualHeight);

            MediaPlayerWidth = newSize.Width;
            MediaPlayerHeight = newSize.Height;

        }

        private void MediaItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BindingExpression be = txtPlaylistSummary.GetBindingExpression(TextBlock.TextProperty);
            be.UpdateTarget();
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BindingExpression summary = txtPlaylistSummary.GetBindingExpression(TextBlock.TextProperty);
            summary.UpdateTarget();
        }

        private void playlistTimer_Tick(object sender, System.EventArgs e)
        {
            if (PlayState == PlayStateEnum.Playing)
            {
                TimeSpan timeSinceLastMouseMove = DateTime.Now - lastMouseMoveTime;

                if (timeSinceLastMouseMove.TotalSeconds >= 2)
                {
                    ShowControls = false;
                    controlsTimer.Stop();
                    Cursor = Cursors.None;
                }
            }
        }

        private void player_OpeningMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnOpeningMediaItem(e);
        }

        private void player_OpeningMediaItemPart(object sender, MediaItemPartEventArgs e)
        {
            MediaPlayerWidth = mipPlayer.MediaItemWidth;
            MediaPlayerHeight = mipPlayer.MediaItemHeight;
        }

        private void mipPlayer_ScreenDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            FullScreen();
        }

        private void mipPlayer_MediaItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MediaItems = new ObservableCollection<MediaItem>(mipPlayer.MediaItems);
        }

        private void element_MediaItemSaved(object sender, MediaItemEventArgs e)
        {
            OnMediaItemsSaved(new MediaItem[1] { e.MediaItem });
        }

        private void player_MediaItemEnded(object sender, MediaItemEventArgs e)
        {
            if (Options.RemoveMediaItemFromNowPlayingOnFinish)
                RemoveMediaItems(e.MediaItem);
        }

        private void player_PlayStateChanged(object sender, System.EventArgs e)
        {
            if (PlayState == PlayStateEnum.Stopped)
            {
                MediaPlayerWidth = 0;
                MediaPlayerHeight = 0;
            }
        }

        private void thmMediaPlayerLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DragMediaPlayer(-(float)e.HorizontalChange);
        }

        private void thmMediaPlayerRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DragMediaPlayer((float)e.HorizontalChange);
        }

        private void thmMediaPlayerTop_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DragMediaPlayer(-(float)e.VerticalChange);
        }

        private void thmMediaPlayerBottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DragMediaPlayer((float)e.VerticalChange);
        }

        private void mivNowPlaying_FileTypeAdded(object sender, FileTypeEventArgs e)
        {
            OnFileTypeAdded(e.FileType);
        }

        private void mivNowPlaying_MediaItemsSaved(object sender, MediaItemsEventArgs e)
        {
            OnMediaItemsSaved(e.MediaItems);
        }

        private void mivNowPlaying_MediaItemsDeleted(object sender, MediaItemsEventArgs e)
        {
            RemoveMediaItems(e.MediaItems);
        }

        private void mivNowPlaying_MergingSelectedMediaItems(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnMergingSelectedMediaItems(e);
        }

        private void mivNowPlaying_ExtractingPartFromMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnExtractingPartFromMediaItem(e);
        }

        //private void lstNowPlaying_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (lstNowPlaying.SelectedItem != null)
        //    {
        //        SelectedIndex = lstNowPlaying.SelectedIndex;

        //        if (PlayState != PlayStateEnum.Playing)
        //            Play();
        //    }
        //}

        //private void lstNowPlaying_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    lstNowPlaying.SelectedItems.Clear();
        //}

        //private void lstNowPlaying_KeyDown(object sender, KeyEventArgs e)
        //{
        //    MediaItem mediaItem = lstNowPlaying.SelectedItem as MediaItem;

        //    if (mediaItem != null)
        //    {
        //        switch (e.Key)
        //        {
        //            case Key.Delete:
        //                mipPlayer.RemoveMediaItems(mediaItem);
        //                break;
        //        }
        //    }
        //}

        private void btnPartsDoNotExist_Click(object sender, RoutedEventArgs e)
        {
            Button btnRemovedMediaItem = sender as Button;
            MediaItem mediaItem = btnRemovedMediaItem.DataContext as MediaItem;

            IntelligentString message = "The following parts belonging to " + mediaItem.Name + " do not exist:" + Environment.NewLine + Environment.NewLine;

            foreach (MediaItemPart part in mediaItem.Parts)
                if (!File.Exists(part.Location.Value))
                    message += part.IndexString + ": " + part.Location + Environment.NewLine;

            message += Environment.NewLine + "Remove " + mediaItem.Name + " from the Now Playing playlist?";

            if (GeneralMethods.MessageBox(message.Value, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                RemoveMediaItems(mediaItem);
        }

        private void btnRemovedMediaItem_Click(object sender, RoutedEventArgs e)
        {
            Button btnRemovedMediaItem = sender as Button;
            MediaItem mediaItem = btnRemovedMediaItem.DataContext as MediaItem;

            mipPlayer.RemoveMediaItems(mediaItem);
        }

        private void btnClearNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            Stop();
            MediaItems.Clear();
        }

        private void thmPlaylistWidth_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double newWidth = PlaylistWidth - e.HorizontalChange;

            if (newWidth <= PlaylistMinWidth)
                PlaylistWidth = PlaylistMinWidth;
            else if (newWidth >= brdPlaylist.ActualWidth)
                PlaylistWidth = brdPlaylist.ActualWidth;
            else
                PlaylistWidth = newWidth;

        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fires when the value of the MediaItemsProperty changes
        /// </summary>
        /// <param name="d">Dependency object containing the dependency property that changed</param>
        /// <param name="e">Events passed to the argument</param>
        private static void OnMediaItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NowPlayingView npv = d as NowPlayingView;

            if (npv.MediaItems != null)
            {
                npv.MediaItems.CollectionChanged -= npv.MediaItems_CollectionChanged;
                npv.MediaItems.CollectionChanged += npv.MediaItems_CollectionChanged;
            }
        }

        /// <summary>
        /// Fires when the value of the FilterProperty changes
        /// </summary>
        /// <param name="d">Dependency object containing the dependency property that changed</param>
        /// <param name="e">Events passed to the argument</param>
        private static void OnFilterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NowPlayingView nowPlayingView = d as NowPlayingView;

            if (nowPlayingView.Filter != null)
            {
                nowPlayingView.Filter.PropertyChanged -= nowPlayingView.Filter_PropertyChanged;
                nowPlayingView.Filter.PropertyChanged += nowPlayingView.Filter_PropertyChanged;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MediaPlayer.EventArgs;
using MediaPlayer.Library.Business;
using MediaPlayer.Business;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for FullScreenDialog.xaml
    /// </summary>
    public partial class FullScreenDialog : Window
    {
        #region Events

        /// <summary>
        /// Fires prior to a media item opening
        /// </summary>
        public event CancelMediaItemsOperationEventHandler OpeningMediaItem;

        /// <summary>
        /// Fires when a media item part is opening
        /// </summary>
        public event MediaItemPartEventHandler OpeningMediaItemPart;

        /// <summary>
        /// Fires when a media item is saved
        /// </summary>
        public event MediaItemEventHandler MediaItemSaved;

        /// <summary>
        /// Fires when a media item is ending
        /// </summary>
        public event MediaItemEventHandler MediaItemEnding;

        /// <summary>
        /// Fires when a media item has ended
        /// </summary>
        public event MediaItemEventHandler MediaItemEnded;

        /// <summary>
        /// Fires when the play state of the media player changes
        /// </summary>
        public event EventHandler PlayStateChanged;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(ObservableCollection<MediaItem>), typeof(FullScreenDialog));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(FullScreenDialog));

        public static readonly DependencyProperty SelectedMediaItemProperty = DependencyProperty.Register("SelectedMediaItem", typeof(MediaItem), typeof(FullScreenDialog));

        public static readonly DependencyProperty SelectedMediaItemPartIndexProperty = DependencyProperty.Register("SelectedMediaItemPartIndex", typeof(Int32), typeof(FullScreenDialog));

        public static readonly DependencyProperty SelectedMediaItemPartProperty = DependencyProperty.Register("SelectedMediaItemPart", typeof(MediaItemPart), typeof(FullScreenDialog));

        public static readonly DependencyProperty PlayStateProperty = DependencyProperty.Register("PlayState", typeof(PlayStateEnum), typeof(FullScreenDialog));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(TimeSpan), typeof(FullScreenDialog));

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(FullScreenDialog));

        public static readonly DependencyProperty DisableTrackerTimerProperty = DependencyProperty.Register("DisableTrackerTimer", typeof(Boolean), typeof(FullScreenDialog));

        public static readonly DependencyProperty ShowControlsProperty = DependencyProperty.Register("ShowControls", typeof(Boolean), typeof(FullScreenDialog), new PropertyMetadata(true));

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Options), typeof(FullScreenDialog), new PropertyMetadata(null));

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
        /// Gets or sets the media items currently playing in the window
        /// </summary>
        public ObservableCollection<MediaItem> MediaItems
        {
            get { return GetValue(MediaItemsProperty) as ObservableCollection<MediaItem>; }
            set { SetValue(MediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the media item in the collection that is currently playing
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return (Int32)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the media item that is currently playing in the window
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
        /// Gets or sets a value determining whether or not the media item player should start playing when the window loads
        /// </summary>
        private Boolean PlayOnLoad { get; set; }

        /// <summary>
        /// Gets or sets the value to set the position of the player to on load
        /// </summary>
        private TimeSpan PositionOnLoad { get; set; }

        /// <summary>
        /// Gets or sets a value determining whether or not the tracker time should be disabled
        /// </summary>
        public Boolean DisableTrackerTimer
        {
            get { return (Boolean)GetValue(DisableTrackerTimerProperty); }
            set { SetValue(DisableTrackerTimerProperty, value); }
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
        /// Gets or sets a value determining whether or not the media player controls should be shown
        /// </summary>
        public Boolean ShowControls
        {
            get { return (Boolean)GetValue(ShowControlsProperty); }
            set { SetValue(ShowControlsProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FullScreenDialog class
        /// </summary>
        /// <param name="mediaItems">Media items currently playing in the window</param>
        /// <param name="selectedIndex">Index of the media item in the collection that is currently playing</param>
        /// <param name="position">Current position of progress through the media item's playback time</param>
        /// <param name="volume">Volume of the control</param>
        /// <param name="playOnLoad">Value determining whether or not the media item player should start playing when the window loads</param>
        public FullScreenDialog(Boolean playOnLoad, TimeSpan positionOnLoad)
        {
            InitializeComponent();

            PlayOnLoad = playOnLoad;
            PositionOnLoad = positionOnLoad;

            controlsTimer.Interval = TimeSpan.FromSeconds(0.5);
            controlsTimer.Tick += new EventHandler(controlsTimer_Tick);
            controlsTimer.Start();
        }

        #endregion

        #region Instance Methods

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            //stops media played from crashing when moving to another monitor
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            HwndTarget hwndTarget = hwndSource.CompositionTarget;
            hwndTarget.RenderMode = RenderMode.SoftwareOnly;
            mipPlayer.meNowPlaying.Play();
            base.OnSourceInitialized(e);
        }

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
        /// Fires the OpeningMediaItemPart event
        /// </summary>
        /// <param name="part">Part that is opening</param>
        private void OnOpeningMediaItemPart(MediaItemPart part)
        {
            if (OpeningMediaItemPart != null)
                OpeningMediaItemPart(this, new MediaItemPartEventArgs(part));
        }

        /// <summary>
        /// Fires the MediaItemSaved event
        /// </summary>
        /// <param name="mediaItem">Media item that was saved</param>
        private void OnMediaItemSaved(MediaItem mediaItem)
        {
            if (MediaItemSaved != null)
                MediaItemSaved(this, new MediaItemEventArgs(mediaItem));
        }

        /// <summary>
        /// Fires the MediaItemEnding event
        /// </summary>
        /// <param name="mediaItem">Arguments to pass to the event</param>
        private void OnMediaItemEnding(MediaItem mediaItem)
        {
            if (MediaItemEnding != null)
                MediaItemEnding(this, new MediaItemEventArgs(mediaItem));
        }

        /// <summary>
        /// Fires the MediaItemEnded event
        /// </summary>
        /// <param name="mediaItem">Arguments to pass to the event</param>
        private void OnMediaItemEnded(MediaItem mediaItem)
        {
            if (MediaItemEnded != null)
                MediaItemEnded(this, new MediaItemEventArgs(mediaItem));
        }

        /// <summary>
        /// Fires the PlayStateChanged event
        /// </summary>
        private void OnPlayStateChanged()
        {
            if (PlayStateChanged != null)
                PlayStateChanged(this, new System.EventArgs());
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

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mipPlayer.Play();
            mipPlayer.Position = PositionOnLoad;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PlayState == PlayStateEnum.Playing)
                mipPlayer.Pause();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            ShowControls = true;
            lastMouseMoveTime = DateTime.Now;

            if (!controlsTimer.IsEnabled)
                controlsTimer.Start();
        }

        private void controlsTimer_Tick(object sender, System.EventArgs e)
        {
            TimeSpan timeSinceLastMouseMove = DateTime.Now - lastMouseMoveTime;

            if (timeSinceLastMouseMove.TotalSeconds >= 5)
            {
                ShowControls = false;
                controlsTimer.Stop();
                Cursor = Cursors.None;
            }
        }

        private void mipPlayer_OpeningMediaItem(object sender, CancelMediaItemsOperationEventArgs e)
        {
            OnOpeningMediaItem(e);
        }

        private void mipPlayer_OpeningMediaItemPart(object sender, MediaItemPartEventArgs e)
        {
            OnOpeningMediaItemPart(e.Part);
        }

        private void mipPlayer_ScreenDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        private void mipPlayer_PlayStateChanged(object sender, System.EventArgs e)
        {
            OnPlayStateChanged();

            if (mipPlayer.PlayState == PlayStateEnum.Stopped)
                DialogResult = true;
        }

        private void mipPlayer_MediaItemSaved(object sender, MediaItemEventArgs e)
        {
            OnMediaItemSaved(e.MediaItem);
        }

        private void mipPlayer_MediaItemEnding(object sender, MediaItemEventArgs e)
        {
            OnMediaItemEnding(e.MediaItem);
        }

        private void mipPlayer_MediaItemEnded(object sender, MediaItemEventArgs e)
        {
            OnMediaItemEnded(e.MediaItem);

            if (Options.RemoveMediaItemFromNowPlayingOnFinish)
                RemoveMediaItems(e.MediaItem);
        }

        private void btnSkipPrevious_Click(object sender, RoutedEventArgs e)
        {
            mipPlayer.SkipPrevious();
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            mipPlayer.TogglePlayPause();
        }

        private void btnSkipNext_Click(object sender, RoutedEventArgs e)
        {
            mipPlayer.SkipNext();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mipPlayer.Stop();
        }

        #endregion
    }
}

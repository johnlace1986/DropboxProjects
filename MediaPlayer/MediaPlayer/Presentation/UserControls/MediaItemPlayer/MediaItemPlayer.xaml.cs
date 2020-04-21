using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MediaPlayer.EventArgs;
using MediaPlayer.Library.Business;
using MediaPlayer.Business;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MediaPlayer.Presentation.UserControls.MediaItemPlayer
{
    /// <summary>
    /// Interaction logic for MediaItemPlayer.xaml
    /// </summary>
    public partial class MediaItemPlayer : UserControl
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
        /// Fires when the media item player is double clicked
        /// </summary>
        public event MouseButtonEventHandler ScreenDoubleClicked;

        /// <summary>
        /// Fires when the play state of the media player changes
        /// </summary>
        public event EventHandler PlayStateChanged;

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
        /// Fires when the media items collection changes
        /// </summary>
        public event NotifyCollectionChangedEventHandler MediaItemsChanged;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(ObservableCollection<MediaItem>), typeof(MediaItemPlayer), new PropertyMetadata(new ObservableCollection<MediaItem>(), new PropertyChangedCallback(OnMediaItemsPropertyChanged)));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(MediaItemPlayer), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexPropertyChanged)));

        public static readonly DependencyProperty SelectedMediaItemProperty = DependencyProperty.Register("SelectedMediaItem", typeof(MediaItem), typeof(MediaItemPlayer), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedMediaItemPropertyChanged)));

        public static readonly DependencyProperty SelectedMediaItemPartIndexProperty = DependencyProperty.Register("SelectedMediaItemPartIndex", typeof(Int32), typeof(MediaItemPlayer), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedMediaItemPartIndexPropertyChanged)));

        public static readonly DependencyProperty SelectedMediaItemPartProperty = DependencyProperty.Register("SelectedMediaItemPart", typeof(MediaItemPart), typeof(MediaItemPlayer));

        public static readonly DependencyProperty CanSkipPreviousProperty = DependencyProperty.Register("CanSkipPrevious", typeof(Boolean), typeof(MediaItemPlayer));

        public static readonly DependencyProperty CanSkipNextProperty = DependencyProperty.Register("CanSkipNext", typeof(Boolean), typeof(MediaItemPlayer));

        public static readonly DependencyProperty PlayStateProperty = DependencyProperty.Register("PlayState", typeof(PlayStateEnum), typeof(MediaItemPlayer), new PropertyMetadata(new PropertyChangedCallback(OnPlayStatePropertyChanged)));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(TimeSpan), typeof(MediaItemPlayer), new PropertyMetadata(TimeSpan.FromMilliseconds(0), new PropertyChangedCallback(OnPositionChanged)));

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(MediaItemPlayer));

        public static readonly DependencyProperty DisableTrackerTimerProperty = DependencyProperty.Register("DisableTrackerTimer", typeof(Boolean), typeof(MediaItemPlayer), new PropertyMetadata(false));

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(MediaItemPlayer));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the timer used to refresh the tracker
        /// </summary>
        private DispatcherTimer trackerTimer = new DispatcherTimer();

        /// <summary>
        /// Gets or sets a value determining whether or not the currently playing part should be update when the position changes
        /// </summary>
        private Boolean positionUpdatesPart = true;

        /// <summary>
        /// Gets or sets a value determining whether or not a media item is currently being removed
        /// </summary>
        private Boolean removingMediaItem = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the media items currently playing in the view
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
            set
            {
                Int32 oldValue = SelectedIndex;

                SetValue(SelectedIndexProperty, value);

                if (value == oldValue)
                    OnSelectedIndexPropertyChanged(this, new DependencyPropertyChangedEventArgs(SelectedIndexProperty, oldValue, value));
            }
        }

        /// <summary>
        /// Gets or sets the media item that is currently playing in the view
        /// </summary>
        public MediaItem SelectedMediaItem
        {
            get { return GetValue(SelectedMediaItemProperty) as MediaItem; }
            set
            {
                MediaItem oldValue = SelectedMediaItem;

                SetValue(SelectedMediaItemProperty, value);

                if (value == oldValue)
                    OnSelectedMediaItemPropertyChanged(this, new DependencyPropertyChangedEventArgs(SelectedMediaItemProperty, oldValue, value));
            }
        }

        /// <summary>
        /// Gets or sets the index in the selected media item of the part currently playing
        /// </summary>
        public Int32 SelectedMediaItemPartIndex
        {
            get { return (Int32)GetValue(SelectedMediaItemPartIndexProperty); }
            set
            {
                Int32 oldValue = SelectedMediaItemPartIndex;

                SetValue(SelectedMediaItemPartIndexProperty, value);

                if (value == oldValue)
                    OnSelectedMediaItemPartIndexPropertyChanged(this, new DependencyPropertyChangedEventArgs(SelectedMediaItemPartIndexProperty, oldValue, value));
            }
        }

        /// <summary>
        /// Gets or sets the media item part currently playing
        /// </summary>
        public MediaItemPart SelectedMediaItemPart
        {
            get { return GetValue(SelectedMediaItemPartProperty) as MediaItemPart; }
            set { SetValue(SelectedMediaItemPartProperty, value); }
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
            set
            {
                SetValue(PositionProperty, value);
            }
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
        /// Gets or sets the stretch value for the rendered media
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// Gets the width of the currently playing media item
        /// </summary>
        public Int32 MediaItemWidth
        {
            get { return meNowPlaying.NaturalVideoWidth; }
        }

        /// <summary>
        /// Gets the height of the currently playing media item
        /// </summary>
        public Int32 MediaItemHeight
        {
            get { return meNowPlaying.NaturalVideoHeight; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemPlayer class
        /// </summary>
        public MediaItemPlayer()
        {
            InitializeComponent();
            
            trackerTimer.Interval = TimeSpan.FromSeconds(0.1);
            trackerTimer.Tick += new EventHandler(timer_Tick);
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
        /// Fires the OpeningMediaItemPart event
        /// </summary>
        /// <param name="part">Part that is opening</param>
        private void OnOpeningMediaItemPart(MediaItemPart part)
        {
            if (OpeningMediaItemPart != null)
                OpeningMediaItemPart(this, new MediaItemPartEventArgs(part));
        }

        /// <summary>
        /// Fires the ScreenDoubleClicked event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnScreenDoubleClicked(MouseButtonEventArgs e)
        {
            if (ScreenDoubleClicked != null)
                ScreenDoubleClicked(this, e);
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
        /// Fires then MediaItemsChanged event
        /// </summary>
        /// <param name="mediaItems">Arguments to pass to the event</param>
        private void OnMediaItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (MediaItemsChanged != null)
                MediaItemsChanged(this, e);
        }

        /// <summary>
        /// Plays the specified media items, starting from the selected index
        /// </summary>
        /// <param name="mediaItems">Media items to play</param>
        /// <param name="selectedIndex">Index of the first media item in the collection to play</param>
        public void LoadMediaItems(MediaItem[] mediaItems, Int32 selectedIndex)
        {
            MediaItems = new ObservableCollection<MediaItem>(mediaItems);
            SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// Adds media items to the playlist
        /// </summary>
        /// <param name="mediaItems">Media items being added to the playlist</param>
        public void AddMediaItems(IEnumerable<MediaItem> mediaItems)
        {
            PlayStateEnum previousPlayState = PlayState;

            foreach (MediaItem mediaItem in mediaItems)
                MediaItems.Add(mediaItem);

            if (previousPlayState == PlayStateEnum.Stopped)
                Stop();
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
            List<MediaItem> lstMediaItem = new List<MediaItem>(MediaItems);

            Int32 selectedIndex = SelectedIndex;

            foreach (MediaItem mediaItem in mediaItems)
            {
                if (mediaItem == SelectedMediaItem)
                    SkipNext();

                int position = lstMediaItem.IndexOf(mediaItem);

                if (position < selectedIndex)
                    selectedIndex--;

                lstMediaItem.Remove(mediaItem);
            }

            removingMediaItem = true;

            if (lstMediaItem.Count > 0)
            {
                if (selectedIndex < lstMediaItem.Count)
                    SelectedIndex = selectedIndex;
                else
                    SelectedMediaItem = null;
            }

            MediaItems = new ObservableCollection<MediaItem>(lstMediaItem);

            removingMediaItem = false;
            SetCanSkip();
        }

        /// <summary>
        /// Plays or pauses the view depending on it's play state
        /// </summary>
        public void TogglePlayPause()
        {
            switch (PlayState)
            {
                case PlayStateEnum.Stopped:
                    Play();
                    break;
                case PlayStateEnum.Playing:
                    Pause();
                    break;
                case PlayStateEnum.Paused:
                    Continue(true);
                    break;
            }
        }

        /// <summary>
        /// Plays the current media item
        /// </summary>
        public void Play()
        {
            Play(SkipDirectionEnum.Forward);
        }

        /// <summary>
        /// Plays the current media item
        /// </summary>
        /// <param name="skipDirection">Determines the direction to skip to if the current media item cannot be played</param>
        public void Play(SkipDirectionEnum skipDirection)
        {
            if (MediaItems.Count > 0)
            {
                if (SelectedIndex == -1)
                    SelectedIndex = 0;

                if ((SelectedMediaItemPart != null) && (SelectedMediaItem.Parts.PartsExist))
                {
                    if (PlayState != PlayStateEnum.Paused)
                        Position = TimeSpan.FromMilliseconds(0);

                    Continue(false);
                }
                else
                {
                    switch (skipDirection)
                    {
                        case SkipDirectionEnum.Forward:
                            SkipNext();
                            break;

                        case SkipDirectionEnum.Backwards:
                            SkipPrevious();
                            break;

                        default:
                            throw new ArgumentException("Unknown SkipDirectionEnum value");
                    }
                }
            }
        }

        /// <summary>
        /// Plays the media item
        /// </summary>
        private void Continue(Boolean setPart)
        {
            meNowPlaying.Play();
            PlayState = PlayStateEnum.Playing;

            if (setPart)
                SetPart();

            trackerTimer.Start();
        }

        /// <summary>
        /// Pauses the current media item
        /// </summary>
        public void Pause()
        {
            meNowPlaying.Pause();
            PlayState = PlayStateEnum.Paused;

            trackerTimer.Stop();
        }

        /// <summary>
        /// Stops the current media item
        /// </summary>
        public void Stop()
        {
            meNowPlaying.Stop();
            SelectedIndex = -1;
            PlayState = PlayStateEnum.Stopped;
            trackerTimer.Stop();
        }

        /// <summary>
        /// Skips to the next media item
        /// </summary>
        public void SkipNext()
        {            
            if (CanSkipNext)
            {
                SelectedIndex++;
                Play();
            }
            else
                Stop();
        }

        /// <summary>
        /// Skips to the previous media item
        /// </summary>
        public void SkipPrevious()
        {
            if (CanSkipPrevious)
            {
                SelectedIndex--;
                Play(SkipDirectionEnum.Backwards);
            }
            else
            {
                if (SelectedMediaItem != null)
                {
                    if (SelectedMediaItem.Parts.PartsExist)
                        Position = TimeSpan.FromSeconds(0);
                    else
                        Stop();
                }
            }
        }

        /// <summary>
        /// Sets the currently playing part to the part that is found at the current duration
        /// </summary>
        private void SetPart()
        {
            TimeSpan position = TimeSpan.FromMilliseconds(0);

            if (Position != null)
            {
                TimeSpan duration = new TimeSpan(0, 0, (int)Position.TotalSeconds);
                int index;

                if (SelectedMediaItem != null)
                {
                    SelectedMediaItem.Parts.GetPartFromDuration(duration, out index, out position);

                    if (SelectedMediaItemPartIndex != index)
                        SelectedMediaItemPartIndex = index;
                }
            }

            meNowPlaying.Position = position;
        }

        /// <summary>
        /// Determines whether the user can skip to the previous or next media item
        /// </summary>
        private void SetCanSkip()
        {
            CanSkipPrevious = (SelectedIndex > 0);
            CanSkipNext = ((SelectedIndex >= 0) && (SelectedIndex < (MediaItems.Count - 1)));
        }

        #endregion

        #region Event Handlers

        private void MediaItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetCanSkip();

            OnMediaItemsChanged(e);
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            if (!DisableTrackerTimer)
            {
                if (PlayState == PlayStateEnum.Playing)
                {
                    if (meNowPlaying.Position.TotalSeconds <= SelectedMediaItem.Parts.Duration.TotalSeconds)
                    {
                        TimeSpan trackerValue = TimeSpan.FromMilliseconds(0);

                        for (int i = 0; i < SelectedMediaItemPartIndex; i++)
                            trackerValue += SelectedMediaItem.Parts[i].Duration;

                        trackerValue += meNowPlaying.Position;
                        positionUpdatesPart = false;
                        Position = trackerValue;
                        positionUpdatesPart = true;
                    }
                }
            }
        }

        private void meNowPlaying_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (meNowPlaying.NaturalDuration.HasTimeSpan)
            {
                if (SelectedMediaItemPart.Duration != meNowPlaying.NaturalDuration.TimeSpan)
                {
                    SelectedMediaItem.SetPartDuration(SelectedMediaItemPartIndex, meNowPlaying.NaturalDuration.TimeSpan);
                    SelectedMediaItem.Save();

                    OnMediaItemSaved(SelectedMediaItem);
                }

                OnOpeningMediaItemPart(SelectedMediaItemPart);
            }
        }

        private void meNowPlaying_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (SelectedMediaItemPartIndex < (SelectedMediaItem.Parts.Count - 1))
                SelectedMediaItemPartIndex++;
            else
            {
                MediaItem selectedMediaItem = SelectedMediaItem;
                OnMediaItemEnding(selectedMediaItem);

                try
                {
                    SelectedMediaItem.Played();
                    SelectedMediaItem.Save();
                    OnMediaItemSaved(SelectedMediaItem);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Could not update play history for media item: " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                SkipNext();
                OnMediaItemEnded(selectedMediaItem);
            }
        }

        private void meNowPlaying_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                OnScreenDoubleClicked(e);
        }

        #endregion

        #region Static Methods

        private static void OnMediaItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemPlayer mip = d as MediaItemPlayer;

            if (!mip.removingMediaItem)
            {
                Int32 oldValue = mip.SelectedIndex;
                Int32 newValue = -1;

                if (mip.MediaItems.Count > 0)
                    newValue = 0;

                if (oldValue == newValue)
                    OnSelectedIndexPropertyChanged(d, new DependencyPropertyChangedEventArgs(SelectedIndexProperty, oldValue, newValue));
                else
                    mip.SelectedIndex = newValue;
            }

            if (mip.MediaItems != null)
            {
                mip.MediaItems.CollectionChanged -= mip.MediaItems_CollectionChanged;
                mip.MediaItems.CollectionChanged += mip.MediaItems_CollectionChanged;
            }
        }

        private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemPlayer mip = d as MediaItemPlayer;

            if (!mip.removingMediaItem)
            {
                mip.CanSkipPrevious = false;
                mip.CanSkipNext = false;

                if (mip.SelectedIndex == -1)
                {
                    mip.SelectedMediaItem = null;
                    mip.Position = TimeSpan.FromSeconds(0);

                    return;
                }
                else
                {
                    MediaItem selectedMediaItem = mip.MediaItems[mip.SelectedIndex];

                    CancelMediaItemsOperationEventArgs cmioea = new CancelMediaItemsOperationEventArgs(new MediaItem[1] { selectedMediaItem });
                    mip.OnOpeningMediaItem(cmioea);

                    if (cmioea.Cancel)
                    {
                        mip.SkipNext();
                        return;
                    }

                    mip.SelectedMediaItem = selectedMediaItem;
                }
            }

            mip.SetCanSkip();
        }

        private static void OnSelectedMediaItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemPlayer npv = d as MediaItemPlayer;
            npv.SelectedMediaItemPartIndex = 0;
        }

        private static void OnSelectedMediaItemPartIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemPlayer mip = d as MediaItemPlayer;

            if (mip.SelectedMediaItem != null)
            {
                MediaItemPart selectedMediaItemPart = mip.SelectedMediaItem.Parts[mip.SelectedMediaItemPartIndex];
                mip.SelectedMediaItemPart = selectedMediaItemPart;
            }
            else
                mip.SelectedMediaItemPart = null;
        }

        private static void OnPlayStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemPlayer mip = d as MediaItemPlayer;

            mip.OnPlayStateChanged();

        }

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemPlayer mip = d as MediaItemPlayer;

            if (mip.positionUpdatesPart)
                if (mip.meNowPlaying != null)
                    mip.SetPart();
        }

        #endregion
    }
}

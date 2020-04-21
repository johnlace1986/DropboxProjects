using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaPlayer.Library.Business;
using System.Windows.Threading;

namespace MediaPlayer.Presentation.UserControls.MediaItemPlayer
{
    /// <summary>
    /// Interaction logic for Tracker.xaml
    /// </summary>
    public partial class Tracker : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Tracker), new PropertyMetadata(new CornerRadius()));

        public static readonly DependencyProperty SelectedMediaItemProperty = DependencyProperty.Register("SelectedMediaItem", typeof(MediaItem), typeof(Tracker), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedMediaItemPropertyChanged)));

        public static readonly DependencyProperty NameTickerPointerProperty = DependencyProperty.Register("NameTickerPointer", typeof(Int32), typeof(Tracker), new PropertyMetadata(0));

        public static readonly DependencyProperty DescriptionTickerPointerProperty = DependencyProperty.Register("DescriptionTickerPointer", typeof(Int32), typeof(Tracker), new PropertyMetadata(0));

        public static readonly DependencyProperty DisableTrackerTimerProperty = DependencyProperty.Register("DisableTrackerTimer", typeof(Boolean), typeof(Tracker));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(TimeSpan), typeof(Tracker));

        public static readonly DependencyProperty ShowTimeRemainingProperty = DependencyProperty.Register("ShowTimeRemaining", typeof(Boolean), typeof(Tracker), new PropertyMetadata(false));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the timer used to tick over the name of the selected media item
        /// </summary>
        private DispatcherTimer nameTicker = new DispatcherTimer();

        /// <summary>
        /// Gets or sets the timer used to tick over the description of the selected media item
        /// </summary>
        private DispatcherTimer descriptionTicker = new DispatcherTimer();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that represents the degree to which the corners of the Tracker are rounded
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetAnimationBaseValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the media item that is selected in the tracker
        /// </summary>
        public MediaItem SelectedMediaItem
        {
            get { return GetValue(SelectedMediaItemProperty) as MediaItem; }
            set { SetValue(SelectedMediaItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the position in the name of the selected media item to start from
        /// </summary>
        public Int32 NameTickerPointer
        {
            get { return (Int32)GetValue(NameTickerPointerProperty); }
            set { SetValue(NameTickerPointerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the position in the description of the selected media item to start from
        /// </summary>
        public Int32 DescriptionTickerPointer
        {
            get { return (Int32)GetValue(DescriptionTickerPointerProperty); }
            set { SetValue(DescriptionTickerPointerProperty, value); }
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
        /// Gets or sets current position of progress through the selected media item's playback time
        /// </summary>
        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the remaining time should be displayed rather than the length of the selected media item
        /// </summary>
        public Boolean ShowTimeRemaining
        {
            get { return (Boolean)GetValue(ShowTimeRemainingProperty); }
            set { SetValue(ShowTimeRemainingProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Tracker class
        /// </summary>
        public Tracker()
        {
            InitializeComponent();

            //sldTracker.AddHandler(Slider.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(sldTracker_PreviewMouseLeftButtonDown), true);

            nameTicker.Interval = TimeSpan.FromSeconds(0.2);
            nameTicker.Tick += new EventHandler(nameTicker_Tick);
            nameTicker.Start();

            descriptionTicker.Interval = TimeSpan.FromSeconds(0.2);
            descriptionTicker.Tick += new EventHandler(descriptionTicker_Tick);
            descriptionTicker.Start();
        }

        #endregion

        #region Event Handlers

        private void nameTicker_Tick(object sender, System.EventArgs e)
        {
            if (SelectedMediaItem != null)
            {
                NameTickerPointer++;

                if (NameTickerPointer >= SelectedMediaItem.Name.Length)
                    NameTickerPointer = 0;
            }
        }

        private void descriptionTicker_Tick(object sender, System.EventArgs e)
        {
            if (SelectedMediaItem != null)
            {
                DescriptionTickerPointer++;

                if (DescriptionTickerPointer >= SelectedMediaItem.Description.Length)
                    DescriptionTickerPointer = 0;
            }
        }

        private void sldTracker_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisableTrackerTimer = true;
        }

        private void sldTracker_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisableTrackerTimer = false;
        }

        private void lblTimeRemaining_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowTimeRemaining = !ShowTimeRemaining;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fires when the value of the SelectedMediaItemProperty dependency property changes
        /// </summary>
        /// <param name="d">Dependency object containing the property that changed</param>
        /// <param name="e">Arguments passed to the event</param>
        private static void OnSelectedMediaItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Tracker tracker = d as Tracker;

            tracker.NameTickerPointer = 0;
            tracker.DescriptionTickerPointer = 0;
        }

        #endregion
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MediaPlayer.Presentation.UserControls.Animations
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Int32), typeof(Loading));

        public static readonly DependencyProperty BlobsProperty = DependencyProperty.Register("Blobs", typeof(Int32), typeof(Loading));

        public static readonly DependencyProperty BlobSizeProperty = DependencyProperty.Register("BlobSize", typeof(double), typeof(Loading));

        public static readonly DependencyProperty BlobMarginProperty = DependencyProperty.Register("BlobMargin", typeof(Thickness), typeof(Loading));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the timer used to animate the control
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the control
        /// </summary>
        public Int32 Value
        {
            get { return (Int32)GetValue(Loading.ValueProperty); }
            private set { SetValue(Loading.ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of blobs in the control
        /// </summary>
        public Int32 Blobs
        {
            get { return (Int32)GetValue(Loading.BlobsProperty); }
            set { SetValue(Loading.BlobsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of each blob in the control
        /// </summary>
        public double BlobSize
        {
            get { return (double)GetValue(Loading.BlobSizeProperty); }
            set { SetValue(Loading.BlobSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin of each individual blob in the control
        /// </summary>
        public Thickness BlobMargin
        {
            get { return (Thickness)GetValue(Loading.BlobMarginProperty); }
            set { SetValue(Loading.BlobMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation interval of the control
        /// </summary>
        public TimeSpan Interval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Loading class
        /// </summary>
        public Loading()
        {
            InitializeComponent();

            Value = 0;
            
            timer.IsEnabled = true;
            timer.Tick += new EventHandler(timer_Tick);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Starts the animation
        /// </summary>
        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        #endregion

        #region Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            Value++;

            if (Value == Blobs)
                Value = 0;
        }

        #endregion
    }
}

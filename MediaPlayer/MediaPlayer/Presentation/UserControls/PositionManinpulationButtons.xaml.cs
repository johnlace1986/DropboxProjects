using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayer.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for PositionManinpulationButtons.xaml
    /// </summary>
    public partial class PositionManinpulationButtons : UserControl
    {
        #region Events

        /// <summary>
        /// Fires when the Up button is clicked
        /// </summary>
        public event RoutedEventHandler UpClicked;

        /// <summary>
        /// Fires when the Add button is clicked
        /// </summary>
        public event RoutedEventHandler AddClicked;

        /// <summary>
        /// Fires when the Delete button is clicked
        /// </summary>
        public event RoutedEventHandler DeleteClicked;

        /// <summary>
        /// Fires when the down button is clicked
        /// </summary>
        public event RoutedEventHandler DownClicked;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PositionManinpulationButtons), new PropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth", typeof(double), typeof(PositionManinpulationButtons), new PropertyMetadata(Button.WidthProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register("ButtonHeight", typeof(double), typeof(PositionManinpulationButtons), new PropertyMetadata(Button.HeightProperty.DefaultMetadata.DefaultValue));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the orientation of the buttons
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the buttons
        /// </summary>
        public double ButtonWidth
        {
            get { return (double)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the buttons
        /// </summary>
        public double ButtonHeight
        {
            get { return (double)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the PositionManinpulationButtons class
        /// </summary>
        public PositionManinpulationButtons()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the UpClicked event
        /// </summary>
        /// <param name="e">Agruments to pass to the event</param>
        private void OnUpClicked(RoutedEventArgs e)
        {
            if (UpClicked != null)
                UpClicked(this, e);
        }

        /// <summary>
        /// Fires the AddClicked event
        /// </summary>
        /// <param name="e">Agruments to pass to the event</param>
        private void OnAddClicked(RoutedEventArgs e)
        {
            if (AddClicked != null)
                AddClicked(this, e);
        }

        /// <summary>
        /// Fires the DeleteClicked event
        /// </summary>
        /// <param name="e">Agruments to pass to the event</param>
        private void OnDeleteClicked(RoutedEventArgs e)
        {
            if (DeleteClicked != null)
                DeleteClicked(this, e);
        }

        /// <summary>
        /// Fires the DownClicked event
        /// </summary>
        /// <param name="e">Agruments to pass to the event</param>
        private void OnDownClicked(RoutedEventArgs e)
        {
            if (DownClicked != null)
                DownClicked(this, e);
        }

        #endregion

        #region Event Handlers

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            OnUpClicked(e);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OnAddClicked(e);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            OnDeleteClicked(e);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            OnDownClicked(e);
        }

        #endregion
    }
}

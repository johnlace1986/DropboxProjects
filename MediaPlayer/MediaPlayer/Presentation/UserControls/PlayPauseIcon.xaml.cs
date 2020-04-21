using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business;

namespace MediaPlayer.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for PlayPauseIcon.xaml
    /// </summary>
    public partial class PlayPauseIcon : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty PlayStateProperty = DependencyProperty.Register("PlayState", typeof(PlayStateEnum), typeof(PlayPauseIcon), new PropertyMetadata(PlayStateEnum.Stopped));

        public static readonly DependencyProperty VisibileWhenStoppedProperty = DependencyProperty.Register("VisibileWhenStopped", typeof(Boolean), typeof(PlayPauseIcon), new PropertyMetadata(false));

        public static readonly DependencyProperty ShowActionIconProperty = DependencyProperty.Register("ShowActionIcon", typeof(Boolean), typeof(PlayPauseIcon), new PropertyMetadata(false));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the play state of the icon
        /// </summary>
        public PlayStateEnum PlayState
        {
            get { return (PlayStateEnum)GetValue(PlayStateProperty); }
            set { SetValue(PlayStateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the icon is visible when the play state is set to stopped
        /// </summary>
        public Boolean VisibileWhenStopped
        {
            get { return (Boolean)GetValue(VisibileWhenStoppedProperty); }
            set { SetValue(VisibileWhenStoppedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the icon displayed should represent the action that would be performed if the icon was clicked rather than the current play state
        /// </summary>
        public Boolean ShowActionIcon
        {
            get { return (Boolean)GetAnimationBaseValue(ShowActionIconProperty); }
            set { SetValue(ShowActionIconProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the PlayPauseIcon class
        /// </summary>
        public PlayPauseIcon()
        {
            InitializeComponent();
        }

        #endregion
    }
}

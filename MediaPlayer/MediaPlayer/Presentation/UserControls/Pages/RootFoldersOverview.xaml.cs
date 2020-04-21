using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.EventArgs;
using MediaPlayer.Library.Business;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for RootFoldersOverview.xaml
    /// </summary>
    public partial class RootFoldersOverview : UserControl
    {
        #region Events

        /// <summary>
        /// Fires when the path to a saved root folder changes
        /// </summary>
        public event RootFolderPathChangedEventHandler PathChanged;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty VideoRootFoldersProperty = DependencyProperty.Register("VideoRootFolders", typeof(RootFolder[]), typeof(RootFoldersOverview));

        public static readonly DependencyProperty SongRootFoldersProperty = DependencyProperty.Register("SongRootFolders", typeof(RootFolder[]), typeof(RootFoldersOverview));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(RootFoldersOverview));

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(Boolean?), typeof(RootFoldersOverview));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the root folders associated with videos currently in the system
        /// </summary>
        public RootFolder[] VideoRootFolders
        {
            get { return GetValue(RootFoldersOverview.VideoRootFoldersProperty) as RootFolder[]; }
            set { SetValue(RootFoldersOverview.VideoRootFoldersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root folders associated with songs currently in the system
        /// </summary>
        public RootFolder[] SongRootFolders
        {
            get { return GetValue(RootFoldersOverview.SongRootFoldersProperty) as RootFolder[]; }
            set { SetValue(RootFoldersOverview.SongRootFoldersProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the library is currently being organised
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(RootFoldersOverview.IsOrganisingProperty); }
            set { SetValue(RootFoldersOverview.IsOrganisingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value determining whether or not hidden media items should be displayed
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return (Boolean?)GetValue(ShowHiddenProperty); }
            set { SetValue(ShowHiddenProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RootFoldersOverview class
        /// </summary>
        public RootFoldersOverview()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the PathChanged event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnPathChanged(RootFolderPathChangedEventArgs e)
        {
            if (PathChanged != null)
                PathChanged(this, e);
        }

        #endregion

        #region Event Handlers

        private void rootFoldersView_PathChanged(object sender, RootFolderPathChangedEventArgs e)
        {
            OnPathChanged(e);
        }

        #endregion
    }
}

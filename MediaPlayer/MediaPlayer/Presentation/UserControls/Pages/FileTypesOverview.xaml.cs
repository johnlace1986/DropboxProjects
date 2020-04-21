using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Library.Business;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for FileTypes.xaml
    /// </summary>
    public partial class FileTypesOverview : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty VideoFileTypesProperty = DependencyProperty.Register("VideoFileTypes", typeof(FileType[]), typeof(FileTypesOverview));

        public static readonly DependencyProperty SongFileTypesProperty = DependencyProperty.Register("SongFileTypes", typeof(FileType[]), typeof(FileTypesOverview));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the file types associated with videos currently in the system
        /// </summary>
        public FileType[] VideoFileTypes
        {
            get { return GetValue(VideoFileTypesProperty) as FileType[]; }
            set { SetValue(VideoFileTypesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the file types associated with songs currently in the system
        /// </summary>
        public FileType[] SongFileTypes
        {
            get { return GetValue(SongFileTypesProperty) as FileType[]; }
            set { SetValue(SongFileTypesProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileTypesOverview class
        /// </summary>
        public FileTypesOverview()
        {
            InitializeComponent();
        }

        #endregion
    }
}

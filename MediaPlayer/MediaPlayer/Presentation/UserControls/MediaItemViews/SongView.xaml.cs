using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.UserControls.MediaItemViews
{
    /// <summary>
    /// Interaction logic for SongView.xaml
    /// </summary>
    public partial class SongView : UserControl, IMediaItemView
    {
        #region Dependency Properties

        public static readonly DependencyProperty SelectedSongProperty = DependencyProperty.Register("SelectedSong", typeof(Song), typeof(SongView));

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(Boolean?), typeof(SongView));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets a value determining whether or not all text within a control should be selected when the control gets focus
        /// </summary>
        private Boolean selectAllOnTextChanged = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the song currently displayed in the view
        /// </summary>
        public Song SelectedSong
        {
            get { return GetValue(SongView.SelectedSongProperty) as Song; }
            set { SetValue(SongView.SelectedSongProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not hidden songs show be displayed
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return GetValue(ShowHiddenProperty) as Boolean?; }
            set { SetValue(ShowHiddenProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SongView class
        /// </summary>
        public SongView()
        {
            InitializeComponent();

            SelectedElement = txtName;
        }

        #endregion

        #region Event Handlers

        private void element_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is DependencyObject)
            {
                TextBox txt;

                if (sender is TextBox)
                    txt = sender as TextBox;
                else
                    txt = VisualTreeHelpers.FindChild<TextBox>(sender as DependencyObject);

                if (txt != null)
                {
                    txt.TextChanged -= txt_TextChanged;
                    txt.TextChanged += txt_TextChanged;

                    txt.SelectionStart = 0;
                    txt.SelectionLength = txt.Text.Length;

                    if (txt.SelectionLength != 0)
                        selectAllOnTextChanged = true;
                }
            }

            SelectedElement = sender as UIElement;
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (txt.IsFocused)
            {
                if (selectAllOnTextChanged)
                {
                    txt.SelectionStart = 0;
                    txt.SelectionLength = txt.Text.Length;
                }

                selectAllOnTextChanged = false;
            }
        }

        #endregion

        #region IMediaItemView Members

        /// <summary>
        /// Gets or sets the currently selected element
        /// </summary>
        public UIElement SelectedElement { get; set; }

        /// <summary>
        /// Copies the properties from the source media item to the target media item
        /// </summary>
        /// <param name="target">Target media item who is copying the properties from the source media item</param>
        public void CopyPropertiesFromClone(MediaItem target)
        {
            Song targetSong = target as Song;

            targetSong.Name = SelectedSong.Name;
            targetSong.Artist = SelectedSong.Artist;
            targetSong.Album = SelectedSong.Album;
            targetSong.TrackNumber = SelectedSong.TrackNumber;
            targetSong.NumberOfTracks = SelectedSong.NumberOfTracks;
            targetSong.DiskNumber = SelectedSong.DiskNumber;
            targetSong.NumberOfDisks = SelectedSong.NumberOfDisks;
            targetSong.Year = SelectedSong.Year;
        }

        /// <summary>
        /// Parses the specified filename
        /// </summary>
        /// <param name="filename">Filename to parse</param>
        public void ParseFilename(String filename)
        {
            SelectedSong.ParseFilename(filename);
        }

        #endregion
    }
}

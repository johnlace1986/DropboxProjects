using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MediaPlayer.Business;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.UserControls.MediaItemViews
{
    /// <summary>
    /// Interaction logic for VideoView.xaml
    /// </summary>
    public partial class VideoView : UserControl, IMediaItemView
    {
        #region Dependency Properties

        public static readonly DependencyProperty SelectedVideoProperty = DependencyProperty.Register("SelectedVideo", typeof(Video), typeof(VideoView));

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(Boolean?), typeof(VideoView));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets a value determining whether or not all text within a control should be selected when the control gets focus
        /// </summary>
        private Boolean selectAllOnTextChanged = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the video currently displayed in the view
        /// </summary>
        public Video SelectedVideo
        {
            get { return GetValue(VideoView.SelectedVideoProperty) as Video; }
            set { SetValue(VideoView.SelectedVideoProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not hidden videos show be displayed
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return GetValue(ShowHiddenProperty) as Boolean?; }
            set { SetValue(ShowHiddenProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the VideoView class
        /// </summary>
        public VideoView()
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
            Video targetVideo = target as Video;

            targetVideo.Name = SelectedVideo.Name;
            targetVideo.Program = SelectedVideo.Program;
            targetVideo.Episode = SelectedVideo.Episode;
            targetVideo.NumberOfEpisodes = SelectedVideo.NumberOfEpisodes;
            targetVideo.Series = SelectedVideo.Series;
        }

        /// <summary>
        /// Parses the specified filename
        /// </summary>
        /// <param name="filename">Filename to parse</param>
        public void ParseFilename(String filename)
        {
            SelectedVideo.ParseFilename(filename);
        }

        #endregion
    }
}

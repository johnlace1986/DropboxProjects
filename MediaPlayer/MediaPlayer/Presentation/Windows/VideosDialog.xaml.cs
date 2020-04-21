using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for VideosDialog.xaml
    /// </summary>
    public partial class VideosDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty VideosProperty = DependencyProperty.Register("Videos", typeof(Video[]), typeof(VideosDialog));

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(Boolean?), typeof(VideosDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets the videos being editted in the window
        /// </summary>
        public Video[] Videos
        {
            get { return GetValue(VideosProperty) as Video[]; }
            set { SetValue(VideosProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not hidden videos should be shown
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return GetValue(ShowHiddenProperty) as Boolean?; }
            set { SetValue(ShowHiddenProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the VideosDialog class
        /// </summary>
        /// <param name="videos">Videos being editted in the window</param>
        /// <param name="showHidden">Value determining whether or not hidden videos should be shown</param>
        public VideosDialog(Video[] videos, Boolean? showHidden)
        {
            InitializeComponent();

            Videos = videos;
            ShowHidden = showHidden;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Loads the videos into the window
        /// </summary>
        private void LoadVideos()
        {
            try
            {
                Video firstVideo = Videos[0];

                Boolean sameProgram = true;
                Boolean sameGenre = true;
                Boolean sameEpisode = true;
                Boolean sameNumberOfEpisodes = true;
                Boolean sameSeries = true;
                Boolean? hidden = firstVideo.IsHidden;

                for (int i = 1; i < Videos.Length; i++)
                {
                    if (Videos[i].Program != firstVideo.Program)
                        sameProgram = false;

                    if (Videos[i].Genre != firstVideo.Genre)
                        sameGenre = false;

                    if (Videos[i].Episode != firstVideo.Episode)
                        sameEpisode = false;

                    if (Videos[i].NumberOfEpisodes != firstVideo.NumberOfEpisodes)
                        sameNumberOfEpisodes = false;

                    if (Videos[i].Series != firstVideo.Series)
                        sameSeries = false;

                    if (Videos[i].IsHidden != firstVideo.IsHidden)
                        hidden = null;
                }

                if (sameProgram)
                {
                    if (!IntelligentString.IsNullOrEmpty(firstVideo.Program.Value))
                    {
                        cmbProgram.Text = firstVideo.Program.Value;
                        chkProgram.IsChecked = false;
                    }
                }

                if (sameGenre)
                {
                    if (!IntelligentString.IsNullOrEmpty(firstVideo.Genre.Value))
                    {
                        cmbGenre.Text = firstVideo.Genre.Value;
                        chkGenre.IsChecked = false;
                    }
                }

                if (sameEpisode)
                {
                    if (firstVideo.Episode != 0)
                    {
                        iudEpisode.Text = firstVideo.Episode.ToString();
                        chkEpisode.IsChecked = false;
                    }
                }

                if (sameNumberOfEpisodes)
                {
                    if (firstVideo.NumberOfEpisodes != 0)
                    {
                        iudNumberOfEpisodes.Text = firstVideo.NumberOfEpisodes.ToString();
                        chkNumberOfEpisodes.IsChecked = false;
                    }
                }

                if (sameSeries)
                {
                    if (firstVideo.Series != 0)
                    {
                        iudSeries.Text = firstVideo.Series.ToString();
                        chkSeries.IsChecked = false;
                    }
                }

                if (hidden.HasValue)
                {
                    if (hidden.Value)
                        cmbIsHidden.Text = "Yes";
                    else
                        cmbIsHidden.Text = "No";

                    chkIsHidden.IsChecked = false;
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not load videos: ");
            }
        }

        /// <summary>
        /// Sets the specified property to values to all videos in the collection and saves the changes to the database
        /// </summary>
        /// <returns>True if the changes were successfully saved, false if not</returns>
        public Boolean SaveVideos()
        {
            try
            {
                foreach (Video video in Videos)
                {
                    if (GeneralMethods.GetNullableBoolValue(chkProgram.IsChecked))
                        video.Program = cmbProgram.Text;

                    if (GeneralMethods.GetNullableBoolValue(chkGenre.IsChecked))
                        video.Genre = cmbGenre.Text;

                    if (GeneralMethods.GetNullableBoolValue(chkEpisode.IsChecked))
                        video.Episode = Convert.ToInt16(iudEpisode.Value.HasValue ? iudEpisode.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkNumberOfEpisodes.IsChecked))
                        video.NumberOfEpisodes = Convert.ToInt16(iudNumberOfEpisodes.Value.HasValue ? iudNumberOfEpisodes.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkSeries.IsChecked))
                        video.Series = Convert.ToInt16(iudSeries.Value.HasValue ? iudSeries.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkIsHidden.IsChecked))
                        video.IsHidden = (cmbIsHidden.SelectedIndex == 0 ? true : false);
                }

                return true;
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not save videos: ");
            }

            return false;
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadVideos();
        }

        private void cmbProgram_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkProgram.IsChecked = true;
        }

        private void cmbGenre_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkGenre.IsChecked = true;
        }

        private void iudEpisode_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkEpisode.IsChecked = true;
        }

        private void iudNumberOfEpisodes_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkNumberOfEpisodes.IsChecked = true;
        }

        private void iudSeries_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkSeries.IsChecked = true;
        }

        private void cmbIsHidden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chkIsHidden.IsChecked = true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (SaveVideos())
                DialogResult = true;
        }

        #endregion
    }
}

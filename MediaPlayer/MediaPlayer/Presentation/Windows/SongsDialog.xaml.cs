using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for SongsDialog.xaml
    /// </summary>
    public partial class SongsDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty SongsProperty = DependencyProperty.Register("Songs", typeof(Song[]), typeof(SongsDialog));

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(Boolean?), typeof(SongsDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets the songs being editted in the window
        /// </summary>
        public Song[] Songs
        {
            get { return GetValue(SongsProperty) as Song[]; }
            set { SetValue(SongsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not hidden songs should be shown
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return GetValue(ShowHiddenProperty) as Boolean?; }
            set { SetValue(ShowHiddenProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SongsDialog class
        /// </summary>
        /// <param name="songs">Songs being editted in the window</param>
        /// <param name="showHidden">Value determining whether or not hidden songs should be shown</param>
        public SongsDialog(Song[] songs, Boolean? showHidden)
        {
            InitializeComponent();

            Songs = songs;
            ShowHidden = showHidden;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Loads the songs into the window
        /// </summary>
        private void LoadSongs()
        {
            try
            {
                Song firstSong = Songs[0];

                Boolean sameArtist = true;
                Boolean sameAlbum = true;
                Boolean sameGenre = true;
                Boolean sameTrack = true;
                Boolean sameNumberOfTracks = true;
                Boolean sameDisk = true;
                Boolean sameNumberOfDisks = true;
                Boolean sameYear = true;
                Boolean? hidden = firstSong.IsHidden;

                for (int i = 1; i < Songs.Length; i++)
                {
                    if (Songs[i].Artist != firstSong.Artist)
                        sameArtist = false;

                    if (Songs[i].Album != firstSong.Album)
                        sameAlbum = false;

                    if (Songs[i].Genre != firstSong.Genre)
                        sameGenre = false;

                    if (Songs[i].TrackNumber != firstSong.TrackNumber)
                        sameTrack = false;

                    if (Songs[i].NumberOfTracks != firstSong.NumberOfTracks)
                        sameNumberOfTracks = false;

                    if (Songs[i].DiskNumber != firstSong.DiskNumber)
                        sameDisk = false;

                    if (Songs[i].NumberOfDisks != firstSong.NumberOfDisks)
                        sameNumberOfDisks = false;

                    if (Songs[i].Year != firstSong.Year)
                        sameYear = false;

                    if (Songs[i].IsHidden != firstSong.IsHidden)
                        hidden = null;
                }

                if (sameArtist)
                {
                    if (!IntelligentString.IsNullOrEmpty(firstSong.Artist.Value))
                    {
                        cmbArtist.Text = firstSong.Artist.Value;
                        chkArtist.IsChecked = false;
                    }
                }

                if (sameAlbum)
                {
                    if (!IntelligentString.IsNullOrEmpty(firstSong.Album.Value))
                    {
                        cmbAlbum.Text = firstSong.Album.Value;
                        chkAlbum.IsChecked = false;
                    }
                }

                if (sameGenre)
                {
                    if (!IntelligentString.IsNullOrEmpty(firstSong.Genre.Value))
                    {
                        cmbGenre.Text = firstSong.Genre.Value;
                        chkGenre.IsChecked = false;
                    }
                }

                if (sameTrack)
                {
                    if (firstSong.TrackNumber != 0)
                    {
                        iudTrack.Text = firstSong.TrackNumber.ToString();
                        chkTrack.IsChecked = false;
                    }
                }

                if (sameNumberOfTracks)
                {
                    if (firstSong.NumberOfTracks != 0)
                    {
                        iudNumberOfTracks.Text = firstSong.NumberOfTracks.ToString();
                        chkNumberOfTracks.IsChecked = false;
                    }
                }

                if (sameDisk)
                {
                    if (firstSong.DiskNumber != 0)
                    {
                        iudDisk.Text = firstSong.DiskNumber.ToString();
                        chkDisk.IsChecked = false;
                    }
                }

                if (sameNumberOfDisks)
                {
                    if (firstSong.NumberOfDisks != 0)
                    {
                        iudNumberOfDisks.Text = firstSong.NumberOfDisks.ToString();
                        chkNumberOfDisks.IsChecked = false;
                    }
                }

                if (sameYear)
                {
                    if (firstSong.Year != 0)
                    {
                        iudYear.Text = firstSong.Year.ToString();
                        chkYear.IsChecked = true;
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
                GeneralMethods.MessageBoxException(e, "Could not load songs: ");
            }
        }

        /// <summary>
        /// Sets the specified property to values to all songs in the collection and saves the changes to the database
        /// </summary>
        /// <returns>True if the changes were successfully saved, false if not</returns>
        public Boolean SaveSongs()
        {
            try
            {
                foreach (Song song in Songs)
                {
                    if (GeneralMethods.GetNullableBoolValue(chkArtist.IsChecked))
                        song.Artist = cmbArtist.Text;

                    if (GeneralMethods.GetNullableBoolValue(chkAlbum.IsChecked))
                        song.Album = cmbAlbum.Text;

                    if (GeneralMethods.GetNullableBoolValue(chkGenre.IsChecked))
                        song.Genre = cmbGenre.Text;

                    if (GeneralMethods.GetNullableBoolValue(chkTrack.IsChecked))
                        song.TrackNumber = Convert.ToInt16(iudTrack.Value.HasValue ? iudTrack.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkNumberOfTracks.IsChecked))
                        song.NumberOfTracks = Convert.ToInt16(iudNumberOfTracks.Value.HasValue ? iudNumberOfTracks.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkDisk.IsChecked))
                        song.DiskNumber = Convert.ToInt16(iudDisk.Value.HasValue ? iudDisk.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkNumberOfDisks.IsChecked))
                        song.NumberOfDisks = Convert.ToInt16(iudNumberOfDisks.Value.HasValue ? iudNumberOfDisks.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkYear.IsChecked))
                        song.Year = Convert.ToInt16(iudYear.Value.HasValue ? iudYear.Value.Value : 0);

                    if (GeneralMethods.GetNullableBoolValue(chkIsHidden.IsChecked))
                        song.IsHidden = (cmbIsHidden.SelectedIndex == 0 ? true : false);
                }

                return true;
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not save songs: ");
            }

            return false;
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSongs();
        }

        private void cmbArtist_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkArtist.IsChecked = true;
        }

        private void cmbAlbum_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkAlbum.IsChecked = true;
        }

        private void cmbGenre_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkGenre.IsChecked = true;
        }

        private void iudTrack_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkTrack.IsChecked = true;
        }

        private void iudNumberOfTracks_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkNumberOfTracks.IsChecked = true;
        }

        private void iudDisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkDisk.IsChecked = true;
        }

        private void iudNumberOfDisks_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkNumberOfDisks.IsChecked = true;
        }

        private void iudYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkYear.IsChecked = true;
        }

        private void cmbIsHidden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chkIsHidden.IsChecked = true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (SaveSongs())
                DialogResult = true;
        }

        #endregion
    }
}

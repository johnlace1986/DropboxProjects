using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MediaPlayer.Library.Business;
using System.Collections.ObjectModel;
using MediaPlayer.Business;
using Utilities.Business;
using MediaPlayer.Presentation.Windows;
using MediaPlayer.Presentation.UserControls.ControlExtenders;
using MediaPlayer.EventArgs;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for OrganisingLibraryView.xaml
    /// </summary>
    public partial class OrganisingLibraryView : UserControl
    {
        #region Events

        /// <summary>
        /// Fires prior to a media item organising
        /// </summary>
        public event CancelMediaItemsOperationEventHandler OrganisingMediaItem;

        /// <summary>
        /// Fires when the organiser finishs organising
        /// </summary>
        public event EventHandler FinishedOrganising;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(OrganisingLibraryView));

        public static readonly DependencyProperty OrganiseHiddenMediaItemsProperty = DependencyProperty.Register("OrganiseHiddenMediaItems", typeof(Boolean?), typeof(OrganisingLibraryView), new PropertyMetadata(null));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(OrganisingLibraryView));

        public static readonly DependencyProperty VideosProperty = DependencyProperty.Register("Videos", typeof(ObservableCollection<Video>), typeof(OrganisingLibraryView));

        public static readonly DependencyProperty VideoRootFoldersProperty = DependencyProperty.Register("VideoRootFolders", typeof(RootFolder[]), typeof(OrganisingLibraryView));

        public static readonly DependencyProperty SongsProperty = DependencyProperty.Register("Songs", typeof(ObservableCollection<Song>), typeof(OrganisingLibraryView));

        public static readonly DependencyProperty SongRootFoldersProperty = DependencyProperty.Register("SongRootFolders", typeof(RootFolder[]), typeof(OrganisingLibraryView));

        public static readonly DependencyProperty LibraryOrganiserProperty = DependencyProperty.Register("LibraryOrganiser", typeof(LibraryOrganiser), typeof(OrganisingLibraryView));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the options currently saved in the database
        /// </summary>
        public Business.Options Options
        {
            get { return GetValue(OrganisingLibraryView.OptionsProperty) as Business.Options; }
            set { SetValue(OrganisingLibraryView.OptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not hidden videos should be organised
        /// </summary>
        public Boolean? OrganiseHiddenMediaItems
        {
            get { return (Boolean?)GetValue(OrganiseHiddenMediaItemsProperty); }
            set { SetValue(OrganiseHiddenMediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the system is currently organising media items
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(OrganisingLibraryView.IsOrganisingProperty); }
            set { SetValue(OrganisingLibraryView.IsOrganisingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the videos in the system
        /// </summary>
        public ObservableCollection<Video> Videos
        {
            get { return GetValue(OrganisingLibraryView.VideosProperty) as ObservableCollection<Video>; }
            set { SetValue(OrganisingLibraryView.VideosProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root folders associated with videos currently in the system
        /// </summary>
        public RootFolder[] VideoRootFolders
        {
            get { return GetValue(OrganisingLibraryView.VideoRootFoldersProperty) as RootFolder[]; }
            set { SetValue(OrganisingLibraryView.VideoRootFoldersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the songs in the system
        /// </summary>
        public ObservableCollection<Song> Songs
        {
            get { return GetValue(OrganisingLibraryView.SongsProperty) as ObservableCollection<Song>; }
            set { SetValue(OrganisingLibraryView.SongsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root folders associated with songs currently in the system
        /// </summary>
        public RootFolder[] SongRootFolders
        {
            get { return GetValue(OrganisingLibraryView.SongRootFoldersProperty) as RootFolder[]; }
            set { SetValue(OrganisingLibraryView.SongRootFoldersProperty, value); }
        }

        /// <summary>
        /// Gets the items currently scheduled to be organised
        /// </summary>
        public OrganisingMediaItemPart[] PartsToOrganise
        {
            get { return dgPartsToOrganise.ItemsSource as OrganisingMediaItemPart[]; }
        }

        /// <summary>
        /// Gets or sets the organiser used to organise the library
        /// </summary>
        public LibraryOrganiser LibraryOrganiser
        {
            get { return GetValue(OrganisingLibraryView.LibraryOrganiserProperty) as LibraryOrganiser; }
            private set { SetValue(OrganisingLibraryView.LibraryOrganiserProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OrganisingLibraryView class
        /// </summary>
        public OrganisingLibraryView()
        {
            InitializeComponent();

            IsOrganising = false;
            LibraryOrganiser = null;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the OrganisingMediaItem event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnOrganisingMediaItem(CancelMediaItemsOperationEventArgs e)
        {
            if (OrganisingMediaItem != null)
                OrganisingMediaItem(this, e);
        }

        /// <summary>
        /// Fires the FinishedOrganising event
        /// </summary>
        private void OnFinishedOrganising()
        {
            if (FinishedOrganising != null)
                GeneralMethods.ExecuteDelegateOnGuiThread(Dispatcher, () => FinishedOrganising(this, new System.EventArgs()));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (PartsToOrganise.Length > 0)
            {
                IsOrganising = true;

                List<OrganisingRootFolder> rootFolders = new List<OrganisingRootFolder>();

                foreach (RootFolder videoRootFolder in VideoRootFolders)
                    rootFolders.Add(new OrganisingRootFolder(videoRootFolder));

                foreach (RootFolder songRootFolder in SongRootFolders)
                    rootFolders.Add(new OrganisingRootFolder(songRootFolder));

                LibraryOrganiser = new LibraryOrganiser(PartsToOrganise, Dispatcher);
                LibraryOrganiser.OrganisingMediaItem += new EventArgs.CancelMediaItemsOperationEventHandler(LibraryOrganiser_OrganisingMediaItem);
                LibraryOrganiser.FinishedOrganising += new EventHandler(LibraryOrganiser_FinishedOrganising);
                LibraryOrganiser.OrganiseAsync(rootFolders.ToArray());
            }
        }

        /// <summary>
        /// Checks to see if the library is now organised
        /// </summary>
        /// <returns>True if the library is organised, false if not</returns>
        private Boolean CheckOrganised()
        {
            CleanUp();

            OrganisingMediaItemPart[] remaining = PartsToOrganise.Where(p => 
                p.Status == OrganisingMediaItemPartStatus.Waiting ||
                p.Status == OrganisingMediaItemPartStatus.Error ||
                (p.Status == OrganisingMediaItemPartStatus.Organised && p.ErrorCount > 0)
                ).ToArray();

            if (remaining.Length == 0)
            {
                RefreshItemBinding();

                IsOrganising = false;
                OnFinishedOrganising();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Refreshes the bindings to the data grid item source
        /// </summary>
        private void RefreshItemBinding()
        {
            MultiBindingExpression mbe = BindingOperations.GetMultiBindingExpression(dgPartsToOrganise, DataGrid.ItemsSourceProperty);
            mbe.UpdateTarget();
        }

        /// <summary>
        /// Cleans up the root folders
        /// </summary>
        public void CleanUp()
        {
            List<NonLibraryFile> filenames = new List<NonLibraryFile>();

            foreach (RootFolder rootFolder in VideoRootFolders)
                foreach (IntelligentString filename in rootFolder.CleanUp(Options.RootFolderFileExceptions.ToArray(), Options.RootFolderDirectoryExceptions.ToArray()))
                    filenames.Add(NonLibraryFile.FromFilename(rootFolder, filename));

            foreach (RootFolder rootFolder in SongRootFolders)
                foreach (IntelligentString filename in rootFolder.CleanUp(Options.RootFolderFileExceptions.ToArray(), Options.RootFolderDirectoryExceptions.ToArray()))
                    filenames.Add(NonLibraryFile.FromFilename(rootFolder, filename));

            if (filenames.Count > 0)
            {
                NonLibraryFilesDialog nlfd = new NonLibraryFilesDialog(filenames.ToArray(), Options);
                nlfd.Owner = Application.Current.MainWindow;
                nlfd.ShowDialog();
            }
        }

        #endregion

        #region Event Handlers

        private void LibraryOrganiser_OrganisingMediaItem(object sender, EventArgs.CancelMediaItemsOperationEventArgs e)
        {
            OnOrganisingMediaItem(e);
        }

        private void LibraryOrganiser_FinishedOrganising(object sender, System.EventArgs e)
        {
            CheckOrganised();
        }

        private void OrganisingProgressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OrganisingProgressBar organisingProgressBar = sender as OrganisingProgressBar;

            if ((organisingProgressBar.Status == OrganisingMediaItemPartStatus.Error) ||
                ((organisingProgressBar.Status == OrganisingMediaItemPartStatus.Organised) && (organisingProgressBar.ErrorCount > 0)))
            {
                String message;

                if (organisingProgressBar.ErrorCount == 1)
                    message = "Error occurred while organising:" + Environment.NewLine + Environment.NewLine;
                else
                    message = "Errors occurred while organising:" + Environment.NewLine + Environment.NewLine;

                foreach (KeyValuePair<IntelligentString, System.Exception> error in organisingProgressBar.Errors)
                {
                    if (error.Key != null)
                        message += error.Key.Value + ": ";

                    message += error.Value.Message + Environment.NewLine;
                }

                GeneralMethods.MessageBoxApplicationError(message.Trim());
            }
        }

        private void btnOrganise_Click(object sender, RoutedEventArgs e)
        {
            if (LibraryOrganiser.IsOrganising)
                LibraryOrganiser.StopOrganising();
            else
            {
                RefreshItemBinding();

                if (!CheckOrganised())
                    Start();
            }
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            IsOrganising = false;
            OnFinishedOrganising();
        }

        #endregion
    }
}

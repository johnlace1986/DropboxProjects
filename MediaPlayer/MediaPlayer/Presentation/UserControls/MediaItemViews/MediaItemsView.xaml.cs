using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MediaPlayer.Library.Business;
using Utilities.Exception;
using MediaPlayer.Presentation.Windows;
using MediaPlayer.EventArgs;
using MediaPlayer.Business;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Diagnostics;
using Utilities.Business;
using System.ComponentModel;

namespace MediaPlayer.Presentation.UserControls.MediaItemViews
{
    /// <summary>
    /// Interaction logic for MediaItemsView.xaml
    /// </summary>
    public partial class MediaItemsView : UserControl
    {
        #region Events

        /// <summary>
        /// Fires when the user selects to play one or more media items
        /// </summary>
        public event PlayMediaItemsEventHandler PlayMediaItems;

        /// <summary>
        /// Fires when one or more videos are added to the "Now Playing..." playlist
        /// </summary>
        public event MediaItemsEventHandler AddedToNowPlaying;

        /// <summary>
        /// Fires when a new file type is added to the system
        /// </summary>
        public event FileTypeEventHandler FileTypeAdded;

        /// <summary>
        /// Fires when a media item is saved
        /// </summary>
        public event MediaItemEventHandler MediaItemSaved;

        /// <summary>
        /// Fires when multiple media media items are saved
        /// </summary>
        public event MediaItemsEventHandler MediaItemsSaved;

        /// <summary>
        /// Fires prior to one or more media items being deleted
        /// </summary>
        public event CancelMediaItemsOperationEventHandler MediaItemsDeleting;

        /// <summary>
        /// Fires when one or more media items are deleted
        /// </summary>
        public event MediaItemsEventHandler MediaItemsDeleted;

        /// <summary>
        /// Fires prior to two or more media items being merged
        /// </summary>
        public event CancelMediaItemsOperationEventHandler MergingSelectedMediaItems;

        /// <summary>
        /// Fires prior to a part of a media item being extracted to another media item
        /// </summary>
        public event CancelMediaItemsOperationEventHandler ExtractingPartFromMediaItem;

        /// <summary>
        /// Fires prior to a part being deleted from a media item
        /// </summary>
        public event CancelMediaItemsOperationEventHandler DeletingPart;
        
        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(ObservableCollection<DataGridColumn>), typeof(MediaItemsView));

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(ObservableCollection<MediaItem>), typeof(MediaItemsView), new PropertyMetadata(new PropertyChangedCallback(OnMediaItemsPropertyChanged)));

        public static readonly DependencyProperty AutoGenerateColumnsProperty = DependencyProperty.Register("AutoGenerateColumns", typeof(Boolean), typeof(MediaItemsView));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(MediaItemsView));

        public static readonly DependencyProperty SelectedMediaItemProperty = DependencyProperty.Register("SelectedMediaItem", typeof(MediaItem), typeof(MediaItemsView));

        public static readonly DependencyProperty AssociatedTypeProperty = DependencyProperty.Register("AssociatedType", typeof(MediaItemTypeEnum), typeof(MediaItemsView));

        public static readonly DependencyProperty RecycleBinPromptProperty = DependencyProperty.Register("RecycleBinPrompt", typeof(Boolean), typeof(MediaItemsView));

        public static readonly DependencyProperty AllowMergeExtractProperty = DependencyProperty.Register("AllowMergeExtract", typeof(Boolean), typeof(MediaItemsView), new PropertyMetadata(false));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(MediaItemsView));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(MediaItemFilter), typeof(MediaItemsView));

        public static readonly DependencyProperty ExportDirectoriesProperty = DependencyProperty.Register("ExportDirectories", typeof(ObservableCollection<String>), typeof(MediaItemsView));

        #endregion

        #region Fields

        private List<SortDescription> previousSortDescriptions = new List<SortDescription>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the columns to display in the view
        /// </summary>
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return dgMediaItems.Columns; }
            set
            {
                dgMediaItems.Columns.Clear();

                foreach (DataGridColumn column in value)
                    dgMediaItems.Columns.Add(column);

                SetValue(MediaItemsView.ColumnsProperty, dgMediaItems.Columns);
            }
        }

        /// <summary>
        /// Gets or sets the media items displayed in the view
        /// </summary>
        public ObservableCollection<MediaItem> MediaItems
        {
            get
            {
                //get the default view to maintain sorting
                ICollectionView cv = CollectionViewSource.GetDefaultView(dgMediaItems.ItemsSource as ObservableCollection<MediaItem>);
                return new ObservableCollection<MediaItem>(cv.Cast<MediaItem>());
            }
            set { SetValue(MediaItemsView.MediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the sort descriptions in the view
        /// </summary>
        public SortDescriptionCollection SortDescriptions
        {
            get { return dgMediaItems.Items.SortDescriptions; }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the columns in the view are generated automatically
        /// </summary>
        public Boolean AutoGenerateItems
        {
            get { return (Boolean)GetValue(MediaItemsView.AutoGenerateColumnsProperty); }
            set { SetValue(MediaItemsView.AutoGenerateColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index in the collection of the selected media item
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return (Int32)GetValue(MediaItemsView.SelectedIndexProperty); }
            set { SetValue(MediaItemsView.SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected media item
        /// </summary>
        public MediaItem SelectedMediaItem
        {
            get { return GetValue(MediaItemsView.SelectedMediaItemProperty) as MediaItem; }
            set { SetValue(MediaItemsView.SelectedMediaItemProperty, value); }
        }

        /// <summary>
        /// Gets the selected media items
        /// </summary>
        public MediaItem[] SelectedMediaItems
        {
            get
            {
                ICollectionView cv = CollectionViewSource.GetDefaultView(dgMediaItems.ItemsSource);                
                List<MediaItem> mediaItems = new List<MediaItem>(cv.Cast<MediaItem>());

                for (int i = 0; i < mediaItems.Count; i++)
                {
                    if (!dgMediaItems.SelectedItems.Contains(mediaItems[i]))
                    {
                        mediaItems.RemoveAt(i);
                        i--;
                    }
                }

                return mediaItems.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the type of media item the items in the view are associated with
        /// </summary>
        public MediaItemTypeEnum AssociatedType
        {
            get { return (MediaItemTypeEnum)GetValue(MediaItemsView.AssociatedTypeProperty); }
            set { SetValue(MediaItemsView.AssociatedTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the user is prompted to move parts to the recycle bin when media items are deleted
        /// </summary>
        public Boolean RecycleBinPrompt
        {
            get { return (Boolean)GetValue(MediaItemsView.RecycleBinPromptProperty); }
            set { SetValue(MediaItemsView.RecycleBinPromptProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not media item parts in the view can be merged or extracted
        /// </summary>
        public Boolean AllowMergeExtract
        {
            get { return (Boolean)GetValue(AllowMergeExtractProperty); }
            set { SetValue(AllowMergeExtractProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the library is currently being organised
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(MediaItemsView.IsOrganisingProperty); }
            set { SetValue(MediaItemsView.IsOrganisingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the play options are enabled in the view
        /// </summary>
        public Boolean EnablePlayOptions { get; set; }

        /// <summary>
        /// Gets or sets the filter used to filter media items
        /// </summary>
        public MediaItemFilter Filter
        {
            get { return GetValue(FilterProperty) as MediaItemFilter; }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the directories that will be listed as options for exporting media items to
        /// </summary>
        public ObservableCollection<String> ExportDirectories
        {
            get { return GetValue(ExportDirectoriesProperty) as ObservableCollection<String>; }
            set { SetValue(ExportDirectoriesProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemsView class
        /// </summary>
        public MediaItemsView()
        {
            AssociatedType = MediaItemTypeEnum.NotSet;

            InitializeComponent();

            Columns = new ObservableCollection<DataGridColumn>();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the PlayMediaItems event
        /// </summary>
        /// <param name="mediaItems">Media items that are to be played</param>
        /// <param name="selectedIndex">Index in the collection of the first media item to be played</param>
        private void OnPlayMediaItems(MediaItem[] mediaItems, Int32 selectedIndex)
        {
            if (PlayMediaItems != null)
                PlayMediaItems(this, new PlayMediaItemsEventArgs(mediaItems, selectedIndex));
        }

        /// <summary>
        /// Fires the AddedToNowPlaying event
        /// </summary>
        /// <param name="mediaItems">Media items that were added the the "Now Playing..." playlist</param>
        private void OnAddedToNowPlaying(MediaItem[] mediaItems)
        {
            if (AddedToNowPlaying != null)
                AddedToNowPlaying(this, new MediaItemsEventArgs(mediaItems));
        }

        /// <summary>
        /// Fires the FileTypeAdded event
        /// </summary>
        /// <param name="fileType">The file type that was added</param>
        private void OnFileTypeAdded(FileType fileType)
        {
            if (FileTypeAdded != null)
                FileTypeAdded(this, new FileTypeEventArgs(fileType));
        }

        /// <summary>
        /// Fires the MediaItemSaved event
        /// </summary>
        /// <param name="mediaItem">Media item that was saved</param>
        private void OnMediaItemSaved(MediaItem mediaItem)
        {
            if (MediaItemSaved != null)
                MediaItemSaved(this, new MediaItemEventArgs(mediaItem));
        }

        /// <summary>
        /// Fires the MediaItemsSaved event
        /// </summary>
        /// <param name="mediaItems">Media items that were saved</param>
        private void OnMediaItemsSaved(MediaItem[] mediaItems)
        {
            if (MediaItemsSaved != null)
                MediaItemsSaved(this, new MediaItemsEventArgs(mediaItems));
        }

        /// <summary>
        /// Fires the MediaItemsDeleting event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnMediaItemsDeleting(CancelMediaItemsOperationEventArgs e)
        {
            if (MediaItemsDeleting != null)
                MediaItemsDeleting(this, e);
        }

        /// <summary>
        /// Fires the MediaItemsDeleted event
        /// </summary>
        /// <param name="mediaItems">Media items that were deleted</param>
        private void OnMediaItemsDeleted(MediaItem[] mediaItems)
        {
            if (MediaItemsDeleted != null)
                MediaItemsDeleted(this, new CancelMediaItemsOperationEventArgs(mediaItems));
        }

        /// <summary>
        /// Fires the MergingSelectedMediaItems event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnMergingSelectedMediaItems(CancelMediaItemsOperationEventArgs e)
        {
            if (MergingSelectedMediaItems != null)
                MergingSelectedMediaItems(this, e);
        }

        /// <summary>
        /// Fires the ExtractingPartFromMediaItem event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnExtractingPartFromMediaItem(CancelMediaItemsOperationEventArgs e)
        {
            if (ExtractingPartFromMediaItem != null)
                ExtractingPartFromMediaItem(this, e);
        }

        /// <summary>
        /// Fires the DeletingPart event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnDeletingPart(CancelMediaItemsOperationEventArgs e)
        {
            if (DeletingPart != null)
                DeletingPart(this, e);
        }

        /// <summary>
        /// Plays the selected media items
        /// </summary>
        private void PlaySelectedMediaItems()
        {
            OnPlayMediaItems(SelectedMediaItems.ToArray(), 0);
        }

        /// <summary>
        /// Plays all media items currently dislayed in the view, starting with the selected media item
        /// </summary>
        private void PlayFromHere()
        {
            for (int i = 0; i < MediaItems.Count; i++)
            {
                if (MediaItems[i] == SelectedMediaItem)
                {
                    OnPlayMediaItems(MediaItems.ToArray(), i);
                    break;
                }
            }
        }

        /// <summary>
        /// Adds the selected media items to the "Now Playing..." playlist
        /// </summary>
        private void AddToNowPlaying()
        {
            OnAddedToNowPlaying(SelectedMediaItems.ToArray());
        }

        /// <summary>
        /// Opens explorer windows containing the selected media items
        /// </summary>
        private void ShowSelectedMediaItemsInExplorer()
        {
            List<String> directories = new List<String>();

            foreach (MediaItem mediaItem in SelectedMediaItems)
            {
                foreach (MediaItemPart part in mediaItem.Parts)
                {
                    FileInfo fi = new FileInfo(part.Location.Value);

                    if (File.Exists(fi.FullName))
                    {
                        if (!directories.Contains(fi.DirectoryName))
                        {
                            directories.Add(fi.DirectoryName);
                        }
                    }
                }
            }

            foreach (String directory in directories)
                Process.Start(directory);
        }

        /// <summary>
        /// Edits the selected videos
        /// </summary>
        private void EditSelectedMediaItems()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any media items");
                    return;
                }

                if (SelectedMediaItems.Length > 0)
                {
                    if (SelectedMediaItems.Length == 1)
                    {
                        MediaItemDialog mediaItemDialog = new MediaItemDialog(true, MediaItems.ToArray());
                        mediaItemDialog.FileTypeAdded += new FileTypeEventHandler(mediaItemDialog_FileTypeAdded);
                        mediaItemDialog.SavedMediaItem += new MediaItemEventHandler(mediaItemDialog_SavedMediaItem);
                        mediaItemDialog.PartExtracted += new MediaItemPartEventHandler(mediaItemDialog_PartExtracted);
                        mediaItemDialog.Owner = Application.Current.MainWindow;
                        mediaItemDialog.ShowHidden = Filter.ShowHidden;
                        mediaItemDialog.SelectedIndex = SelectedIndex;
                        mediaItemDialog.ShowDialog();
                    }
                    else
                    {
                        MediaItem firstItem = SelectedMediaItems[0];

                        if (SelectedMediaItems.Any(p => p.Type != firstItem.Type))
                        {
                            //generic media items
                            throw new NotImplementedException();
                        }
                        else
                        {
                            switch (firstItem.Type)
                            {
                                case MediaItemTypeEnum.Video:

                                    VideosDialog videosDialog = new VideosDialog(SelectedMediaItems.Cast<Video>().ToArray(), Filter.ShowHidden);
                                    videosDialog.Owner = Application.Current.MainWindow;

                                    if (GeneralMethods.GetNullableBoolValue(videosDialog.ShowDialog()))
                                        OnMediaItemsSaved(videosDialog.Videos);

                                    break;

                                case MediaItemTypeEnum.Song:

                                    SongsDialog songsDialog = new SongsDialog(SelectedMediaItems.Cast<Song>().ToArray(), Filter.ShowHidden);
                                    songsDialog.Owner = Application.Current.MainWindow;

                                    if (GeneralMethods.GetNullableBoolValue(songsDialog.ShowDialog()))
                                        OnMediaItemsSaved(songsDialog.Songs);

                                    break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not edit media items: ");
            }
        }

        /// <summary>
        /// Modifies the selected media items
        /// </summary>
        private void ModifySelectedMediaItems()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before modifying any media items");
                    return;
                }

                if (SelectedMediaItems.Length > 0)
                {
                    ModifyMediaItemsDialog mmid = new ModifyMediaItemsDialog(SelectedMediaItems.ToArray());
                    mmid.Owner = Application.Current.MainWindow;

                    if (GeneralMethods.GetNullableBoolValue(mmid.ShowDialog()))
                        OnMediaItemsSaved(SelectedMediaItems);
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not modify media items: ");
            }
        }

        /// <summary>
        /// Deletes the selected media items
        /// </summary>
        private void RemoveSelectedMediaItems()
        {
            RemoveMediaItems(SelectedMediaItems);
        }

        /// <summary>
        /// Deletes the media items
        /// </summary>
        /// <param name="mediaItem">Media item being deleted</param>
        public void RemoveMediaItems(MediaItem mediaItem)
        {
            RemoveMediaItems(new MediaItem[1] { mediaItem });
        }

        /// <summary>
        /// Deletes the media items
        /// </summary>
        /// <param name="mediaItems">Media items being deleted</param>
        public void RemoveMediaItems(MediaItem[] mediaItems)
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before deleting any media items");
                    return;
                }

                CancelMediaItemsOperationEventArgs e = new CancelMediaItemsOperationEventArgs(mediaItems);
                OnMediaItemsDeleting(e);

                if (e.Cancel)
                {
                    GeneralMethods.MessageBoxApplicationError(e.ReasonForCancel);
                    return;
                }

                MessageBoxResult result;

                switch (e.MediaItems.Count)
                {
                    case 0:
                        //do nothing
                        return;

                    case 1:
                        if (RecycleBinPrompt)
                            result = GeneralMethods.MessageBox("You are about to delete " + e.MediaItems[0].Name + ". Would you also like to move the physical files to the recycle bin?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        else
                            result = GeneralMethods.MessageBox("You are about to delete " + e.MediaItems[0].Name + ".", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        break;

                    default:

                        if (RecycleBinPrompt)
                            result = GeneralMethods.MessageBox("You are about to delete " + e.MediaItems.Count.ToString() + " items. Would you also like to move the physical files to the recycle bin?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        else
                            result = GeneralMethods.MessageBox("You are about to delete " + e.MediaItems.Count.ToString() + " items.", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        break;
                }

                if (result == MessageBoxResult.Cancel)
                    return;

                if (result == MessageBoxResult.Yes)
                    foreach (MediaItem mediaItem in e.MediaItems)
                        foreach (MediaItemPart part in mediaItem.Parts)
                            if (FileSystem.FileExists(part.Location.Value))
                                FileSystem.DeleteFile(part.Location.Value, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

                DeleteMediaItems(e.MediaItems.ToArray());
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not delete items: ");
            }
        }

        /// <summary>
        /// Deletes the media items from the database
        /// </summary>
        /// <param name="mediaItems">Media items being deleted</param>
        private void DeleteMediaItems(MediaItem[] mediaItems)
        {
            foreach (MediaItem mediaItem in mediaItems)
            {
                if (RecycleBinPrompt)
                    mediaItem.Delete();

                MediaItems.Remove(mediaItem);
            }

            OnMediaItemsDeleted(mediaItems);
        }

        /// <summary>
        /// Merges the parts of the selected media items into a single media item
        /// </summary>
        private void MergePartsOfSelectedMediaItems()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before merging the parts of any media items");
                    return;
                }

                CancelMediaItemsOperationEventArgs e = new CancelMediaItemsOperationEventArgs(SelectedMediaItems);
                OnMergingSelectedMediaItems(e);

                if (e.Cancel)
                {
                    GeneralMethods.MessageBoxApplicationError(e.ReasonForCancel);
                    return;
                }

                MediaItem mergedMediaItem = SelectedMediaItems[0];
                List<MediaItemPart> originalParts = new List<MediaItemPart>(mergedMediaItem.Parts);

                for (int i = 1; i < SelectedMediaItems.Length; i++)
                {
                    foreach (MediaItemPart part in SelectedMediaItems[i].Parts)
                        mergedMediaItem.Parts.Add(part.Location, part.Size, part.Duration);
                }

                MediaItemDialog mediaItemDialog = new MediaItemDialog(true, mergedMediaItem);
                mediaItemDialog.Owner = Application.Current.MainWindow;
                mediaItemDialog.ShowHidden = Filter.ShowHidden;
                mediaItemDialog.FileTypeAdded += new FileTypeEventHandler(mediaItemDialog_FileTypeAdded);

                if (GeneralMethods.GetNullableBoolValue(mediaItemDialog.ShowDialog()))
                {
                    DeleteMediaItems(SelectedMediaItems.Where(p => p != mergedMediaItem).ToArray());
                    OnMediaItemSaved(mergedMediaItem);
                }
                else
                    mergedMediaItem.Parts = new MediaItemPartCollection(originalParts);
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not merge parts: ");
            }
        }

        /// <summary>
        /// Extracts the part to a new media item
        /// </summary>
        /// <param name="part">Part being extracted</param>
        private void ExtractPart(MediaItemPart part)
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before extracting the parts of any media items");
                    return;
                }

                CancelMediaItemsOperationEventArgs e = new CancelMediaItemsOperationEventArgs(new MediaItem[1] { SelectedMediaItem });
                OnExtractingPartFromMediaItem(e);

                if (e.Cancel)
                {
                    GeneralMethods.MessageBoxApplicationError(e.ReasonForCancel);
                    return;
                }

                MediaItem extractedFrom = dgMediaItems.SelectedItem as MediaItem;
                MediaItem extractedTo;

                switch (extractedFrom.Type)
                {
                    case MediaItemTypeEnum.Song:
                        extractedTo = new Song();
                        break;

                    case MediaItemTypeEnum.Video:
                        extractedTo = new Video();
                        break;

                    default:
                        throw new UnknownEnumValueException(extractedFrom.Type);
                }

                extractedTo.Parts.Add(part.Location, part.Size, part.Duration);

                MediaItemDialog mediaItemDialog = new MediaItemDialog(true, extractedTo);
                mediaItemDialog.Owner = Application.Current.MainWindow;
                mediaItemDialog.ShowHidden = Filter.ShowHidden;
                mediaItemDialog.FileTypeAdded += new FileTypeEventHandler(mediaItemDialog_FileTypeAdded);

                if (GeneralMethods.GetNullableBoolValue(mediaItemDialog.ShowDialog()))
                {
                    extractedFrom.Parts.Remove(part.Location);
                    OnMediaItemSaved(extractedFrom);

                    OnMediaItemSaved(extractedTo);
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not extract part: ");
            }
        }

        /// <summary>
        /// Sets the name for each selected media item to it's filename
        /// </summary>
        private void SetNameFromFilename()
        {
            List<MediaItem> mediaItems = new List<MediaItem>();

            foreach (MediaItem mediaItem in SelectedMediaItems)
            {
                mediaItem.Name = Path.GetFileNameWithoutExtension(mediaItem.Parts[0].Location.Value);
                mediaItems.Add(mediaItem);
            }

            OnMediaItemsSaved(mediaItems.ToArray());
        }

        /// <summary>
        /// Sets the episode number from each selected video from it's title
        /// </summary>
        private void SetEpisodeNumberFromTitle()
        {
            List<MediaItem> mediaItems = new List<MediaItem>();

            foreach (Video video in SelectedMediaItems)
            {
                String episodeNumber = String.Empty;
                Int32 ptr = 0;

                while (ptr < video.Name.Length)
                {
                    if (!Char.IsNumber(video.Name[ptr]))
                        break;

                    episodeNumber += video.Name[ptr];
                    ptr++;
                }

                if (!String.IsNullOrEmpty(episodeNumber))
                {
                    video.Episode = Convert.ToInt16(episodeNumber.Trim());
                    video.Name = video.Name.Substring(ptr, video.Name.Length - ptr).Trim();
                    mediaItems.Add(video);
                }
            }

            OnMediaItemsSaved(mediaItems.ToArray());
        }

        /// <summary>
        /// Sets the episode number from each selected video from it's index in the view
        /// </summary>
        private void SetEpisodeNumberFromIndex()
        {
            for (int i = 0; i < SelectedMediaItems.Length; i++)
            {
                Video video = SelectedMediaItems[i] as Video;
                video.Episode = Convert.ToInt16(i + 1);
            }

            OnMediaItemsSaved(SelectedMediaItems);
        }

        /// <summary>
        /// Sets the number of episodes property of the selected videos to the number of videos selected
        /// </summary>
        private void SetNumberOfEpisodesToSelectionCount()
        {
            foreach (Video video in SelectedMediaItems)
                video.NumberOfEpisodes = Convert.ToInt16(SelectedMediaItems.Length);

            OnMediaItemsSaved(SelectedMediaItems);
        }

        /// <summary>
        /// Sets the track number from each selected song from it's title
        /// </summary>
        private void SetTrackNumberFromTitle()
        {
            List<MediaItem> mediaItems = new List<MediaItem>();

            foreach (Song song in SelectedMediaItems)
            {
                String trackNumber = String.Empty;
                Int32 ptr = 0;

                while (ptr < song.Name.Length)
                {
                    if (!Char.IsNumber(song.Name[ptr]))
                        break;

                    trackNumber += song.Name[ptr];
                    ptr++;
                }

                if (!String.IsNullOrEmpty(trackNumber))
                {
                    song.TrackNumber = Convert.ToInt16(trackNumber.Trim());
                    song.Name = song.Name.Substring(ptr, song.Name.Length - ptr).Trim();
                    mediaItems.Add(song);
                }
            }

            OnMediaItemsSaved(mediaItems.ToArray());
        }

        /// <summary>
        /// Sets the track number from each selected song from it's index in the view
        /// </summary>
        private void SetTrackNumberFromIndex()
        {
            for (int i = 0; i < SelectedMediaItems.Length; i++)
            {
                Song song = SelectedMediaItems[i] as Song;
                song.TrackNumber = Convert.ToInt16(i + 1);
            }

            OnMediaItemsSaved(SelectedMediaItems);
        }

        /// <summary>
        /// Sets the number of episodes property of the selected videos to the number of videos selected
        /// </summary>
        private void SetNumberOfTracksToSelectionCount()
        {
            foreach (Song song in SelectedMediaItems)
                song.NumberOfTracks = Convert.ToInt16(SelectedMediaItems.Length);

            OnMediaItemsSaved(SelectedMediaItems);
        }

        #endregion

        #region Event Handlers

        private void dgMediaItems_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    RemoveSelectedMediaItems();
                    break;
            }
        }

        private void dgMediaItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PlayFromHere();
            e.Handled = true;
        }

        private void dgMediaItems_ItemsSourceChanged(object sender, System.EventArgs e)
        {
            SortDescriptions.Clear();

            foreach (SortDescription sortDescription in previousSortDescriptions)
            {
                DataGridColumn column = Columns.SingleOrDefault(p => p.SortMemberPath == sortDescription.PropertyName);

                if (column != null)
                    column.SortDirection = sortDescription.Direction;

                SortDescriptions.Add(sortDescription);
            }
        }

        private void cmMediaItems_Opened(object sender, RoutedEventArgs e)
        {
            int selectedItemCount = SelectedMediaItems.Length;
            Boolean singleTypeSelected = Convert.ToBoolean(selectedItemCount);

            if (selectedItemCount > 1)
                singleTypeSelected = !SelectedMediaItems.Any(p => p.Type != SelectedMediaItems[0].Type);

            Visibility playOptionsVisibility = GeneralMethods.ConvertBooleanToVisibility(EnablePlayOptions);

            miPlay.Visibility = playOptionsVisibility;
            miPlay.IsEnabled = selectedItemCount >= 1;
            miPlayFromHere.Visibility = playOptionsVisibility;
            miPlayFromHere.IsEnabled = selectedItemCount == 1;
            miAddToNowPlaying.Visibility = playOptionsVisibility;
            miAddToNowPlaying.IsEnabled = selectedItemCount >= 1;
            sepPlay.Visibility = playOptionsVisibility;
            miShowInExplorer.IsEnabled = selectedItemCount >= 1;
            miEdit.IsEnabled = selectedItemCount >= 1;
            miModify.IsEnabled = (singleTypeSelected && (selectedItemCount >= 1));
            miDelete.IsEnabled = selectedItemCount >= 1;
            miMergeParts.Visibility = GeneralMethods.ConvertBooleanToVisibility(AllowMergeExtract);
            miMergeParts.IsEnabled = (singleTypeSelected && (selectedItemCount > 1));
            miSetNameFromFilename.IsEnabled = (selectedItemCount > 0);
            miSetEpisodeNumber.Visibility = GeneralMethods.ConvertBooleanToVisibility((selectedItemCount > 0) && (singleTypeSelected) && (SelectedMediaItems[0].Type == MediaItemTypeEnum.Video));
            miSetEpisodeNumber.IsEnabled = selectedItemCount >= 1;
            miSetNumberOfEpisodesToSelectionCount.Visibility = GeneralMethods.ConvertBooleanToVisibility((selectedItemCount > 0) && (singleTypeSelected) && (SelectedMediaItems[0].Type == MediaItemTypeEnum.Video));
            miSetNumberOfEpisodesToSelectionCount.IsEnabled = selectedItemCount >= 1;
            miSetTrackNumber.Visibility = GeneralMethods.ConvertBooleanToVisibility((selectedItemCount > 0) && (singleTypeSelected) && (SelectedMediaItems[0].Type == MediaItemTypeEnum.Song));
            miSetTrackNumber.IsEnabled = selectedItemCount >= 1;
            miSetNumberOfTracksToSelectionCount.Visibility = GeneralMethods.ConvertBooleanToVisibility((selectedItemCount > 0) && (singleTypeSelected) && (SelectedMediaItems[0].Type == MediaItemTypeEnum.Song));
            miSetNumberOfTracksToSelectionCount.IsEnabled = selectedItemCount >= 1;
            miTags.IsEnabled = (selectedItemCount == 1) && (SelectedMediaItem.Tags.Count > 0);
            miTags.ItemsSource = (SelectedMediaItem == null ? null : SelectedMediaItem.Tags);
            miExport.IsEnabled = (selectedItemCount > 0) && (ExportDirectories != null) && (ExportDirectories.Count > 0);
            miExport.ItemsSource = ExportDirectories;
        }

        private void miPlay_Click(object sender, RoutedEventArgs e)
        {
            PlaySelectedMediaItems();
        }

        private void miPlayFromHere_Click(object sender, RoutedEventArgs e)
        {
            PlayFromHere();
        }

        private void miAddToNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            AddToNowPlaying();
        }

        private void miShowInExplorer_Click(object sender, RoutedEventArgs e)
        {
            ShowSelectedMediaItemsInExplorer();
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedMediaItems();
        }

        private void miModify_Click(object sender, RoutedEventArgs e)
        {
            ModifySelectedMediaItems();
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedMediaItems();
        }

        private void miMergeParts_Click(object sender, RoutedEventArgs e)
        {
            MergePartsOfSelectedMediaItems();
        }

        private void miSetNameFromFilename_Click(object sender, RoutedEventArgs e)
        {
            SetNameFromFilename();
        }

        private void miSetEpisodeNumberFromTitle_Click(object sender, RoutedEventArgs e)
        {
            SetEpisodeNumberFromTitle();
        }

        private void miSetEpisodeNumberFromIndex_Click(object sender, RoutedEventArgs e)
        {
            SetEpisodeNumberFromIndex();
        }

        private void miSetNumberOfEpisodesToSelectionCount_Click(object sender, RoutedEventArgs e)
        {
            SetNumberOfEpisodesToSelectionCount();
        }

        private void miSetTrackNumberFromTitle_Click(object sender, RoutedEventArgs e)
        {
            SetTrackNumberFromTitle();
        }

        private void miSetTrackNumberFromIndex_Click(object sender, RoutedEventArgs e)
        {
            SetTrackNumberFromIndex();
        }

        private void miSetNumberOfTracksToSelectionCount_Click(object sender, RoutedEventArgs e)
        {
            SetNumberOfTracksToSelectionCount();
        }

        private void miTags_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;

            if (element != null)
            {
                if (element.DataContext != null)
                {
                    IntelligentString tag = (IntelligentString)element.DataContext;
                    Filter.SearchText = tag.Value;
                }
            }
        }

        private void miExport_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = e.OriginalSource as FrameworkElement;
            String exportDirectory = (String)element.DataContext;

            if (!Directory.Exists(exportDirectory))
            {
                MessageBox.Show(String.Format("Export directory \"{0}\" does not exist.", exportDirectory), "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExportMediaItemsDialog emid = new ExportMediaItemsDialog(exportDirectory, SelectedMediaItems);
            emid.Owner = Application.Current.MainWindow;
            emid.ShowDialog();
        }

        private void mediaItemDialog_FileTypeAdded(object sender, FileTypeEventArgs e)
        {
            OnFileTypeAdded(e.FileType);
        }

        private void mediaItemDialog_SavedMediaItem(object sender, MediaItemEventArgs e)
        {
            OnMediaItemSaved(e.MediaItem);
        }

        private void mediaItemDialog_PartExtracted(object sender, MediaItemPartEventArgs e)
        {
            ExtractPart(e.Part);
        }

        #endregion

        #region Static Methods

        private static void OnMediaItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemsView miv = d as MediaItemsView;
            miv.previousSortDescriptions = new List<SortDescription>(miv.SortDescriptions);
        }

        #endregion
    }
}

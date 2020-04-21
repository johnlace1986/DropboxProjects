using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MediaPlayer.Library.Business;
using MediaPlayer.EventArgs;
using Utilities.Business;
using Utilities.Exception;
using MediaPlayer.Business;
using System.IO;
using System.Windows.Controls;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for MediaItemDialog.xaml
    /// </summary>
    public partial class MediaItemDialog : Window
    {
        #region Events

        /// <summary>
        /// Fires when a new file type is added to the system
        /// </summary>
        public event FileTypeEventHandler FileTypeAdded;

        /// <summary>
        /// Fires when a media item is saved to the database
        /// </summary>
        public event MediaItemEventHandler SavedMediaItem;

        /// <summary>
        /// Fires when a part is extracted
        /// </summary>
        public event MediaItemPartEventHandler PartExtracted;

        #endregion

        #region Depencency Properties

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(MediaItem[]), typeof(MediaItemDialog));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(MediaItemDialog), new PropertyMetadata(new PropertyChangedCallback(OnSelectedIndexPropertyChanged)));

        public static readonly DependencyProperty SelectedMediaItemProperty = DependencyProperty.Register("SelectedMediaItem", typeof(MediaItem), typeof(MediaItemDialog), new PropertyMetadata(new PropertyChangedCallback(OnSelectedMediaItemPropertyChanged)));

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(Boolean?), typeof(MediaItemDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets the view used to display the media item
        /// </summary>
        public IMediaItemView SelectedMediaItemView
        {
            get
            {
                switch (SelectedMediaItem.Type)
                {
                    case MediaItemTypeEnum.Video:
                        return vvSelectedVideo as IMediaItemView;

                    case MediaItemTypeEnum.Song:
                        return svSelectedSong as IMediaItemView;

                    default:
                        throw new UnknownEnumValueException(SelectedMediaItem.Type);
                }
            }
        }

        /// <summary>
        /// Gets or sets the media items being displayed in the window
        /// </summary>
        public MediaItem[] MediaItems
        {
            get { return GetValue(MediaItemDialog.MediaItemsProperty) as MediaItem[]; }
            set { SetValue(MediaItemDialog.MediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index in the collection of media items being displayed of the selected media item
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return (Int32)GetValue(MediaItemDialog.SelectedIndexProperty); }
            set { SetValue(MediaItemDialog.SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected media item
        /// </summary>
        public MediaItem SelectedMediaItem
        {
            get { return GetValue(MediaItemDialog.SelectedMediaItemProperty) as MediaItem; }
            set { SetValue(MediaItemDialog.SelectedMediaItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not hidden videos show be displayed
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return GetValue(ShowHiddenProperty) as Boolean?; }
            set { SetValue(ShowHiddenProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not a new media item should have all of it's properties extracted from the filename
        /// </summary>
        public Boolean FullParseFilename { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemDialog class
        /// </summary>
        /// <param name="fullParseFilename">Value determining whether or not a new media item should have all of it's properties extracted from the filename</param>
        private MediaItemDialog(Boolean fullParseFilename)
        {
            SelectedIndex = -1;
            FullParseFilename = fullParseFilename;

            DataContext = this;

            InitializeComponent();
        }

        /// <summary>
        /// Initialises a new instance of the MediaItemDialog class
        /// </summary>
        /// <param name="fullParseFilename">Value determining whether or not a new media item should have all of it's properties extracted from the filename</param>
        /// <param name="mediaItems">Collection of media items to be edited</param>
        public MediaItemDialog(Boolean fullParseFilename, MediaItem[] mediaItems)
            : this(fullParseFilename)
        {
            MediaItems = mediaItems;
        }

        /// <summary>
        /// Initialises a new instance of the MediaItemDialog class
        /// </summary>
        /// <param name="fullParseFilename">Value determining whether or not a new media item should have all of it's properties extracted from the filename</param>
        /// <param name="mediaItem">Media item to be edited</param>
        public MediaItemDialog(Boolean fullParseFilename, MediaItem mediaItem)
            : this(fullParseFilename, new MediaItem[1] { mediaItem })
        {
            SelectedIndex = 0;
        }

        #endregion

        #region Instance Methods

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
        /// Fires the SavedMediaItem event
        /// </summary>
        /// <param name="mediaItem">The media item that was saved</param>
        private void OnSavedMediaItem(MediaItem mediaItem)
        {
            if (SavedMediaItem != null)
                SavedMediaItem(this, new MediaItemEventArgs(mediaItem));
        }

        /// <summary>
        /// Saves the selected media item to the database
        /// </summary>
        /// <returns>True if the media item is saved successfully, false if not</returns>
        private Boolean UpdateSelectedMediaItem()
        {
            try
            {
                if (IntelligentString.IsNullOrEmpty(SelectedMediaItem.Name))
                {
                    GeneralMethods.MessageBoxApplicationError("Please give the " + SelectedMediaItem.Type.ToString().ToLower() + " a name");
                    return false;
                }

                if (mipParts.MediaItem.Parts.Count == 0)
                {
                    GeneralMethods.MessageBoxApplicationError("A " + SelectedMediaItem.Type.ToString().ToLower() + " must have at least 1 part");
                    return false;
                }

                MediaItem selectedMediaItem = MediaItems[SelectedIndex];
                CopyPropertiesFromClone(selectedMediaItem);
                selectedMediaItem.Parts = new MediaItemPartCollection(mipParts.MediaItem.Parts);

                OnSavedMediaItem(selectedMediaItem);

                SelectedMediaItem = selectedMediaItem.Clone() as MediaItem;

                return true;
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not save video: ");
                return false;
            }
        }

        /// <summary>
        /// Copies the properties from the source media item to the target media item
        /// </summary>
        /// <param name="target">Target media item who is copying the properties from the source media item</param>
        public void CopyPropertiesFromClone(MediaItem target)
        {
            target.Genre = SelectedMediaItem.Genre;
            target.IsHidden = SelectedMediaItem.IsHidden;
            target.Tags = SelectedMediaItem.Tags;

            SelectedMediaItemView.CopyPropertiesFromClone(target);
        }

        /// <summary>
        /// Fires the PartExtracted event
        /// </summary>
        /// <param name="part">Part that was extracted</param>
        private void OnPartExtracted(MediaItemPart part)
        {
            if (PartExtracted != null)
                PartExtracted(this, new MediaItemPartEventArgs(part));
        }

        /// <summary>
        /// Adds a tag to the selected media item
        /// </summary>
        private void AddTag()
        {
            if (SelectedMediaItem.Tags.Contains(cmbAddTag.Text))
            {
                GeneralMethods.MessageBoxApplicationError(SelectedMediaItem.Name + " already contains tag \"" + cmbAddTag.Text + "\".");
                return;
            }

            SelectedMediaItem.Tags.Add(cmbAddTag.Text);
            cmbAddTag.Text = String.Empty;
            Keyboard.Focus(cmbAddTag);
        }

        /// <summary>
        /// Deletes the selected tags from the media item
        /// </summary>
        private void DeleteSelectedTags()
        {
            if (lstTags.SelectedItems.Count == 0)
            {
                GeneralMethods.MessageBoxApplicationError("Please select a tag to delete.");
                return;
            }

            String message;

            if (lstTags.SelectedItems.Count == 1)
                message = "Are you sure you want to delete \"" + (IntelligentString)lstTags.SelectedItem + "\" from " + SelectedMediaItem.Name + "?";
            else
                message = "Are you sure you want to delete these tags from " + SelectedMediaItem.Name + "?";

            if (GeneralMethods.MessageBox(message, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                List<IntelligentString> tags = new List<IntelligentString>(lstTags.SelectedItems.Cast<IntelligentString>());

                foreach (IntelligentString tag in tags)
                    SelectedMediaItem.Tags.Remove(tag);
            }
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (MediaItems == null)
            {
                MediaItems = new MediaItem[] { SelectedMediaItem };
                SelectedIndex = 0;
            }
        }

        private void element_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectedMediaItemView.SelectedElement = sender as UIElement;
        }

        private void element_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (UpdateSelectedMediaItem())
                        DialogResult = true;

                    break;
            }
        }

        private void mipParts_FileTypeAdded(object sender, FileTypeEventArgs e)
        {
            OnFileTypeAdded(e.FileType);
        }

        private void mipParts_PartsAdded(object sender, MediaItemPartsEventArgs e)
        {
            try
            {
                if (!SelectedMediaItem.IsInDatabase)
                {
                    if (SelectedMediaItem.Parts.Count == 1)
                    {
                        if (FullParseFilename)
                            SelectedMediaItemView.ParseFilename(mipParts.MediaItem.Parts[0].Location.Value);
                        else
                            mipParts.MediaItem.Name = Path.GetFileName(mipParts.MediaItem.Parts[0].Location.Value);
                    }
                }
            }
            catch (System.Exception ex)
            {
                GeneralMethods.MessageBoxException(ex, "Could not parse filename: ");
            }
        }

        private void mipParts_PartExtracted(object sender, MediaItemPartEventArgs e)
        {
            OnPartExtracted(e.Part);

            ///TODO: currently cannot update parts so must close window
            DialogResult = true;
        }

        private void lstTags_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    DeleteSelectedTags();
                    break;
            }
        }

        private void cmbAddTag_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    AddTag();
                    e.Handled = true;
                    break;
            }
        }

        private void btnAddTag_Click(object sender, RoutedEventArgs e)
        {
            AddTag();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedMediaItem();
            SelectedIndex--;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateSelectedMediaItem())
                DialogResult = true;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedMediaItem();
            SelectedIndex++;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fires when the value of the SelectedIndexProperty dependency property changes
        /// </summary>
        /// <param name="d">Dependency object containing the property that changed</param>
        /// <param name="e">Arguments passed to the change event</param>
        private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemDialog mid = d as MediaItemDialog;

            if (mid.SelectedIndex != -1)
            {
                MediaItem selectedMediaItem = mid.MediaItems[mid.SelectedIndex];

                Boolean modified = false;

                //refresh the size of the parts
                foreach (MediaItemPart part in selectedMediaItem.Parts)
                {
                    FileInfo fi = new FileInfo(part.Location.Value);

                    if (fi.Exists)
                    {
                        if (part.Size != fi.Length)
                        {
                            part.Size = fi.Length;
                            modified = true;
                        }
                    }
                }

                if (modified)
                    mid.OnSavedMediaItem(selectedMediaItem);

                mid.SelectedMediaItem = selectedMediaItem.Clone() as MediaItem;
            }
        }

        /// <summary>
        /// Fires when the value of the SelectedMediaItemProperty dependency property changes
        /// </summary>
        /// <param name="d">Dependency object containing the property that changed</param>
        /// <param name="e">Arguments passed to the change event</param>
        private static void OnSelectedMediaItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemDialog mid = d as MediaItemDialog;

            FocusManager.SetFocusedElement(mid, mid.btnOK);
            mid.UpdateLayout();
            FocusManager.SetFocusedElement(mid, mid.SelectedMediaItemView.SelectedElement);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaPlayer.Library.Business;
using forms = System.Windows.Forms;
using MediaPlayer.EventArgs;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using Utilities.Business;
using Utilities.Presentation.WPF.Windows;

namespace MediaPlayer.Presentation.UserControls.MediaItemViews
{
    /// <summary>
    /// Interaction logic for MediaItemPartsView.xaml
    /// </summary>
    public partial class MediaItemPartsView : UserControl
    {
        #region Events

        /// <summary>
        /// Fires when a new file type is added to the system
        /// </summary>
        public event FileTypeEventHandler FileTypeAdded;

        /// <summary>
        /// Fires when one or more part is added to the collection
        /// </summary>
        public event MediaItemPartsEventHandler PartsAdded;

        /// <summary>
        /// Fires when a part is deleted
        /// </summary>
        public event EventHandler PartDeleted;

        /// <summary>
        /// Fires when the index of a part changes
        /// </summary>
        public event MediaItemEventHandler IndexChanged;

        /// <summary>
        /// Fires when a part is extracted
        /// </summary>
        public event MediaItemPartEventHandler PartExtracted;

        /// <summary>
        /// Fires prior to a part being deleted from a media item
        /// </summary>
        public event CancelMediaItemsOperationEventHandler DeletingPart;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MediaItemProperty = DependencyProperty.Register("MediaItem", typeof(MediaItem), typeof(MediaItemPartsView), new PropertyMetadata(new PropertyChangedCallback(OnMediaItemPropertyChanged)));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(MediaItemPartsView));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the media item containing the parts displayed in the view
        /// </summary>
        public MediaItem MediaItem
        {
            get { return GetValue(MediaItemPartsView.MediaItemProperty) as MediaItem; }
            set { SetValue(MediaItemPartsView.MediaItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the library is currently being organised
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(MediaItemPartsView.IsOrganisingProperty); }
            set { SetValue(MediaItemPartsView.IsOrganisingProperty, value); }
        }

        #endregion

        #region Constructors

        public MediaItemPartsView()
        {
            InitializeComponent();
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
        /// Fires the PartsAdded event
        /// </summary>
        /// <param name="addedParts">Parts that were added</param>
        private void OnPartAdded(MediaItemPart[] addedParts)
        {
            if (PartsAdded != null)
                PartsAdded(this, new MediaItemPartsEventArgs(addedParts));
        }

        /// <summary>
        /// Fires the PartDelete event
        /// </summary>
        private void OnPartDeleted()
        {
            if (PartDeleted != null)
                PartDeleted(this, new System.EventArgs());
        }

        /// <summary>
        /// Fires the IndexChanged event
        /// </summary>
        /// <param name="mediaItem">The media item containing the part</param>
        private void OnIndexChanged(MediaItem mediaItem)
        {
            if (IndexChanged != null)
                IndexChanged(this, new MediaItemEventArgs(mediaItem));
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
        /// Fires the DeletingPart event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnDeletingPart(CancelMediaItemsOperationEventArgs e)
        {
            if (DeletingPart != null)
                DeletingPart(this, e);
        }

        /// <summary>
        /// Browses for a media file to add to the media item parts collection
        /// </summary>
        public void BrowseForMediaFiles()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any media items");
                    return;
                }

                FileType[] fileTypes = App.GetFileTypesForMediaItem(MediaItem);

                if (fileTypes != null)
                {
                    String[] filenames = BrowseForMediaFilenames(fileTypes, true);

                    if (filenames.Length > 0)
                    {
                        Dictionary<MediaItemPart, TimeSpan> parts = new Dictionary<MediaItemPart, TimeSpan>();

                        foreach (String filename in filenames)
                        {
                            if (MediaItem.MediaItemPartLocationExists(filename))
                            {
                                GeneralMethods.MessageBoxApplicationError("There is already a " + MediaItem.Type.ToString().ToLower() + " in the system with the filename: \"" + filename + "\"");
                                return;
                            }

                            FileInfo fi = new FileInfo(filename);

                            if (!fileTypes.Any(p => p.Extensions.Any(e => e.Value.ToLower() == fi.Extension.ToLower())))
                            {
                                try
                                {
                                    FileType fileType = GetFileTypeForExtension(fileTypes, fi.Extension, MediaItem.Type);

                                    if (fileType != null)
                                    {
                                        fileType.Save();

                                        if (!fileTypes.Contains(fileType))
                                            OnFileTypeAdded(fileType);
                                    }
                                }
                                catch (System.Exception e)
                                {
                                    GeneralMethods.MessageBoxException(e, "Could not add extension to file type: ");
                                }
                            }

                            MediaItemPart part = new MediaItemPart(fi.FullName) { Size = fi.Length, Index = Convert.ToInt16(MediaItem.Parts.Count) };

                            //store the duration of the part
                            parts.Add(part, MediaItem.GetDuration(fi.FullName));
                        }

                        foreach (KeyValuePair<MediaItemPart, TimeSpan> part in parts)
                        {
                            MediaItemPart newPart = MediaItem.Parts.Add(part.Key.Location, part.Key.Size, part.Key.Duration);
                            MediaItem.SetPartDuration(newPart.Index, part.Value);
                        }

                        OnPartAdded(parts.Keys.ToArray());
                    }
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not add part: ");
            }
        }

        /// <summary>
        /// Removes the selected parts from the collection
        /// </summary>
        private void RemoveSelectedParts()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any media items");
                    return;
                }

                if (dgParts.SelectedItems.Count == 0)
                {
                    GeneralMethods.MessageBoxApplicationError("Please select 1 or more parts to delete");
                    return;
                }

                if (dgParts.SelectedItems.Count == MediaItem.Parts.Count)
                {
                    GeneralMethods.MessageBoxApplicationError("A media item must have at least 1 part");
                    return;
                }

                CancelMediaItemsOperationEventArgs cea = new CancelMediaItemsOperationEventArgs(new MediaItem[1] { MediaItem });
                OnDeletingPart(cea);

                if (!cea.Cancel)
                {
                    MessageBoxResult result;

                    if (dgParts.SelectedItems.Count == 1)
                        result = GeneralMethods.MessageBox("Would you also like to move the physical " + MediaItem.Type.ToString().ToLower() + " file to the recycle bin?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    else
                        result = GeneralMethods.MessageBox("Would you also like to move the physical " + MediaItem.Type.ToString().ToLower() + " files to the recycle bin?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Cancel)
                        return;

                    if (result == MessageBoxResult.Yes)
                        foreach (MediaItemPart part in dgParts.SelectedItems)
                            if (FileSystem.FileExists(part.Location.Value))
                                FileSystem.DeleteFile(part.Location.Value, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

                    MediaItemPart[] selectedParts = dgParts.SelectedItems.Cast<MediaItemPart>().ToArray();
                    List<MediaItemPart> parts = new List<MediaItemPart>(MediaItem.Parts);
                    parts.Sort();

                    foreach (MediaItemPart part in selectedParts)
                        parts.Remove(part);

                    for (int i = 0; i < parts.Count; i++)
                    {
                        parts[i].Index = Convert.ToInt16(i);
                    }

                    MediaItem.Parts = new MediaItemPartCollection(parts);
                    OnPartDeleted();
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not delete parts: ");
            }
        }

        /// <summary>
        /// Decreases the index of the selected part(s) by 1
        /// </summary>
        private void MoveUp()
        {
            if (IsOrganising)
            {
                GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any media items");
                return;
            }

            List<MediaItemPart> parts = new List<MediaItemPart>(dgParts.SelectedItems.Cast<MediaItemPart>());

            if (parts.Count > 0)
            {
                parts.Sort();

                foreach (MediaItemPart part in parts)
                    if (part.Index != 0)
                        SwitchParts(part.Index, (short)(part.Index - 1));

                dgParts.SelectedItem = parts[0];

                OnIndexChanged(DataContext as MediaItem);
            }
        }

        /// <summary>
        /// Increases the index of the selected part(s) by 1
        /// </summary>
        private void MoveDown()
        {
            if (IsOrganising)
            {
                GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any media items");
                return;
            }

            List<MediaItemPart> parts = new List<MediaItemPart>(dgParts.SelectedItems.Cast<MediaItemPart>());

            if (parts.Count > 0)
            {
                parts.Sort();

                foreach (MediaItemPart part in parts)
                    if (part.Index < (MediaItem.Parts.Count - 1))
                        SwitchParts(part.Index, (short)(part.Index + 1));

                dgParts.SelectedItem = parts[0];

                OnIndexChanged(DataContext as MediaItem);
            }
        }

        /// <summary>
        /// Switches two items in the collection
        /// </summary>
        /// <param name="index1">Index of the first item being switched</param>
        /// <param name="index2">Index of the second item being switched</param>
        private void SwitchParts(Int16 index1, Int16 index2)
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any media items");
                    return;
                }

                List<MediaItemPart> parts = new List<MediaItemPart>(MediaItem.Parts);

                MediaItemPart part1 = parts[index1];
                MediaItemPart part2 = parts[index2];

                if (part1 != part2)
                {
                    part1.Index = index2;
                    part2.Index = index1;

                    parts[index1] = part2;
                    parts[index2] = part1;

                    parts.Sort();

                    MediaItem.Parts = new MediaItemPartCollection(parts);
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not switch parts: ");
            }
        }

        #endregion

        #region Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgParts.Items.SortDescriptions.Clear();
            dgParts.Items.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));
        }

        private void dgParts_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    RemoveSelectedParts();
                    break;
            }
        }

        private void cmParts_Opened(object sender, RoutedEventArgs e)
        {
            miShowInExplorer.IsEnabled = dgParts.SelectedItems.Count > 0;
            miRemove.IsEnabled = dgParts.SelectedItems.Count > 0;
            miExtract.IsEnabled = ((MediaItem.Parts.Count > 1) && (dgParts.SelectedItems.Count == 1));
            miMoveUp.IsEnabled = dgParts.SelectedItems.Count > 0;
            miMoveDown.IsEnabled = dgParts.SelectedItems.Count > 0;
        }

        private void miShowInExplorer_Click(object sender, RoutedEventArgs e)
        {
            List<String> directories = new List<String>();

            foreach (MediaItemPart part in dgParts.SelectedItems)
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

            foreach (String directory in directories)
                Process.Start(directory);
        }

        private void miRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedParts();
        }

        private void miExtract_Click(object sender, RoutedEventArgs e)
        {
            OnPartExtracted(dgParts.SelectedItem as MediaItemPart);
        }

        private void miMoveUp_Click(object sender, RoutedEventArgs e)
        {
            MoveUp();
        }

        private void miMoveDown_Click(object sender, RoutedEventArgs e)
        {
            MoveDown();
        }

        private void psbParts_UpClicked(object sender, RoutedEventArgs e)
        {
            MoveUp();
        }

        private void psbParts_AddClicked(object sender, RoutedEventArgs e)
        {
            BrowseForMediaFiles();
        }

        private void psbParts_DeleteClicked(object sender, RoutedEventArgs e)
        {
            RemoveSelectedParts();
        }

        private void psbParts_DownClicked(object sender, RoutedEventArgs e)
        {
            MoveDown();
        }
        
        #endregion

        #region Static Methods

        private static void OnMediaItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaItemPartsView mipv = d as MediaItemPartsView;

            if (mipv.MediaItem != null)
                if (mipv.MediaItem.Parts.Count == 0)
                    mipv.BrowseForMediaFiles();
        }

        /// <summary>
        /// Enables the user to browse the file system for media files
        /// </summary>
        /// <param name="fileTypes">Types of media files to look for</param>
        /// <returns>Media files selected by the user</returns>
        public static String[] BrowseForMediaFilenames(FileType[] fileTypes, Boolean multiSelected)
        {
            using (forms.OpenFileDialog ofd = new forms.OpenFileDialog())
            {
                ofd.Title = "Select a media file...";
                ofd.Multiselect = multiSelected;

                String filter = GetOpenFileDialogFilter(fileTypes);
                ofd.Filter = "All files|*.*";

                if (!String.IsNullOrEmpty(filter))
                {
                    ofd.Filter += "|" + filter;
                    ofd.FilterIndex = 2;
                }

                if (ofd.ShowDialog() == forms.DialogResult.OK)
                    return ofd.FileNames;
            }

            return new String[0];
        }

        /// <summary>
        /// Gets the filter text for an open file dialog window that will only display the file types specified
        /// </summary>
        /// <param name="FileTypes">FileTypes used to generate the filter</param>
        /// <returns>Filter text for an open file dialog window that will only display the file types specified</returns>
        private static String GetOpenFileDialogFilter(FileType[] FileTypes)
        {
            String filter = String.Empty;

            if (FileTypes != null)
            {
                if (FileTypes.Length > 0)
                {
                    String allMediaFilesFilter = String.Empty;

                    foreach (FileType fileType in FileTypes)
                    {
                        filter += fileType.FilterText + "|";
                        allMediaFilesFilter += fileType.Extensions.FilterText + ";";
                    }

                    if (filter.EndsWith("|"))
                        filter = filter.Substring(0, filter.Length - 1);

                    if (allMediaFilesFilter.EndsWith(";"))
                        allMediaFilesFilter = allMediaFilesFilter.Substring(0, allMediaFilesFilter.Length - 1);

                    filter = "All media files|" + allMediaFilesFilter + "|" + filter;
                }
            }

            return filter;
        }

        /// <summary>
        /// Gets the file type the extension is to be added to
        /// </summary>
        /// <param name="fileTypes">Current set of file types</param>
        /// <param name="extension">Extensino to check</param>
        /// <param name="associatedType">Type the extension should be associated with</param>
        /// <returns>File type the extension is to be added to</returns>
        public static FileType GetFileTypeForExtension(FileType[] fileTypes, String extension, MediaItemTypeEnum associatedType)
        {
            if (GeneralMethods.MessageBox("There is currently no file type associated with " + associatedType.ToString().ToLower() + "s containing the extension \"" + extension + "\". Would you like to add one?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GetTextValueDialog gtvd = new GetTextValueDialog(TextValueDialogInputType.ComboBox);
                gtvd.Style = (Style)App.Current.FindResource("dialogWindow");
                gtvd.Owner = Application.Current.MainWindow;
                gtvd.Title = "File Type";
                gtvd.Header = "Enter the name for the file type:";
                gtvd.ItemsSource = fileTypes;
                gtvd.DisplayMemberPath = "Name";
                gtvd.Width = 500;

                FileType fileType = null;

                if (GeneralMethods.GetNullableBoolValue(gtvd.ShowDialog()))
                {
                    if (fileTypes.Any(p => p.Name.ToLower() == gtvd.Value.ToLower()))
                        fileType = fileTypes.First(p => p.Name.ToLower() == gtvd.Value.ToLower());
                    else
                    {
                        fileType = new FileType();
                        fileType.Name = gtvd.Value;
                        fileType.MediaItemType = associatedType;
                    }

                    fileType.Extensions.Add(extension);
                }

                return fileType;
            }

            return null;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MediaPlayer.Library.Business;
using Utilities.Business;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.IO;
using MediaPlayer.EventArgs;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for RootFolderDialog.xaml
    /// </summary>
    public partial class RootFolderDialog : Window
    {
        #region DLL Import

        /// <summary>
        /// Gets the free disk space of the specified drive
        /// </summary>
        /// <param name="lpDirectoryName">Directory of the drive being requested</param>
        /// <param name="lpFreeBytesAvailable">Total number of avaiable bytes left in the drive</param>
        /// <param name="lpTotalNumberOfBytes">Total number of bytes left in the drive</param>
        /// <param name="lpTotalNumberOfFreeBytes">Total number of unused bytes left in the drive</param>
        /// <returns>True if the free disk space could be retrieved, false if not</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
            out ulong lpFreeBytesAvailable,
            out ulong lpTotalNumberOfBytes,
            out ulong lpTotalNumberOfFreeBytes);

        #endregion

        #region Events

        /// <summary>
        /// Fires when the path to a saved root folder changes
        /// </summary>
        public event RootFolderPathChangedEventHandler PathChanged;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty SelectedRootFolderProperty = DependencyProperty.Register("SelectedRootFolder", typeof(RootFolder), typeof(RootFolderDialog), new PropertyMetadata(new PropertyChangedCallback(OnSelectedRootFolderPropertyChanged)));

        public static readonly DependencyProperty TagsProperty = DependencyProperty.Register("Tags", typeof(IntelligentString[]), typeof(RootFolderDialog));

        public static readonly DependencyProperty IsValidPathProperty = DependencyProperty.Register("IsValidPath", typeof(Boolean), typeof(RootFolderDialog), new PropertyMetadata(false));

        public static readonly DependencyProperty TotalSpaceProperty = DependencyProperty.Register("TotalSpace", typeof(Int64), typeof(RootFolderDialog), new PropertyMetadata((Int64)0));

        public static readonly DependencyProperty UsedSpaceProperty = DependencyProperty.Register("UsedSpace", typeof(Int64), typeof(RootFolderDialog), new PropertyMetadata((Int64)0));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected root folder
        /// </summary>
        public RootFolder SelectedRootFolder
        {
            get { return GetValue(RootFolderDialog.SelectedRootFolderProperty) as RootFolder; }
            set { SetValue(RootFolderDialog.SelectedRootFolderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the tags available to the root folder
        /// </summary>
        public IntelligentString[] Tags
        {
            get { return GetValue(TagsProperty) as IntelligentString[]; }
            set { SetValue(TagsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the selected root folder is a valid path
        /// </summary>
        public Boolean IsValidPath
        {
            get { return (Boolean)GetValue(IsValidPathProperty); }
            private set { SetValue(IsValidPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the total space of the root folder
        /// </summary>
        public Int64 TotalSpace
        {
            get { return (Int64)GetValue(TotalSpaceProperty); }
            private set { SetValue(TotalSpaceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the total space left in the root folder
        /// </summary>
        public Int64 UsedSpace
        {
            get { return (Int64)GetValue(UsedSpaceProperty); }
            set { SetValue(UsedSpaceProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RootFolderDialog class
        /// </summary>
        /// <param name="tags">Tags available to the root folder</param>
        private RootFolderDialog(IntelligentString[] tags)
        {
            InitializeComponent();

            Tags = tags;
        }

        /// <summary>
        /// Initialises a new instance of the RootFolderDialog class
        /// </summary>
        /// <param name="tags">Tags available to the root folder</param>
        /// <param name="type">Type of media item the new root folder will be associated with</param>
        public RootFolderDialog(IntelligentString[] tags, MediaItemTypeEnum type)
            : this(tags)
        {
            SelectedRootFolder = new RootFolder(type);

            Title = "New Root Folder";
        }

        /// <summary>
        /// Initialises a new instance of the RootFolderDialog class
        /// </summary>
        /// <param name="tags">Tags available to the root folder</param>
        /// <param name="selectedRootFolder">Root folder being edited in the dialog</param>
        public RootFolderDialog(IntelligentString[] tags, RootFolder selectedRootFolder)
            : this(tags)
        {
            SelectedRootFolder = selectedRootFolder;

            Title = SelectedRootFolder.Path.Value;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the PathChanged event
        /// </summary>
        /// <param name="rootFolder">Root folder whose path changed</param>
        /// <param name="previousPath">Previous path of the root folder</param>
        private void OnPathChanged(RootFolder rootFolder, IntelligentString previousPath)
        {
            if (PathChanged != null)
                PathChanged(this, new RootFolderPathChangedEventArgs(rootFolder, previousPath));
        }

        /// <summary>
        /// Updates the total space and used space properties
        /// </summary>
        private void UpdateSize()
        {
            if (Directory.Exists(SelectedRootFolder.Path.Value))
            {
                ulong lpFreeBytesAvailable;
                ulong lpTotalNumberOfBytes;
                ulong lpTotalNumberOfFreeBytes;

                Boolean result = GetDiskFreeSpaceEx(SelectedRootFolder.Path.Value, out lpFreeBytesAvailable, out lpTotalNumberOfBytes, out lpTotalNumberOfFreeBytes);

                if (result)
                {
                    IsValidPath = true;
                    TotalSpace = Convert.ToInt64(lpTotalNumberOfBytes);
                    UsedSpace = Convert.ToInt64(lpTotalNumberOfBytes - lpTotalNumberOfFreeBytes);

                    return;
                }
            }

            IsValidPath = false;
            UsedSpace = 0;
            TotalSpace = 0;
        }

        /// <summary>
        /// Adds the tag entered by the user to the root folder
        /// </summary>
        private void AddTag()
        {
            try
            {
                if (String.IsNullOrEmpty(cmbTag.Text))
                {
                    GeneralMethods.MessageBoxApplicationError("Please enter a value for the tag");
                    return;
                }

                IntelligentString tag = cmbTag.Text;

                if (SelectedRootFolder.Tags.Contains(tag))
                {
                    GeneralMethods.MessageBoxApplicationError("Root folder already contains tag \"" + tag + "\"");
                    return;
                }

                List<IntelligentString> tags = new List<IntelligentString>(SelectedRootFolder.Tags);
                tags.Add(tag);
                tags.Sort();

                SelectedRootFolder.Tags = new ObservableCollection<IntelligentString>(tags);

                cmbTag.Text = String.Empty;
                cmbTag.Focus();
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not add tag: ");
            }
        }

        /// <summary>
        /// Removes the selected tags from the root folder
        /// </summary>
        private void RemoveSelectedTags()
        {
            try
            {
                List<IntelligentString> tags = new List<IntelligentString>();

                foreach (IntelligentString tag in lstTags.SelectedItems)
                    tags.Add(tag);

                foreach (IntelligentString tag in tags)
                    SelectedRootFolder.Tags.Remove(tag);
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not remove tags: ");
            }
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IntelligentString.IsNullOrEmpty(SelectedRootFolder.Path))
                fbtPath.Browse();

            fbtPath.Focus();
        }

        private void SelectedRootFolder_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Path")
                UpdateSize();
        }

        private void lstTags_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    RemoveSelectedTags();
                    break;
            }
        }

        private void cmbTag_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    AddTag();
                    break;
            }
        }

        private void btnAddTag_Click(object sender, RoutedEventArgs e)
        {
            AddTag();
        }

        private void btnRemoveTag_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedTags();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IntelligentString.IsNullOrEmpty(SelectedRootFolder.Path))
                {
                    GeneralMethods.MessageBoxApplicationError("Please select the path to the root folder.");
                    return;
                }

                Boolean updateMediaItems = false;
                IntelligentString previousPath = IntelligentString.Empty;

                if (SelectedRootFolder.IsInDatabase)
                {
                    RootFolder clone = new RootFolder(SelectedRootFolder.MediaItemType, SelectedRootFolder.Priority);

                    if (clone.Path != SelectedRootFolder.Path)
                    {
                        if (RootFolder.RootFolderPathExists(SelectedRootFolder.MediaItemType, SelectedRootFolder.Path.Value))
                        {
                            GeneralMethods.MessageBoxApplicationError("There is already another root folder with the path \"" + SelectedRootFolder.Path + "\"");
                            return;
                        }
                        
                        MessageBoxResult result = GeneralMethods.MessageBox("Would you like to update the locations of all media items that are in this root folder to match the new path?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                        switch (result)
                        {
                            case MessageBoxResult.Cancel:
                                return;
                            case MessageBoxResult.Yes:
                                updateMediaItems = true;
                                previousPath = clone.Path;
                                break;
                        }
                    }
                }
                else
                {
                    if (RootFolder.RootFolderPathExists(SelectedRootFolder.MediaItemType, SelectedRootFolder.Path.Value))
                    {
                        GeneralMethods.MessageBoxApplicationError("There is already a root folder with the path \"" + SelectedRootFolder.Path + "\"");
                        return;
                    }
                }

                SelectedRootFolder.Save();

                if (updateMediaItems)
                    OnPathChanged(SelectedRootFolder, previousPath);

                DialogResult = true;
            }
            catch (System.Exception ex)
            {
                GeneralMethods.MessageBoxException(ex, "Could not save root folder: ");
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fires when the SelectedRootFolderProperty value changes
        /// </summary>
        /// <param name="d">Dependency object containing the dependency property that changed</param>
        /// <param name="e">Arguments passed to the event</param>
        private static void OnSelectedRootFolderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RootFolderDialog rfd = d as RootFolderDialog;

            if (rfd.SelectedRootFolder != null)
            {
                rfd.SelectedRootFolder.PropertyChanged -= rfd.SelectedRootFolder_PropertyChanged;
                rfd.SelectedRootFolder.PropertyChanged += rfd.SelectedRootFolder_PropertyChanged;

                rfd.UpdateSize();
            }
        }

        #endregion
    }
}

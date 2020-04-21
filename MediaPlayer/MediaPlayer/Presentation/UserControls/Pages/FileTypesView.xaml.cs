using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using MediaPlayer.Library.Business;
using MediaPlayer.Presentation.Windows;
using Utilities.Business;
using Utilities.Presentation.WPF.Windows;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for FileTypesView.xaml
    /// </summary>
    public partial class FileTypesView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(String), typeof(FileTypesView));

        public static readonly DependencyProperty AssociatedTypeProperty = DependencyProperty.Register("AssociatedType", typeof(MediaItemTypeEnum), typeof(FileTypesView));

        public static readonly DependencyProperty FileTypesProperty = DependencyProperty.Register("FileTypes", typeof(FileType[]), typeof(FileTypesView));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the view's header
        /// </summary>
        public String Header
        {
            get { return (String)GetValue(FileTypesView.HeaderProperty); }
            set { SetValue(FileTypesView.HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of media item the file types in the view are associated with
        /// </summary>
        public MediaItemTypeEnum AssociatedType
        {
            get { return (MediaItemTypeEnum)GetValue(FileTypesView.AssociatedTypeProperty); }
            set { SetValue(FileTypesView.AssociatedTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the file types displayed in the view
        /// </summary>
        public FileType[] FileTypes
        {
            get { return GetValue(FileTypesView.FileTypesProperty) as FileType[]; }
            set { SetValue(FileTypesView.FileTypesProperty, value); }
        }

        /// <summary>
        /// Gets the current sort descriptions in the view
        /// </summary>
        public SortDescriptionCollection SortDescriptions
        {
            get { return dgFileTypes.Items.SortDescriptions; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileTypesView class
        /// </summary>
        public FileTypesView()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a new file type to the system
        /// </summary>
        private void AddFileType()
        {
            FileTypeDialog ftd = new FileTypeDialog(AssociatedType);
            ftd.Owner = Application.Current.MainWindow;

            if (GeneralMethods.GetNullableBoolValue(ftd.ShowDialog()))
                AddFileType(ftd.SelectedFileType);
        }

        /// <summary>
        /// Adds a new file type to the system
        /// </summary>
        /// <param name="fileType">File type being added</param>
        private void AddFileType(FileType fileType)
        {
            List<FileType> fileTypes = new List<FileType>(FileTypes);
            fileTypes.Add(fileType);
            fileTypes.Sort();

            List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

            FileTypes = fileTypes.ToArray();

            SortDescriptions.Clear();

            foreach (SortDescription sortDescription in sortDescriptions)
                SortDescriptions.Add(sortDescription);
        }

        /// <summary>
        /// Edits the selected file type
        /// </summary>
        private void EditSelectedFileType()
        {
            try
            {
                FileType selectedFileType = dgFileTypes.SelectedItem as FileType;

                if (selectedFileType == null)
                {
                    GeneralMethods.MessageBoxApplicationError("Please select a file type to edit");
                    return;
                }

                FileTypeDialog ftd = new FileTypeDialog(selectedFileType.Clone() as FileType);
                ftd.Owner = Application.Current.MainWindow;

                if (GeneralMethods.GetNullableBoolValue(ftd.ShowDialog()))
                {
                    List<FileType> fileTypes = new List<FileType>(FileTypes);
                    fileTypes.Remove(selectedFileType);
                    fileTypes.Add(ftd.SelectedFileType);
                    fileTypes.Sort();

                    List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

                    FileTypes = fileTypes.ToArray();

                    SortDescriptions.Clear();

                    foreach (SortDescription sortDescription in sortDescriptions)
                        SortDescriptions.Add(sortDescription);

                    dgFileTypes.SelectedItem = ftd.SelectedFileType;
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not edit file type: ");
            }
        }

        /// <summary>
        /// Deletes the selected file types from the system
        /// </summary>
        private void DeleteSelectedFileTypes()
        {
            try
            {
                List<FileType> selectedFileTypes = new List<FileType>();

                foreach (FileType fileType in dgFileTypes.SelectedItems)
                    selectedFileTypes.Add(fileType);

                MessageBoxResult result;

                switch (selectedFileTypes.Count)
                {
                    case 0:
                        GeneralMethods.MessageBoxApplicationError("Please select 1 or more file types to delete");
                        return;

                    case 1:
                        result = GeneralMethods.MessageBox("Are you sure you want to delete " + selectedFileTypes[0].Name + "?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        break;

                    default:
                        result = GeneralMethods.MessageBox("Are you sure you want to delete these file types?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        break;
                }

                if (result == MessageBoxResult.Yes)
                    DeleteFileTypes(selectedFileTypes.ToArray());
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not delete file types: ");
            }
        }

        /// <summary>
        /// Removes the specified file types from the system
        /// </summary>
        /// <param name="fileTypes">File types being removed from the system</param>
        private void DeleteFileTypes(FileType[] fileTypes)
        {
            try
            {
                List<FileType> lstFileTypes = new List<FileType>(FileTypes);

                foreach (FileType fileType in fileTypes)
                {
                    fileType.Delete();
                    lstFileTypes.Remove(fileType);
                }

                lstFileTypes.Sort();

                List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

                FileTypes = lstFileTypes.ToArray();

                SortDescriptions.Clear();

                foreach (SortDescription sortDescription in sortDescriptions)
                    SortDescriptions.Add(sortDescription);
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not delete file types: ");
            }
        }

        #endregion

        #region Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgFileTypes.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        private void cmFileTypes_Opened(object sender, RoutedEventArgs e)
        {
            miEdit.IsEnabled = dgFileTypes.SelectedItems.Count == 1;
            miDelete.IsEnabled = dgFileTypes.SelectedItems.Count >= 1;
            miMerge.IsEnabled = dgFileTypes.SelectedItems.Count > 1;
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedFileType();
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedFileTypes();
        }

        private void miMerge_Click(object sender, RoutedEventArgs e)
        {
            List<FileType> selectedFileTypes = new List<FileType>();

            foreach (FileType selectedFileType in dgFileTypes.SelectedItems)
                selectedFileTypes.Add(selectedFileType);

            selectedFileTypes.Sort();

            GetTextValueDialog gtvd = new GetTextValueDialog(TextValueDialogInputType.TextBox);
            gtvd.Style = (Style)FindResource("dialogWindow");
            gtvd.Owner = Application.Current.MainWindow;
            gtvd.Title = "Merge File Types";
            gtvd.Header = "Enter the name of the merged file types:";
            gtvd.Value = selectedFileTypes[0].Name;
            gtvd.Width = 500;

            if (GeneralMethods.GetNullableBoolValue(gtvd.ShowDialog()))
            {
                FileType mergedFileType;

                if (selectedFileTypes.Any(p => p.Name == gtvd.Value))
                {
                    mergedFileType = selectedFileTypes.First(p => p.Name == gtvd.Value);
                    selectedFileTypes.Remove(mergedFileType);
                }
                else
                {
                    if (FileType.FileTypeNameExists(gtvd.Value))
                    {
                        GeneralMethods.MessageBoxApplicationError("There is already another file type with the name \"" + gtvd.Value + "\"");
                        return;
                    }

                    mergedFileType = new FileType();
                    mergedFileType.Name = gtvd.Value;
                    mergedFileType.MediaItemType = AssociatedType;
                }

                foreach (FileType fileType in selectedFileTypes)
                        foreach (IntelligentString extension in fileType.Extensions)
                            if (!mergedFileType.Extensions.Contains(extension))
                                mergedFileType.Extensions.Add(extension);

                mergedFileType.Save();

                DeleteFileTypes(selectedFileTypes.ToArray());

                if (!FileTypes.Contains(mergedFileType))
                    AddFileType(mergedFileType);

                dgFileTypes.SelectedItem = mergedFileType;
            }
        }

        private void lstExtensions_KeyDown(object sender, KeyEventArgs e)
        {
            ListBox lstExtensions = sender as ListBox;
            FileType selectedFileType = lstExtensions.DataContext as FileType;
            IntelligentString extension = (IntelligentString)lstExtensions.SelectedItem;

            switch (e.Key)
            {
                case Key.Delete:
                    if (GeneralMethods.MessageBox("Are you sure you wish to remove \"" + extension + "\" from " + selectedFileType.Name + "?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        selectedFileType.Extensions.Remove(extension);
                        selectedFileType.Save();
                    }

                    e.Handled = true;
                    break;
            }
        }

        private void dgFileTypes_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    DeleteSelectedFileTypes();
                    break;
            }
        }

        private void btnAddFileType_Click(object sender, RoutedEventArgs e)
        {
            AddFileType();
        }

        private void btnDeleteFileType_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedFileTypes();
        }

        #endregion
    }
}

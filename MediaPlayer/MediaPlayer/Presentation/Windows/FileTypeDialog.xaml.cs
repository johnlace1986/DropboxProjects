using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for FileTypeDialog.xaml
    /// </summary>
    public partial class FileTypeDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty SelectedFileTypeProperty = DependencyProperty.Register("SelectedFileType", typeof(FileType), typeof(FileTypeDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected file type
        /// </summary>
        public FileType SelectedFileType
        {
            get { return GetValue(FileTypeDialog.SelectedFileTypeProperty) as FileType; }
            set { SetValue(FileTypeDialog.SelectedFileTypeProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileTypeDialog class
        /// </summary>
        /// <param name="type">Type of media item the new file type will be associated with</param>
        public FileTypeDialog(MediaItemTypeEnum type)
        {
            InitializeComponent();

            SelectedFileType = new FileType();
            SelectedFileType.MediaItemType = type;

            Title = "New File Type";
        }

        /// <summary>
        /// Initialises a new instance of the FileTypeDialog class
        /// </summary>
        /// <param name="selectedFileType">File type being edited in the dialog</param>
        public FileTypeDialog(FileType selectedFileType)
        {
            InitializeComponent();

            SelectedFileType = selectedFileType;

            Title = SelectedFileType.Name;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds the extension entered by the user to the file type
        /// </summary>
        private void AddExtension()
        {
            try
            {
                if (String.IsNullOrEmpty(txtExtension.Text))
                {
                    GeneralMethods.MessageBoxApplicationError("Please enter a value for the extension");
                    return;
                }

                IntelligentString extension = txtExtension.Text;

                if (!extension.StartsWith("."))
                    extension = "." + extension;

                if (extension.Substring(1).Contains("."))
                {
                    GeneralMethods.MessageBoxApplicationError("Extension contains multiple full stops");
                    return;
                }

                if (SelectedFileType.Extensions.Contains(extension))
                {
                    GeneralMethods.MessageBoxApplicationError("File type already contains extension \"" + extension + "\"");
                    return;
                }

                SelectedFileType.Extensions.Add(extension);
                SelectedFileType.Extensions.Sort();

                txtExtension.Text = String.Empty;
                txtExtension.Focus();
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not add extension: ");
            }
        }

        /// <summary>
        /// Removes the selected extensions from the file type
        /// </summary>
        private void RemoveSelectedExtensions()
        {
            try
            {
                List<IntelligentString> extensions = new List<IntelligentString>();

                foreach (IntelligentString extension in lstExtensions.SelectedItems)
                    extensions.Add(extension);

                foreach (IntelligentString extension in extensions)
                    SelectedFileType.Extensions.Remove(extension);
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not remove extension: ");
            }
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtName.SelectionStart = 0;
            txtName.SelectionLength = txtName.Text.Length;
        }

        private void lstExtensions_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    RemoveSelectedExtensions();
                    break;
            }
        }

        private void txtExtension_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    AddExtension();
                    break;
            }
        }

        private void btnAddExtension_Click(object sender, RoutedEventArgs e)
        {
            AddExtension();
        }

        private void btnRemoveExtension_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedExtensions();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(SelectedFileType.Name))
                {
                    GeneralMethods.MessageBoxApplicationError("Please give the file type a name");
                    return;
                }

                if (SelectedFileType.IsInDatabase)
                {
                    FileType clone = new FileType(SelectedFileType.Id);

                    if (clone.Name != SelectedFileType.Name)
                    {
                        if (FileType.FileTypeNameExists(SelectedFileType.Name))
                        {
                            GeneralMethods.MessageBoxApplicationError("There is already another file type with the name \"" + SelectedFileType.Name + "\"");
                            return;
                        }
                    }
                }
                else
                {
                    if (FileType.FileTypeNameExists(SelectedFileType.Name))
                    {
                        GeneralMethods.MessageBoxApplicationError("There is already a file type with the name \"" + SelectedFileType.Name + "\"");
                        return;
                    }
                }

                if (SelectedFileType.Extensions.Count == 0)
                {
                    GeneralMethods.MessageBoxApplicationError("Please give the file type at least 1 extension");
                    return;
                }

                SelectedFileType.Save();
                DialogResult = true;
            }
            catch (System.Exception ex)
            {
                GeneralMethods.MessageBoxException(ex, "Could not save file type: ");
            }
        }

        #endregion
    }
}

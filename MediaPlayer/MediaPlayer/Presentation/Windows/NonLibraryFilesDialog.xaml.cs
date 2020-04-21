using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business;
using System.Collections.ObjectModel;
using Utilities.Business;
using System.IO;
using MediaPlayer.Library.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for NonLibraryFilesDialog.xaml
    /// </summary>
    public partial class NonLibraryFilesDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<NonLibraryFile>), typeof(NonLibraryFilesDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the files that were found in root folders that don't belong to the media library
        /// </summary>
        public ObservableCollection<NonLibraryFile> ItemsSource
        {
            get { return GetValue(NonLibraryFilesDialog.ItemsSourceProperty) as ObservableCollection<NonLibraryFile>; }
            set { SetValue(NonLibraryFilesDialog.ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the options currently saved in the database
        /// </summary>
        private Options Options { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the NonLibraryFilesDialog class
        /// </summary>
        /// <param name="itemsSource">Files that were found in root folders that don't belong to the media library</param>
        /// <param name="options">Options currently saved in the database</param>
        public NonLibraryFilesDialog(NonLibraryFile[] itemsSource, Options options)
        {
            InitializeComponent();

            ItemsSource = new ObservableCollection<NonLibraryFile>(itemsSource);
            Options = options;
        }

        #endregion

        #region Event Handlers

        private void btnDeleteNonLibraryFile_Click(object sender, RoutedEventArgs e)
        {
            //get the file that was selected
            Button btnDeleteNonLibraryFile = sender as Button;
            NonLibraryFile nlf = btnDeleteNonLibraryFile.DataContext as NonLibraryFile;
            
            //delete file
            FileInfo fi = new FileInfo(nlf.FullName.Value);
            fi.Delete();

            //clean up folder again
            RootFolder.CleanUp(fi.Directory.FullName, new List<String>(), Options.RootFolderFileExceptions.ToArray(), Options.RootFolderDirectoryExceptions.ToArray(), nlf.IsInRootFolder);

            //remove file from data grid
            ItemsSource.Remove(nlf);

            if (ItemsSource.Count == 0)
                DialogResult = true;
        }

        private void btnAddRootFolderFileException_Click(object sender, RoutedEventArgs e)
        {
            Button btnAddRootFolderFileException = sender as Button;
            NonLibraryFile nlf = btnAddRootFolderFileException.DataContext as NonLibraryFile;

            Options.RootFolderFileExceptions.Add(nlf.Name.Value);
            Options.Save();

            NonLibraryFile[] matches = ItemsSource.Where(p => p.Name == nlf.Name).ToArray();

            foreach (NonLibraryFile nonLibraryFile in matches)
            {
                FileInfo fi = new FileInfo(nonLibraryFile.FullName.Value);

                RootFolder.CleanUp(fi.Directory.FullName, new List<String>(), Options.RootFolderFileExceptions.ToArray(), Options.RootFolderDirectoryExceptions.ToArray(), nonLibraryFile.IsInRootFolder);
                ItemsSource.Remove(nonLibraryFile);
            }

            if (ItemsSource.Count == 0)
                DialogResult = true;
        }

        private void btnAddRootFolderDirectoryException_Click(object sender, RoutedEventArgs e)
        {
            Button btnAddRootFolderDirectoryExceptio = sender as Button;
            NonLibraryFile nlf = btnAddRootFolderDirectoryExceptio.DataContext as NonLibraryFile;

            Options.RootFolderDirectoryExceptions.Add(nlf.SubFolderName.Value);
            Options.Save();

            NonLibraryFile[] matches = ItemsSource.Where(p => p.SubFolderName == nlf.SubFolderName).ToArray();

            foreach (NonLibraryFile nonLibraryFile in matches)
            {
                FileInfo fi = new FileInfo(nonLibraryFile.FullName.Value);

                RootFolder.CleanUp(fi.Directory.Parent.FullName, new List<String>(), Options.RootFolderFileExceptions.ToArray(), Options.RootFolderDirectoryExceptions.ToArray(), IntelligentString.IsNullOrEmpty(nonLibraryFile.SubFolderPath));
                ItemsSource.Remove(nonLibraryFile);
            }

            if (ItemsSource.Count == 0)
                DialogResult = true;
        }

        #endregion
    }
}

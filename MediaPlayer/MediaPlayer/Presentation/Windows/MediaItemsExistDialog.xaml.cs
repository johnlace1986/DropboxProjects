using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using MediaPlayer.Business;
using MediaPlayer.Library.Business;
using System.IO;
using MediaPlayer.EventArgs;
using MediaPlayer.Presentation.UserControls.MediaItemViews;
using Utilities.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for MediaItemsExistDialog.xaml
    /// </summary>
    public partial class MediaItemsExistDialog : Window
    {
        #region Events

        /// <summary>
        /// Fires when a new file type is added to the system
        /// </summary>
        public event FileTypeEventHandler FileTypeAdded;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<MediaItemExists>), typeof(MediaItemsExistDialog));

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(ObservableCollection<DataGridColumn>), typeof(MediaItemsExistDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the source of the data grid in the window
        /// </summary>
        public ObservableCollection<MediaItemExists> ItemsSource
        {
            get { return GetValue(MediaItemsExistDialog.ItemsSourceProperty) as ObservableCollection<MediaItemExists>; }
            private set { SetValue(MediaItemsExistDialog.ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the columns to display in the window
        /// </summary>
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return dgMediaItems.Columns; }
            set
            {
                dgMediaItems.Columns.Clear();

                foreach (DataGridColumn column in value)
                    dgMediaItems.Columns.Add(column);

                SetValue(MediaItemsExistDialog.ColumnsProperty, dgMediaItems.Columns);
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the library is currently being organised
        /// </summary>
        public Boolean IsOrganising { get; private set; }

        /// <summary>
        /// Gets or sets the available file types
        /// </summary>
        public ObservableCollection<FileType> FileTypes { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemsExistDialog class
        /// </summary>
        /// <param name="mediaItemParts">Media item parts that do not exist</param>
        /// <param name="fileTypes">Available file types</param>
        /// <param name="isOrganising">Value determining whether or not the library is currently being organised</param>
        public MediaItemsExistDialog(MediaItemExists[] mediaItemParts, FileType[] fileTypes, Boolean isOrganising)
        {
            InitializeComponent();

            ItemsSource = new ObservableCollection<MediaItemExists>(mediaItemParts);
            FileTypes = new ObservableCollection<FileType>(fileTypes);
            IsOrganising = isOrganising;
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
        /// Adds a text column to the data grid
        /// </summary>
        /// <param name="header">Header of the column</param>
        /// <param name="propertyName">Name of the property the column will be bound to</param>
        public void GenerateDataGridTextColumn(String header, String propertyName)
        {
            GenerateDataGridTextColumn(header, propertyName, propertyName);
        }

        /// <summary>
        /// Adds a text column to the data grid
        /// </summary>
        /// <param name="header">Header of the column</param>
        /// <param name="propertyName">Name of the property the column will be bound to</param>
        /// <param name="sortMemberPath">Name of the property the items in the data grid are sorted by when the header of this column is clicked</param>
        public void GenerateDataGridTextColumn(String header, String propertyName, String sortMemberPath)
        {
            GenerateDataGridTextColumn(header, propertyName, sortMemberPath, String.Empty);
        }

        /// <summary>
        /// Adds a text column to the data grid
        /// </summary>
        /// <param name="header">Header of the column</param>
        /// <param name="propertyName">Name of the property the column will be bound to</param>
        /// <param name="sortMemberPath">Name of the property the items in the data grid are sorted by when the header of this column is clicked</param>
        /// <param name="cellStyleKey">Key for the cell style</param>
        public void GenerateDataGridTextColumn(String header, String propertyName, String sortMemberPath, String cellStyleKey)
        {
            Binding binding = new Binding(propertyName);
            binding.Mode = BindingMode.OneWay;

            DataGridTextColumn dgtc = new DataGridTextColumn();
            dgtc.Header = header;
            dgtc.Binding = binding;

            if (!String.IsNullOrEmpty(cellStyleKey))
                dgtc.CellStyle = (Style)FindResource(cellStyleKey);

            Columns.Add(dgtc);
        }

        /// <summary>
        /// Adds a browse button column to the data grid
        /// </summary>
        public void GenerateBrowsePartTemplate()
        {
            DataGridTemplateColumn dgtc = new DataGridTemplateColumn();
            dgtc.Header = "Browse";
            dgtc.CellTemplate = (DataTemplate)FindResource("mediaItemPartExistsBrowseTemplate");
            dgtc.CanUserResize = false;

            Columns.Add(dgtc);
        }

        #endregion

        #region Event Handlers

        private void btnBrowsePart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any media items");
                    return;
                }

                Button btnBrowsePart = sender as Button;

                MediaItemExists mediaItemExists = btnBrowsePart.DataContext as MediaItemExists;

                String[] filenames = MediaItemPartsView.BrowseForMediaFilenames(FileTypes.ToArray(), false);

                if (filenames.Length == 1)
                {
                    String filename = filenames[0];

                    if (MediaItem.MediaItemPartLocationExists(filename))
                    {
                        GeneralMethods.MessageBoxApplicationError("There is already a " + mediaItemExists.MediaItem.Type.ToString().ToLower() + " in the system with the filename: \"" + filename + "\"");
                        return;
                    }

                    FileInfo fi = new FileInfo(filename);

                    if (!FileTypes.Any(p => p.Extensions.Any(ext => ext.Value.ToLower() == fi.Extension.ToLower())))
                    {
                        try
                        {
                            FileType fileType = MediaItemPartsView.GetFileTypeForExtension(FileTypes.ToArray(), fi.Extension, mediaItemExists.MediaItem.Type);

                            if (fileType != null)
                            {
                                fileType.Save();

                                if (!FileTypes.Contains(fileType))
                                {
                                    FileTypes.Add(fileType);
                                    OnFileTypeAdded(fileType);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            GeneralMethods.MessageBoxException(ex, "Could not add extension to file type: ");
                        }
                    }

                    mediaItemExists.Part.Location = filename;
                    mediaItemExists.Part.Size = fi.Length;
                    mediaItemExists.MediaItem.SetPartDuration(mediaItemExists.Part.Index, MediaItem.GetDuration(fi.FullName));
                    mediaItemExists.MediaItem.Save();

                    ItemsSource.Remove(mediaItemExists);

                    if (ItemsSource.Count == 0)
                        DialogResult = true;
                }
            }
            catch (System.Exception ex)
            {
                GeneralMethods.MessageBoxException(ex, "Could not locate file: ");
            }
        }

        #endregion
    }
}
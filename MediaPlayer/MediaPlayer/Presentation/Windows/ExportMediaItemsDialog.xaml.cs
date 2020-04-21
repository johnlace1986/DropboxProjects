using MediaPlayer.Library.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Utilities.Business;
using io = System.IO;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for ExportMediaItemsDialog.xaml
    /// </summary>
    public partial class ExportMediaItemsDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty ExportDirectoryProperty = DependencyProperty.Register("ExportDirectory", typeof(String), typeof(ExportMediaItemsDialog), new PropertyMetadata(String.Empty));

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(MediaItem[]), typeof(ExportMediaItemsDialog), new PropertyMetadata(new MediaItem[0]));

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(Int64), typeof(ExportMediaItemsDialog), new PropertyMetadata(0L));

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets a value determining whether or not the current export should cancel
        /// </summary>
        private Boolean cancel = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the directory the media items will be exported to
        /// </summary>
        public String ExportDirectory
        {
            get { return (String)GetValue(ExportDirectoryProperty); }
            set { SetValue(ExportDirectoryProperty, value); }
        }

        /// <summary>
        /// Gets or sets the media items being exported
        /// </summary>
        public MediaItem[] MediaItems
        {
            get { return GetValue(MediaItemsProperty) as MediaItem[]; }
            set { SetValue(MediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of media items that have been exported
        /// </summary>
        public Int64 Progress
        {
            get { return (Int64)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ExportMediaItemsDialog class
        /// </summary>
        /// <param name="exportDirectory">Directory the media items will be exported to</param>
        /// <param name="mediaItems">Media items being exported</param>
        public ExportMediaItemsDialog(String exportDirectory, MediaItem[] mediaItems)
        {
            InitializeComponent();

            ExportDirectory = exportDirectory;
            MediaItems = mediaItems;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Exports the specified media items
        /// </summary>
        /// <param name="data">Dictionary containing the data needed to export the media items</param>
        private void Export(object data)
        {
            cancel = false;

            try
            {
                Dictionary<String, object> parameters = data as Dictionary<String, object>;
                String exportDirectory = (String)parameters["ExportDirectory"];
                MediaItem[] mediaItems = (MediaItem[])parameters["MediaItems"];

                GeneralMethods.ExecuteDelegateOnGuiThread(Dispatcher, () => Progress = 0);
                List<String> createdFolders = new List<String>();

                foreach (MediaItem mediaItem in mediaItems)
                {
                    if (cancel)
                        break;

                    if (mediaItem.Parts.Count == 1)
                    {
                        String destination = io.Path.Combine(exportDirectory, mediaItem.OrganisedPath.Value + io.Path.GetExtension(mediaItem.Parts.FirstLocation.Value));

                        if (!io.Directory.Exists(io.Path.GetDirectoryName(destination)))
                            io.Directory.CreateDirectory(io.Path.GetDirectoryName(destination));

                        CopyFile(new io.FileInfo(mediaItem.Parts.FirstLocation.Value), destination);
                    }
                    else
                    {
                        for (int i = 0; i < mediaItem.Parts.Count; i++)
                        {
                            String destination = io.Path.Combine(exportDirectory, mediaItem.OrganisedPath.Value + " (Path " + i.ToString() + ")" + io.Path.GetExtension(mediaItem.Parts[i].Location.Value));

                            if (!io.Directory.Exists(io.Path.GetDirectoryName(destination)))
                                io.Directory.CreateDirectory(io.Path.GetDirectoryName(destination));

                            CopyFile(new io.FileInfo(mediaItem.Parts[i].Location.Value), destination);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(String.Format("Unable to export media items: {0}", e.Message), "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                GeneralMethods.ExecuteDelegateOnGuiThread(Dispatcher, () => Close());
            }
        }

        /// <summary>
        /// Copies a file to another location
        /// </summary>
        /// <param name="source">Source file being copied</param>
        /// <param name="destination">Path the file is being copied to</param>
        private void CopyFile(io.FileInfo source, String destination)
        {
            try
            {
                using (io.FileStream sourceStream = source.OpenRead())
                {
                    using (io.FileStream destinationStream = new io.FileStream(destination, io.FileMode.Create, io.FileAccess.Write))
                    {
                        const int maxBufferSize = 8192;

                        Int64 progress = 0L;

                        while (progress < source.Length)
                        {
                            if (cancel)
                                break;

                            Int64 bufferSize = source.Length - progress;

                            if (bufferSize > maxBufferSize)
                                bufferSize = maxBufferSize;

                            byte[] buffer = new byte[bufferSize];
                            sourceStream.Read(buffer, 0, buffer.Length);
                            destinationStream.Write(buffer, 0, buffer.Length);

                            GeneralMethods.ExecuteDelegateOnGuiThread(Dispatcher, () => Progress += bufferSize);
                            progress += bufferSize;
                        }
                    }
                }
            }
            catch
            {
                cancel = true;
                throw;
            }
            finally
            {
                if (cancel)
                    if (io.File.Exists(destination))
                        io.File.Delete(destination);
            }
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("ExportDirectory", ExportDirectory);
            parameters.Add("MediaItems", MediaItems);

            Thread thread = new Thread(new ParameterizedThreadStart(Export));
            thread.Start(parameters);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cancel = true;
        }

        #endregion
    }
}

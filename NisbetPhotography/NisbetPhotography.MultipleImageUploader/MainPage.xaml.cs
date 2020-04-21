using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using NisbetPhotography.MultipleImageUploader.NisbetPhotographyService;
using System.Threading;
using System.ServiceModel;
using System.Net.Browser;
using System.Windows.Browser;

namespace NisbetPhotography.MultipleImageUploader
{
    public partial class MainPage : UserControl
    {
        #region Fields

        /// <summary>
        /// List of files the user has selected to upload
        /// </summary>
        private List<byte[]> filesAsBytes = new List<byte[]>();

        /// <summary>
        /// The type of album the images are being uploaded to
        /// </summary>
        private AlbumTypeEnum type;

        /// <summary>
        /// The unique identifier of the album
        /// </summary>
        private Int16 albumId = -1;

        /// <summary>
        /// The unique identifer of the customer who owns the album if the album type is customer
        /// </summary>
        private Guid customerId;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainPage(AlbumTypeEnum type, Int16 albumId, Guid customerId)
        {
            InitializeComponent();

            this.type = type;
            this.albumId = albumId;
            this.customerId = customerId;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Uploads the selected image files
        /// </summary>
        private void AddPictures()
        {
            SetButtonsEnabled(false);

            Thread thread = new Thread(new ThreadStart(UploadImageAsync));
            thread.Start();
        }

        /// <summary>
        /// Asynchronously uploads all image files
        /// </summary>
        private void UploadImageAsync()
        {            
            for(int i  = 0; i < filesAsBytes.Count; i++)
            {
                //update gui to show image is uploading
                Deployment.Current.Dispatcher.BeginInvoke(() => txtBrowse.Text = "Uploading image " + (i + 1).ToString() + " of " + filesAsBytes.Count.ToString() + " ...");

                byte[] fileAsBytes = filesAsBytes[i];

                //create handle so the thread will wait for single upload to complete
                ManualResetEvent handle = new ManualResetEvent(false);

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("Type", type);
                parameters.Add("AlbumId", albumId);
                parameters.Add("CustomerId", customerId);
                parameters.Add("FileAsBytes", fileAsBytes);
                parameters.Add("Handle", handle);

                //upload image
                switch (type)
                {
                    case AlbumTypeEnum.Customer:

                        NisbetPhotographyServiceClient customerClient = new NisbetPhotographyServiceClient();
                        customerClient.UploadImageToCustomerAlbumCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UploadImageCompleted);
                        customerClient.UploadImageToCustomerAlbumAsync(customerId, albumId, fileAsBytes, parameters);
                        
                        break;
                    case AlbumTypeEnum.Portfolio:
                        
                        NisbetPhotographyServiceClient portfolioClient = new NisbetPhotographyServiceClient();
                        portfolioClient.UploadImageToPortfolioCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UploadImageCompleted);
                        portfolioClient.UploadImageToPortfolioAsync(albumId, fileAsBytes, parameters);
                        
                        break;
                    case AlbumTypeEnum.Public:
                        
                        NisbetPhotographyServiceClient publicClient = new NisbetPhotographyServiceClient();
                        publicClient.UploadImageToPublicAlbumCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UploadImageCompleted);
                        publicClient.UploadImageToPublicAlbumAsync(albumId, fileAsBytes, parameters);
                        
                        break;
                    default:
                        return;
                }

                //wait for image to finish upload asynchronously
                handle.WaitOne();

                if (parameters.ContainsKey("Error"))
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => UploadImageAsyncError((Exception)parameters["Error"]));
                    return;
                }
            }

            Deployment.Current.Dispatcher.BeginInvoke(() => UploadImageAsyncComplete());
        }

        /// <summary>
        /// Occurs when asynchronous upload of all files succeeds
        /// </summary>
        private void UploadImageAsyncComplete()
        {
            RefreshPage();
        }

        /// <summary>
        /// Occurs when a file could not be uploaded
        /// </summary>
        /// <param name="e"></param>
        private void UploadImageAsyncError(Exception e)
        {
            txtBrowse.Text = "Error uploading images";
            filesAsBytes.Clear();
            SetButtonsEnabled(true);
        }

        /// <summary>
        /// Sets the IsEnabled property of all buttons in the control
        /// </summary>
        /// <param name="enabled">Determines whether or not the button are enabled</param>
        private void SetButtonsEnabled(bool enabled)
        {
            btnAddPictures.IsEnabled = enabled;
            btnBrowse.IsEnabled = enabled;
            btnCancel.IsEnabled = enabled;
        }

        private void RefreshPage()
        {
            HtmlPage.Window.Eval("window.location = '" + HtmlPage.Document.DocumentUri.ToString() + "'");
        }

        #endregion

        #region Event Handlers

        private void client_UploadImageCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //get the parameters
            Dictionary<string, object> parameters = e.UserState as Dictionary<string, object>;
            ManualResetEvent handle = (ManualResetEvent)parameters["Handle"];

            if (e.Error != null)
            {
                FaultException<ExceptionDetail> fault = e.Error as FaultException<ExceptionDetail>;

                parameters.Add("Error", e.Error);
            }

            handle.Set();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            filesAsBytes.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            bool? showDialog = ofd.ShowDialog();

            if ((showDialog != null) && (showDialog.Value))
            {
                try
                {
                    foreach (FileInfo fi in ofd.Files)
                    {
                        using (FileStream fs = fi.OpenRead())
                        {                            
                            //load file into bitmap to make sure it's a valid image
                            BitmapImage source = new BitmapImage();
                            source.SetSource(fs);

                            //reset the stream's position to 0
                            fs.Position = 0;

                            //read stream into byte array
                            byte[] fileAsBytes = new byte[fs.Length];
                            fs.Read(fileAsBytes, 0, fileAsBytes.Length);
                            filesAsBytes.Add(fileAsBytes);
                        }
                    }
                }
                catch(Exception ex)
                {
                    txtBrowse.Text = "Invalid image file selected.";
                    filesAsBytes.Clear();
                    return;
                }
            }

            if (filesAsBytes.Count == 1)
                txtBrowse.Text = "1 image selected.";
            else
                txtBrowse.Text = filesAsBytes.Count.ToString() + " images selected.";
        }

        private void btnAddPictures_Click(object sender, RoutedEventArgs e)
        {
            txtBrowse.Text = "";

            if (filesAsBytes.Count == 0)
            {
                txtBrowse.Text = "Please select at least 1 image";
                return;
            }

            AddPictures();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage();
        }

        #endregion
    }
}

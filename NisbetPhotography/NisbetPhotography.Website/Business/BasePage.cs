using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Services;
using System.Net.Mail;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Business
{
    public class BasePage : Page
    {
        #region Fields

        /// <summary>
        /// Current session state
        /// </summary>
        //private static HttpSessionState Session = HttpContext.Current.Session;

        #endregion

        #region Properties

        /// <summary>
        /// Url of error page to redirect to
        /// </summary>
        public String ErrorPageUrl { get; set; }

        public String WebsiteUrl
        {
            get
            {
                return Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Length - Request.Url.PathAndQuery.Length);
            }
        }

        #region App Settings

        /// <summary>
        /// Number of columns in tables displaying image thumbnails
        /// </summary>
        public static Int32 ThumbnailImageColumnCount
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailImageColumnCount"]); }
        }

        /// <summary>
        /// Maximum height in pixels an uploaded image can be
        /// </summary>
        public static Int32 MaxPictureHeight
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxPictureHeight"]); }
        }

        /// <summary>
        /// Maximum width in pixels an uploaded image can be
        /// </summary>
        public static Int32 MaxPictureWidth
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxPictureWidth"]); }
        }

        /// <summary>
        /// Maximum number of characters to display in error message
        /// </summary>
        public static Int32 MaxErrorMessageLength
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxErrorMessageLength"]); }
        }

        /// <summary>
        /// SMTP settings for outgoing email account
        /// </summary>
        public static String SmtpSettings
        {
            get { return ConfigurationManager.AppSettings["SmtpSettings"]; }
        }

        /// <summary>
        /// Username for outgoing email account
        /// </summary>
        public static String EmailUserName
        {
            get { return ConfigurationManager.AppSettings["EmailUserName"]; }
        }

        /// <summary>
        /// Password for outgoing email account
        /// </summary>
        public static String EmailPassword
        {
            get { return ConfigurationManager.AppSettings["EmailPassword"]; }
        }

        /// <summary>
        /// Email address to send enquiries to
        /// </summary>
        public static String EnquiryEmailAddress
        {
            get { return ConfigurationManager.AppSettings["EnquiryEmailAddress"]; }
        }

        #endregion

        #region Session Variables

        /// <summary>
        /// Currently logged in user
        /// </summary>
        public static DbObjects.Business.User CurrentUser
        {
            get
            {
                System.Security.Principal.IIdentity Identity = HttpContext.Current.User.Identity;

                if (Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.Session["CurrentUser"] == null)
                        HttpContext.Current.Session["CurrentUser"] = new DbObjects.Business.User(new Guid(Identity.Name));

                    return (DbObjects.Business.User)HttpContext.Current.Session["CurrentUser"];
                }
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
        }

        /// <summary>
        /// List of categories in the site's porfolio
        /// </summary>
        public DbObjects.Business.PortfolioCategory[] PortfolioCategories
        {
            get
            {
                if (Session["PortfolioCategories"] == null)
                    Session["PortfolioCategories"] = DbObjects.Business.PortfolioCategory.GetPortfolioCategories();

                return (DbObjects.Business.PortfolioCategory[])Session["PortfolioCategories"];
            }
            set
            {
                Session["PortfolioCategories"] = value;
            }
        }

        /// <summary>
        /// The portfolio category that was selected last
        /// </summary>
        public static DbObjects.Business.PortfolioCategory SelectedPortfolioCategory
        {
            get
            {
                if (HttpContext.Current.Session["SelectedPortfolioCategory"] == null)
                    return null;

                return (DbObjects.Business.PortfolioCategory)HttpContext.Current.Session["SelectedPortfolioCategory"];
            }
            set
            {
                HttpContext.Current.Session["SelectedPortfolioCategory"] = value;
            }
        }

        /// <summary>
        /// List of errors that have occured in the website
        /// </summary>
        public DbObjects.Business.Error[] Errors
        {
            get
            {
                if (Session["Errors"] == null)
                    Session["Errors"] = DbObjects.Business.Error.GetErrors();

                return (DbObjects.Business.Error[])Session["Errors"];
            }
            set
            {
                Session["Errors"] = value;
            }
        }

        /// <summary>
        /// The error that was selected last
        /// </summary>
        public DbObjects.Business.Error SelectedError
        {
            get
            {
                if (Session["SelectedError"] == null)
                    return null;

                return (DbObjects.Business.Error)Session["SelectedError"];
            }
            set
            {
                Session["SelectedError"] = value;
            }
        }

        /// <summary>
        /// List of customers in the database
        /// </summary>
        public DbObjects.Business.User[] Customers
        {
            get
            {
                if (Session["Customers"] == null)
                    Session["Customers"] = DbObjects.Business.User.GetUsers().Where(p => p.Admin == false).ToArray<DbObjects.Business.User>();

                return (DbObjects.Business.User[])Session["Customers"];
            }
            set
            {
                Session["Customers"] = value;
            }
        }

        /// <summary>
        /// The customer that was selected last
        /// </summary>
        public DbObjects.Business.User SelectedCustomer
        {
            get
            {
                if (Session["SelectedCustomer"] == null)
                    return null;

                return (DbObjects.Business.User)Session["SelectedCustomer"];
            }
            set
            {
                Session["SelectedCustomer"] = value;
            }
        }

        /// <summary>
        /// The customer album that was selected last
        /// </summary>
        public static DbObjects.Business.CustomerAlbum SelectedCustomerAlbum
        {
            get
            {
                if (HttpContext.Current.Session["SelectedCustomerAlbum"] == null)
                    return null;

                return (DbObjects.Business.CustomerAlbum)HttpContext.Current.Session["SelectedCustomerAlbum"];
            }
            set
            {
                HttpContext.Current.Session["SelectedCustomerAlbum"] = value;
            }
        }

        /// <summary>
        /// List of public albums that have been added to the website
        /// </summary>
        public DbObjects.Business.PublicAlbum[] PublicAlbums
        {
            get
            {
                if (Session["PublicAlbums"] == null)
                    Session["PublicAlbums"] = DbObjects.Business.PublicAlbum.GetPublicAlbums();

                return (DbObjects.Business.PublicAlbum[])Session["PublicAlbums"];
            }
            set
            {
                Session["PublicAlbums"] = value;
            }
        }

        /// <summary>
        /// The album that was selected last
        /// </summary>
        public static DbObjects.Business.PublicAlbum SelectedPublicAlbum
        {
            get
            {
                if (HttpContext.Current.Session["SelectedPublicAlbum"] == null)
                    return null;

                return (DbObjects.Business.PublicAlbum)HttpContext.Current.Session["SelectedPublicAlbum"];
            }
            set
            {
                HttpContext.Current.Session["SelectedPublicAlbum"] = value;
            }
        }

        #endregion

        #endregion

        #region Instance Methods

        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            //save error
            LogError(Server.GetLastError());

            //redirect to error page
            Response.Redirect(ErrorPageUrl);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Saves the exception and all inner exceptions to the database
        /// </summary>
        /// <param name="e">Exception that was thrown</param>
        private static void LogError(System.Exception e)
        {
            //if the database connection is not down
            System.Exception connectionError;

            if (DbObjects.Data.Control.TestConnection(out connectionError))
            {
                DbObjects.Business.Error parent = null;

                //traverse down inner exceptions
                while (e != null)
                {
                    //all app errors are inner exceptions to a HttpUnhandledException so we want to ignore the first one
                    if (e.GetType().Name != "HttpUnhandledException")
                    {
                        DbObjects.Business.Error child = DbObjects.Business.Error.FromException(e);
                        child.Save();

                        if (parent != null)
                            parent.AddInnerError(child);

                        parent = child;
                    }

                    e = e.InnerException;
                }
            }
            else
            {
                ///TODO: output error to alternative souce along with connectionError
            }
        }

        /// <summary>
        /// Determines whether or not an email address is valid
        /// </summary>
        /// <param name="emailAddress">Email address to validate</param>
        /// <returns>True if the email address is valid, false if not</returns>
        public static Boolean IsEmailAddressValid(string emailAddress)
        {
            try
            {
                MailAddress ma = new MailAddress(emailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected Bitmap ResizeImage(Bitmap img)
        {
            if (img.Width > MaxPictureWidth)
            {
                double factor = Convert.ToDouble(MaxPictureWidth) / Convert.ToDouble(img.Width);
                double newHeight = img.Height * factor;

                return ResizeImage(DrawImage(img, MaxPictureWidth, Convert.ToInt32(newHeight)));
            }

            if (img.Height > MaxPictureHeight)
            {
                double factor = Convert.ToDouble(MaxPictureHeight) / Convert.ToDouble(img.Height);
                double newWidth = img.Width * factor;

                return ResizeImage(DrawImage(img, Convert.ToInt32(newWidth), MaxPictureHeight));
            }

            return img;
        }

        private static Bitmap DrawImage(System.Drawing.Image img, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
            {
                g.DrawImage(img, 0, 0, width, height);
            }

            return result;
        }

        protected static void LoadSilverlightControl(Literal initParams, AlbumTypeEnum type, Int16 albumId, Guid customerId)
        {
            initParams.Text = "<param name=\"initParams\" value=\"Type=" + ((int)type).ToString() + ",AlbumId=" + albumId.ToString() + ",CustomerId=" + customerId.ToString() + "\" />";
        }

        #region Web Methods

        /// <summary>
        /// Allows the SelectedPortfolioCategory session variable to be set in javascript
        /// </summary>
        /// <param name="portfolioCategoryId">Unique identifier of the category</param>
        [WebMethod]
        public static void SetSelectedPortfolioCategoryFromJS(short portfolioCategoryId)
        {
            SelectedPortfolioCategory = new DbObjects.Business.PortfolioCategory(portfolioCategoryId);
        }

        /// <summary>
        /// Allows the SelectedPublicAlbum session variable to be set in javascript
        /// </summary>
        /// <param name="publicAlbumId">Unique identifier of the album</param>
        [WebMethod]
        public static void SetSelectedPublicAlbumFromJS(short publicAlbumId)
        {
            SelectedPublicAlbum = new DbObjects.Business.PublicAlbum(publicAlbumId);
        }

        /// <summary>
        /// Allows the SelectedCustomerAlbum session variable to be set in javascript
        /// </summary>
        /// <param name="customerAlbumId">Unique identifier of the album</param>
        /// <param name="customerId">Unique identifier of the customer who owns the album</param>
        [WebMethod]
        public static void SetSelectedCustomerAlbumFromJS(short customerAlbumId, string customerId)
        {
            SelectedCustomerAlbum = new DbObjects.Business.CustomerAlbum(customerAlbumId, new Guid(customerId));
        }

        #endregion

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace NisbetPhotography.DbObjects.Business
{
    internal static class ConfigSettings
    {
        /// <summary>
        /// SqlConnection to the master database
        /// </summary>
        public static SqlConnection MasterDbConnection
        {
            get { return new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDb"].ConnectionString); }
        }

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
    }
}

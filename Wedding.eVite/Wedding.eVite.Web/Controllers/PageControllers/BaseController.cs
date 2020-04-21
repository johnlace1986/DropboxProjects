using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Utilities.Data;
using Utilities.Data.SQL;
using Utilities.Exception;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Properties;

namespace Wedding.eVite.Web.Controllers.PageControllers
{
    public abstract class BaseController : Controller
    {
        #region Properties

        /// <summary>
        /// Gets or sets the invite of the guest who is currently logged in
        /// </summary>
        protected Invite LoggedInInvite
        {
            get { return (Invite)System.Web.HttpContext.Current.Session["LoggedInInvite"]; }
            set { System.Web.HttpContext.Current.Session["LoggedInInvite"] = value; }
        }

        protected String LastUsername
        {
            get { return (String)System.Web.HttpContext.Current.Session["LastUsername"]; }
            set { System.Web.HttpContext.Current.Session["LastUsername"] = value; }
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the connection string to the database from the app config file
        /// </summary>
        private static String ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString; }
        }

        /// <summary>
        /// Gets the email address emails will be sent from
        /// </summary>
        private static String MailFrom
        {
            get { return ConfigurationManager.AppSettings["MailFrom"]; }
        }

        /// <summary>
        /// Gets the SMTP host used to send emails
        /// </summary>
        private static String SmtpHost
        {
            get { return ConfigurationManager.AppSettings["SMTP.Host"]; }
        }

        /// <summary>
        /// Gets the SMTP port used to send emails
        /// </summary>
        private static Int32 SmtpPort
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["SMTP.Port"]); }
        }

        /// <summary>
        /// Gets the password for the account used to send emails
        /// </summary>
        private static String SmtpPassword
        {
            get { return ConfigurationManager.AppSettings["SMTP.Password"]; }
        }

        /// <summary>
        /// Gets a value determining whether or not SSL is enabled on emails
        /// </summary>
        private static Boolean EnableSsl
        {
            get { return Boolean.Parse(ConfigurationManager.AppSettings["SMTP.EnableSSL"]); }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Opens a connection to the database, performs an action and closes the connection to the database
        /// </summary>
        /// <param name="action">Action to perform</param>
        public static void DatabaseAction(Action<SqlConnection> action)
        {
            SqlDbHelper.DatabaseAction(ConnectionString, action);
        }

        /// <summary>
        /// Opens a connection to the database, performs an function, closes the connection to the database and returns the result of the function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="function">Function to perform</param>
        /// <returns>Result of the function</returns>
        public static T DatabaseFunction<T>(Func<SqlConnection, T> function)
        {
            return SqlDbHelper.DatabaseFunction<T>(ConnectionString, function);
        }

        /// <summary>
        /// Sends an invite email to the specified invite
        /// </summary>
        /// <param name="invite">Invite the email is being sent to</param>
        protected static void SendInviteEmail(Invite invite)
        {
            MailMessage email = new MailMessage();
            email.To.Add(invite.EmailAddress);
            email.From = new MailAddress(MailFrom, "John Lace & Lizzie Toms");
            email.Subject = "John & Lizzie are getting married";
            email.IsBodyHtml = true;
            
            email.Body = (invite.IncludesCeremony ? Resources.InviteEmail : Resources.InviteEmailReceptionOnly)
                .Replace("[PASSWORD]", invite.Password)
                .Replace("[EMAIL_ADDRESS]", invite.EmailAddress);

            SendEmails(new MailMessage[] { email });
        }

        /// <summary>
        /// Sends an update email to the specified invite
        /// </summary>
        /// <param name="invite">Invite the email is being sent to</param>
        protected static void SendUpdateInviteEmail(Invite invite)
        {
            MailMessage email = new MailMessage();
            email.To.Add(invite.EmailAddress);
            email.From = new MailAddress(MailFrom, "John Lace & Lizzie Toms");
            email.Subject = "The wedding of John & Lizzie";
            email.IsBodyHtml = true;

            email.Body = Resources.UpdateInviteEmail
                .Replace("[EMAIL_ADDRESS]", invite.EmailAddress);

            SendEmails(new MailMessage[] { email });
        }

        /// <summary>
        /// Sends the emails relating to the specified message to the specified recipient invite
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="message">Message the emails relate to</param>
        /// <param name="recipientInvite">Invite the messages are being sent to</param>
        protected static void SendMessageEmails(SqlConnection conn, Message message, Invite recipientInvite)
        {
            List<MailMessage> emails = new List<MailMessage>();
            
            switch (message.Sender)
            {
                case MessageSender.Admin:

                    if (recipientInvite.EmailMessages)
                    {
                        MailMessage email = new MailMessage();
                        email.To.Add(recipientInvite.EmailAddress);
                        email.From = new MailAddress(MailFrom, "John Lace & Lizzie Toms");
                        email.Subject = "John & Lizzie have sent you a message";
                        email.IsBodyHtml = true;

                        email.Body = Resources.MessageToInvite
                            .Replace("[INVITE_ID]", message.InviteId.ToString())
                            .Replace("[MESSAGE_BODY]", message.Body);

                        emails.Add(email);
                    }

                    break;

                case MessageSender.Invite:

                    Invite[] invites = Invite.GetInvites(conn);

                    foreach (Invite invite in invites.Where(p => p.IsAdmin && p.EmailMessages))
                    {
                        MailMessage email = new MailMessage();
                        email.To.Add(invite.EmailAddress);
                        email.From = new MailAddress(MailFrom, recipientInvite.EmailAddress);
                        email.Subject = "One of your guests has sent you a message";
                        email.IsBodyHtml = true;

                        email.Body = Resources.MessageFromInvite
                            .Replace("[INVITE_ID]", message.InviteId.ToString())
                            .Replace("[GUEST_LIST]", recipientInvite.GuestList)
                            .Replace("[MESSAGE_BODY]", message.Body);

                        emails.Add(email);
                    }

                    break;

                default:
                    throw new UnknownEnumValueException(message.Sender);
            }

            SendEmails(emails);
        }

        /// <summary>
        /// Sends the emails notifying invites that the gift website has been set up
        /// </summary>
        protected static void SendGiftWebsiteEmails()
        {
            List<MailMessage> emails = new List<MailMessage>();

            DatabaseAction(conn =>
            {
                foreach (Invite invite in Invite.GetInvites(conn).Where(p => p.NotifyGiftWebsite))
                {
                    MailMessage message = new MailMessage();
                    message.To.Add(invite.EmailAddress);
                    message.From = new MailAddress(MailFrom, "John Lace & Lizzie Toms");
                    message.Subject = "The wedding of John & Lizzie";
                    message.IsBodyHtml = true;

                    message.Body = Resources.GiftWebsiteEmail
                        .Replace("[GUEST_LIST]", invite.GuestListForenames);

                    emails.Add(message);
                }
            });

            SendEmails(emails);
        }

        /// <summary>
        /// Sends the specified emails using the SMTP settings in the web.config file
        /// </summary>
        /// <param name="emails">Emails being sent</param>
        /// <param name="displayName">Display name of the person(s) the emails are being sent from</param>
        private static void SendEmails(IEnumerable<MailMessage> emails)
        {
            SmtpClient client = new SmtpClient();
            client.Port = SmtpPort;
            client.Host = SmtpHost;
            client.EnableSsl = EnableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(MailFrom, SmtpPassword);

            foreach (MailMessage email in emails)
            {
                client.Send(email);
            }
        }

        #endregion

        #region Controller Members

        protected override void OnException(ExceptionContext filterContext)
        {
            DatabaseAction(conn =>
            {
                Error error = Error.FromException(filterContext.Exception);
                error.Save(conn);
            });

            filterContext.ExceptionHandled = true;
            filterContext.Result = Redirect("~/Error");
        }

        #endregion
    }
}
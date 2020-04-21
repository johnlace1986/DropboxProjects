using System;
using System.Linq;
using System.Web.Mvc;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.ActionConverters;

namespace Wedding.eVite.Web.Controllers.PageControllers
{
    [Authorize]
    public abstract class PageController : BaseController
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value determining whether or not the Is Admin property of the logged in invite should be overridden
        /// </summary>
        protected Boolean OverrideAdmin
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["OverrideAdmin"] == null)
                    System.Web.HttpContext.Current.Session["OverrideAdmin"] = false;

                return (Boolean)System.Web.HttpContext.Current.Session["OverrideAdmin"];
            }
            set
            {
                System.Web.HttpContext.Current.Session["OverrideAdmin"] = value;
            }
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Gets a value determining whether or not the page the controller is redirecting to is an admin page
        /// </summary>
        public abstract Boolean IsAdminPageController { get; }

        #endregion

        #region Static Methods
        /// <summary>
        /// Makes the specified string safe to print to a web page
        /// </summary>
        /// <param name="text">Text to be made safe</param>
        /// <returns>Text safe to print to a web page</returns>
        private static String MakeStringWebSafe(String text)
        {
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("\n", "<br/>");

            return text;
        }


        #endregion

        #region BaseController Members

        /// <summary>
        /// Creates a JsonResult object that serializes the specified object to JavaScript Object Notation (JSON) format.
        /// </summary>
        /// <returns>JSON result object that serializes the specified object to JSON format</returns>
        protected JsonResult Json()
        {
            return Json(new { });
        }

        /// <summary>
        /// Creates a JsonResult object that serializes the specified object to JavaScript Object Notation (JSON) format.
        /// </summary>
        /// <param name="data">Object being serialized</param>
        /// <returns>JSON result object that serializes the specified object to JSON format</returns>
        protected new JsonResult Json(object data)
        {
            JsonDateTimeResult result = new JsonDateTimeResult();
            result.Data = data;
            return result;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (LoggedInInvite == null)
            {
                filterContext.Result = Redirect("~/LogIn");
                return;
            }

            ViewBag.LoggedInInvite = LoggedInInvite;

            if (LoggedInInvite.HasChangedPassword)
            {
                if ((IsAdminPageController) && (!LoggedInInvite.IsAdmin))
                {
                    //this is an admin page but the logged in user isn't an admin user so they can't view it
                    //redirct to home page
                    filterContext.Result = Redirect("~");
                    return;
                }

                if ((!IsAdminPageController) && (LoggedInInvite.IsAdmin))
                {
                    if (!OverrideAdmin)
                    {
                        //this isn't an admin page but the logged user is an admin user so they can't view it
                        //redirect to admin home page
                        filterContext.Result = Redirect("~/Admin");
                        return;
                    }
                }
            }
            else
            {
                //user must change their password before viewing any other part of the website
                filterContext.Result = Redirect("~/ChangePassword");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        [HttpPost]
        public JsonResult GetInviteUnreadMessageCount(Int32 inviteId)
        {
            return DatabaseFunction<JsonResult>(conn =>
            {
                return Json(new { unreadMessageCount = Message.GetInviteUnreadMessageCount(conn, LoggedInInvite.Id, LoggedInInvite.IsAdmin) });
            });
        }

        [HttpPost]
        public JsonResult GetMessages(Int32 inviteId)
        {
            Message[] messages = DatabaseFunction<Message[]>(conn =>
            {
                Message[] allMessages = Message.GetMessagesByInviteId(conn, inviteId);
                MessageSender senderType = LoggedInInvite.IsAdmin ? MessageSender.Invite : MessageSender.Admin;

                foreach (Message message in allMessages.Where(p => p.Sender == senderType && !p.Read))
                {
                    message.Read = true;
                    message.Save(conn);
                }

                return allMessages;
            });

            return Json(messages);
        }

        [HttpPost]
        public ActionResult GetUnreadMessages(Int32 inviteId)
        {
            Message[] messages = DatabaseFunction<Message[]>(conn =>
            {
                return Message.GetUnreadMessagesByInviteId(conn, inviteId, LoggedInInvite.IsAdmin);
            });

            return Json(messages);
        }

        [HttpPost]
        public ActionResult SendMessageFromInvite(Int32 inviteId, String body)
        {
            Message message = new Message();
            message.InviteId = inviteId;
            message.Body = MakeStringWebSafe(body);
            message.Sender = LoggedInInvite.IsAdmin ? MessageSender.Admin : MessageSender.Invite;
            message.Read = false;

            DatabaseAction(conn =>
            {
                message.Save(conn);

                Invite recipientInvite = new Invite(conn, message.InviteId);
                SendMessageEmails(conn, message, recipientInvite);
            });

            return Json(message);
        }

        [HttpPost]
        public ActionResult SetInviteEmailMessages(Boolean emailMessages)
        {
            LoggedInInvite.EmailMessages = emailMessages;

            DatabaseAction(conn =>
            {
                LoggedInInvite.Save(conn);
            });

            return Json();
        }

        #endregion
    }
}
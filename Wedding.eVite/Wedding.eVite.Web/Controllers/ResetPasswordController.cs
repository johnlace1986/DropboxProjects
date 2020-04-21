using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.PageControllers;
using Wedding.eVite.Web.Models;

namespace Wedding.eVite.Web.Controllers
{
    public class ResetPasswordController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return View(new ResetPasswordModel()
            {
                EmailAddress = String.Empty
            });
        }

        [HttpPost]
        public ActionResult Index(ResetPasswordModel model)
        {
            if (String.IsNullOrEmpty(model.EmailAddress))
            {
                ViewBag.ErrorMessage = "Please enter an email address";
                return View();
            }

            return DatabaseFunction<ActionResult>(conn =>
            {
                Invite[] invites = Invite.GetInvites(conn);

                Invite loggedInInvite = invites.SingleOrDefault(p => p.EmailAddress.ToLower() == model.EmailAddress.Trim().ToLower());

                if (loggedInInvite == null)
                {
                    ViewBag.ErrorMessage = "Email address not found";
                    return View();
                }

                loggedInInvite.ResetPassword();
                loggedInInvite.Save(conn);

                SendInviteEmail(loggedInInvite);

                return RedirectToAction("Index", "LogIn");
            });
        }

        #endregion
    }
}

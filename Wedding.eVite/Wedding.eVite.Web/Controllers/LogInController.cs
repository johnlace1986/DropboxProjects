using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utilities.Data;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.PageControllers;
using Wedding.eVite.Web.Models;

namespace Wedding.eVite.Web.Controllers
{
    public class LogInController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
#if DEBUG
            String username = "lace.john@gmail.com";
#else
            String username = String.Empty;            
#endif
            if (!String.IsNullOrEmpty(LastUsername))
                username = LastUsername;

            return View(new LoginViewModel()
            {
                Username = username,
                RememberMe = true
            });
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (String.IsNullOrEmpty(model.Username))
            {
                ViewBag.ErrorMessage = "Please enter an email address";
                return View();
            }

            Invite[] invites = DatabaseFunction<Invite[]>(conn =>
            {
                return Invite.GetInvites(conn);
            });

            Invite loggedInInvite = invites.SingleOrDefault(p => p.EmailAddress.ToLower() == model.Username.Trim().ToLower() && p.CheckPassword(model.Password));

            if (loggedInInvite == null)
            {
                ViewBag.ErrorMessage = "Email address and password not found";
                return View();
            }

            FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
            LoggedInInvite = loggedInInvite;
            LastUsername = LoggedInInvite.EmailAddress;

            UserPageController.LayoutImagesLoaded = false;

            String returnUrl = Request.QueryString["ReturnUrl"];

            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Default");

            return Redirect(returnUrl);
        }

        public ActionResult LogOut()
        {
            LoggedInInvite = null;
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }

        #endregion
    }
}

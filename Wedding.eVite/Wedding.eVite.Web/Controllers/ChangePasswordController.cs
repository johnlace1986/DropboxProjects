using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities.Data;
using Wedding.eVite.Web.Controllers.PageControllers;
using Wedding.eVite.Web.Models;

namespace Wedding.eVite.Web.Controllers
{
    [Authorize]
    public class ChangePasswordController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ChangePasswordModel model)
        {
            if (model.Password != model.ReEnteredPassword)
            {
                ViewBag.ErrorMessage = "Passwords to not match";
                return View();
            }

            DatabaseAction(conn =>
            {
                LoggedInInvite.ChangePassword(conn, model.Password);
            });

            return Redirect("~");
        }

        #endregion

        #region BaseController Members

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserPageController.LayoutImagesLoaded = false;

            base.OnActionExecuting(filterContext);
        }

        #endregion
    }
}

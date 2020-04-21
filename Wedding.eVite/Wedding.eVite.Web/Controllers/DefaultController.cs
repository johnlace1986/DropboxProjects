using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class DefaultController : UserPageController
    {
        #region Actions

        public ActionResult Index()
        {
            if (DateTime.Now < new DateTime(2016, 1, 1))
                ViewBag.WeLookForwardToSeeingYou = "next year";
            else if (DateTime.Now < new DateTime(2016, 7, 1))
                ViewBag.WeLookForwardToSeeingYou = "in July";
            else
                ViewBag.WeLookForwardToSeeingYou = "on the 29th";

            return View();
        }

        #endregion
    }
}

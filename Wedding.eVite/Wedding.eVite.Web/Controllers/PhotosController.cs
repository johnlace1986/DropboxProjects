using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class PhotosController : UserPageController
    {
        #region Actions

        public ActionResult Index()
        {
            ViewBag.SlideshowImages =
                from String s in Directory.GetFiles(Server.MapPath("~/Content/Images/BarneySlideshow"), "*.png").OrderBy(p => p)
                select Path.GetFileName(s);

            return View();
        }

        #endregion
    }
}

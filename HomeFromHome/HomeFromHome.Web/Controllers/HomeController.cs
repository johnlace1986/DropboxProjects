using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeFromHome.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Prices()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult Testimonials()
        {
            return View();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class MessagesController : UserPageController
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities.Business;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class ErrorController : BaseController
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}

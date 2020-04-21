using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wedding.eVite.Web.Controllers.PageControllers
{
    public abstract class AdminPageController : PageController
    {
        #region PageController Members

        public override Boolean IsAdminPageController
        {
            get { return true; }
        }

        #endregion

        #region Actions
        
        public ActionResult ViewFullWebsite()
        {
            OverrideAdmin = true;
            return Redirect("~/");
        }

        #endregion
    }
}
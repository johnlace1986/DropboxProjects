using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class GiftController : UserPageController
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetNotifyGiftWebsite(Boolean notifyGiftWebsite)
        {
            LoggedInInvite.NotifyGiftWebsite = notifyGiftWebsite;

            DatabaseAction(conn =>
            {
                LoggedInInvite.Save(conn);
            });

            return Json();
        }

        #endregion
    }
}

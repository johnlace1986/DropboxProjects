using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class UnsubscribeController : BaseController
    {
        #region Actions

        public ActionResult Index(Int32 inviteId)
        {
            return View(DatabaseFunction<Invite>(conn =>
            {
                Invite invite = new Invite(conn, inviteId);
                invite.EmailMessages = false;
                invite.Save(conn);

                return invite;
            }));
        }

        #endregion
    }
}

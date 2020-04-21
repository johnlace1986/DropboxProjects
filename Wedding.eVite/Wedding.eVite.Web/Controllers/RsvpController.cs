using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities.Data;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class RsvpController : UserPageController
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetGuestAttending(Int32 guestId, Boolean attending)
        {
            Guest guest = LoggedInInvite.Guests.Single(p => p.Id == guestId);
            guest.IsAttending = attending;
            guest.DateOfRsvp = DateTime.Now;

            DatabaseAction(conn =>
            {
                LoggedInInvite.Save(conn);
            });

            return Json();
        }

        [HttpPost]
        public ActionResult SetGuestPlusOneDetails(Int32 guestId, Boolean isBringingPlusOne, String plusOneForename, String plusOneSurname)
        {
            Guest guest = LoggedInInvite.Guests.Single(p => p.Id == guestId);
            guest.IsBringingPlusOne = isBringingPlusOne;
            guest.PlusOneForename = plusOneForename;
            guest.PlusOneSurname = plusOneSurname;

            DatabaseAction(conn =>
            {
                LoggedInInvite.Save(conn);
            });

            return Json(new
            {
                plusOneFullName = guest.PlusOneFullName
            });
        }

        #endregion
    }
}

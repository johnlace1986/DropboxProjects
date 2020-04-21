using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.eVite.Business;
using Wedding.eVite.Web.Controllers.PageControllers;

namespace Wedding.eVite.Web.Controllers
{
    public class BigDayController : UserPageController
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetGuestVegetarian(Int32 guestId, Boolean isVegetarian)
        {
            Guest guest = LoggedInInvite.Guests.Single(p => p.Id == guestId);
            guest.IsVegetarian = isVegetarian;

            DatabaseAction(conn =>
            {
                LoggedInInvite.Save(conn);
            });

            return Json();
        }

        [HttpPost]
        public ActionResult SetGuestPlusOneVegetarian(Int32 guestId, Boolean isVegetarian)
        {
            Guest guest = LoggedInInvite.Guests.Single(p => p.Id == guestId);
            guest.PlusOneIsVegetarian = isVegetarian;

            DatabaseAction(conn =>
            {
                LoggedInInvite.Save(conn);
            });

            return Json();
        }

        [HttpPost]
        public ActionResult SetReserveSandholeRoom(Boolean reserveSandholeRoom)
        {
            LoggedInInvite.ReserveSandholeRoom = reserveSandholeRoom;

            DatabaseAction(conn =>
            {
                LoggedInInvite.Save(conn);
            });

            return Json();
        }

        #endregion
    }
}

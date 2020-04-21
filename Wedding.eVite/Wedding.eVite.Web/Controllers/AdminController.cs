using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wedding.eVite.Web.Controllers.PageControllers;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using Wedding.eVite.Web.Models;
using Wedding.eVite.Web.Content.AppCode;
using System.Xml;
using System.IO;

namespace Wedding.eVite.Web.Controllers
{
    public class AdminController : AdminPageController
    {
        #region Static Methods

        /// <summary>
        /// Gets the number of adult guests in the specified invites
        /// </summary>
        /// <param name="invites">Invites to search</param>
        /// <returns>Number of adult guests in the specified invites</returns>
        private static Int32 GetAdultGuests(IEnumerable<Business.Invite> invites)
        {
            return (from invite in invites.Where(p => p.IncludesCeremony)
                           from guest in invite.Guests.Where(g => g.IsAttending != false && !g.IsChild)
                           select guest).Count();
        }

        /// <summary>
        /// Gets the number of child guests in the specified invites
        /// </summary>
        /// <param name="invites">Invites to search</param>
        /// <returns>Number of child guests in the specified invites</returns>
        private static Int32 GetChildGuests(IEnumerable<Business.Invite> invites)
        {
            return (from invite in invites.Where(p => p.IncludesCeremony)
                    from guest in invite.Guests.Where(g => g.IsAttending != false && g.IsChild)
                    select guest).Count();
        }

        /// <summary>
        /// Gets the number of evening guests in the specified invites
        /// </summary>
        /// <param name="invites">Invites to search</param>
        /// <returns>Number of evening guests in the specified invites</returns>
        private static Int32 GetEveningGuests(IEnumerable<Business.Invite> invites)
        {
            return (from invite in invites
                           from guest in invite.Guests.Where(g => g.IsAttending != false)
                           select guest).Count();
        }


        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View(DatabaseFunction<AdminInviteController[]>(conn =>
            {
                List<AdminInviteController> invites = new List<AdminInviteController>();

                foreach (Business.Invite invite in Business.Invite.GetInvites(conn))
                {
                    Int32 unreadMessageCount = invite.GetUnreadMessagesCount(conn, true);

                    invites.Add(new AdminInviteController()
                    {
                        Invite = invite,
                        UnreadMessageCount = unreadMessageCount
                    });
                }

                invites.Sort();
                return invites.ToArray();
            }));
        }

        [HttpPost]
        public ActionResult GetInvitesSummary()
        {
            IEnumerable<Business.Invite> invites = DatabaseFunction<IEnumerable<Business.Invite>>(conn =>
            {
                return Business.Invite.GetInvites(conn);
            });

            return Json(new { 
                totalGuests = invites.Sum(invite => invite.Guests.Count()) + invites.Sum(invite => invite.Guests.Where(guest => guest.IsAttending == true && guest.CanBringPlusOne && guest.IsBringingPlusOne).Count()),
                guestsAttending = invites.Sum(invite => invite.Guests.Where(guest => guest.IsAttending == true).Count()) + invites.Sum(invite => invite.Guests.Where(guest => guest.IsAttending == true && guest.CanBringPlusOne && guest.IsBringingPlusOne).Count()),
                guestsNotAttending = invites.Sum(invite => invite.Guests.Where(guest => guest.IsAttending == false).Count()),
                guestsNoRSVP = invites.Sum(invite => invite.Guests.Where(guest => guest.IsAttending == null).Count())
            });
        }

        [HttpPost]
        public ActionResult SendInviteEmail(Int32 inviteId)
        {
            DatabaseAction(conn =>
            {
                Business.Invite invite = new Business.Invite(conn, inviteId);
                invite.ResetPassword();
                invite.Save(conn);

                SendInviteEmail(invite);
            });

            return Json();
        }

        [HttpPost]
        public ActionResult DeleteInvite(Int32 inviteId)
        {
            DatabaseAction(conn =>
            {
                Business.Invite invite = new Business.Invite(conn, inviteId);
                invite.Delete(conn);
            });

            return Json();
        }

        [HttpPost]
        public new ActionResult SendGiftWebsiteEmails()
        {
            BaseController.SendGiftWebsiteEmails();
            return Json();
        }

        public ActionResult Invite(Int32? inviteId, String returnAction)
        {
            Business.Invite invite;

            if (inviteId.HasValue)
            {
                invite = DatabaseFunction<Business.Invite>(conn =>
                {
                    return new Business.Invite(conn, inviteId.Value);
                });
            }
            else
                invite = new Business.Invite();

            return View(new AdminInviteModel(invite) { ReturnAction = returnAction });
        }

        [HttpPost]
        public ActionResult SaveInvite(Int32 inviteId, String emailAddress, Boolean includesCeremony, Boolean reserveSandholeRoom, Boolean isAdmin, String guests, InviteEmailType emailType)
        {
            Boolean result = true;
            String errorMessage = String.Empty;

            emailAddress = emailAddress.Trim();

            DatabaseAction(conn =>
            {
                if (!new EmailAddressAttribute().IsValid(emailAddress))
                {
                    result = false;
                    errorMessage = "Please enter a valid email address.";
                    return;
                }

                if (Business.Invite.GetInvites(conn).Any(p => p.Id != inviteId && p.EmailAddress.ToLower() == emailAddress.ToLower()))
                {
                    result = false;
                    errorMessage = "There is already another invite using that email address.";
                    return;
                }

                Business.Invite invite = new Business.Invite();

                if (inviteId != -1)
                    invite = new Business.Invite(conn, inviteId);
                
                invite.EmailAddress = emailAddress;
                invite.IncludesCeremony = includesCeremony;
                invite.ReserveSandholeRoom = reserveSandholeRoom;
                invite.IsAdmin = isAdmin;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<String, object>[] guestsData = (Dictionary<String, object>[])serializer.Deserialize(guests, typeof(Dictionary<String, object>[]));

                invite.Guests.Clear();

                foreach (Dictionary<String, object> guestData in guestsData)
                {
                    Int32 guestId = (Int32)guestData["guestId"];
                    String forename = (String)guestData["forename"];
                    String surname = (String)guestData["surname"];
                    Boolean? isAttending = (Boolean?)guestData["isAttending"];
                    DateTime? dateOfRsvp = guestData["dateOfRsvp"] == null ? (DateTime?)null : DateTime.Parse((String)guestData["dateOfRsvp"]);
                    Boolean isChild = (Boolean)guestData["isChild"];
                    Boolean isVegetarian = (Boolean)guestData["isVegetarian"];
                    Boolean canBringPlusOne = (Boolean)guestData["canBringPlusOne"];
                    Int32? tableId = guestData["tableId"] == null ? (Int32?)null : (Int32)guestData["tableId"];
                    Int32? roomId = guestData["roomId"] == null ? (Int32?)null : (Int32)guestData["roomId"];
                    String notes = (String)guestData["notes"];

                    Business.Guest guest = new Business.Guest(guestId, forename, surname, isAttending, dateOfRsvp, isChild, isVegetarian, canBringPlusOne, tableId, roomId, notes);
                    invite.Guests.Add(guest);
                }

                invite.Save(conn);

                switch(emailType)
                {
                    case InviteEmailType.Invite:
                        SendInviteEmail(invite);
                        break;

                    case InviteEmailType.Update:
                        SendUpdateInviteEmail(invite);
                        break;
                }                
            });

            return Json(new
            {
                result = result,
                errorMessage = errorMessage
            });
        }

        public ActionResult Messages(Int32 inviteId)
        {
            Business.Invite invite = DatabaseFunction<Business.Invite>(conn =>
            {
                return new Business.Invite(conn, inviteId);
            });

            return View(invite);
        }

        public ActionResult Guests(String rsvp = "all")
        {
            Business.Invite[] invites = DatabaseFunction<Business.Invite[]>(conn =>
            {
                return Business.Invite.GetInvites(conn);
            });

            return View(
                new AdminGuestsModel()
                {
                    RSVP = rsvp,
                    Guests = invites.SelectMany(invite => invite.Guests.Select(guest => new AdminGuestModel() { Invite = invite, Guest = guest })).OrderBy(model => model.Guest.Surname)
                });
        }

        public ActionResult ExportGuests()
        {
            XmlDocument document = new XmlDocument();

            XmlElement invites = document.CreateElement("INVITES");
            document.AppendChild(invites);

            foreach (Business.Invite invite in DatabaseFunction<Business.Invite[]>(conn => Business.Invite.GetInvites(conn)))
            {
                invites.AppendChild(invite.ToXml(document));
            }

            MemoryStream ms = new MemoryStream();

            StreamWriter writer = new StreamWriter(ms);
            writer.Write(document.OuterXml);

            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            return File(ms, "text/xml", "Invites.xml");
        }

        public ActionResult RsvpGuests(Dictionary<String, object>[] guests, Boolean isAttending)
        {
            DateTime dateOfRsvp = DateTime.Now;

            DatabaseAction(conn =>
            {
                List<Business.Invite> invites = new List<Business.Invite>();

                foreach (Dictionary<String, object> guestData in guests)
                {
                    Int32 inviteId = (Int32)guestData["inviteId"];
                    Int32 guestId = (Int32)guestData["guestId"];

                    Business.Invite invite = invites.SingleOrDefault(p => p.Id == inviteId);

                    if (invite == null)
                    {
                        invite = new Business.Invite(conn, inviteId);
                        invites.Add(invite);
                    }

                    Business.Guest guest = invite.Guests.Single(p => p.Id == guestId);
                    guest.IsAttending = isAttending;
                    guest.DateOfRsvp = dateOfRsvp;
                }

                foreach (Business.Invite invite in invites)
                    invite.Save(conn);
            });

            return Json(dateOfRsvp);
        }

        public ActionResult SeatingPlan()
        {
            AdminSeatingPlanModel model = DatabaseFunction<AdminSeatingPlanModel>(conn =>
            {
                return new AdminSeatingPlanModel()
                {
                    Tables = Business.Table.GetTables(conn),
                    UnassignedGuests = Business.Guest.GetUnassignedTableGuests(conn)
                };
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult AddNewTable()
        {
            Business.Table table = new Business.Table();

            DatabaseAction(conn =>
            {
                table.Save(conn);
            });

            return Json(table.Id);
        }

        [HttpPost]
        public ActionResult DeleteTable(Int32 tableId)
        {
            DatabaseAction(conn =>
            {
                Business.Table table = new Business.Table(conn, tableId);
                table.Delete(conn);
            });

            return Json();
        }

        [HttpPost]
        public ActionResult SetGuestTableId(Int32 guestId, Int32? tableId)
        {
            DatabaseAction(conn =>
            {
                Business.Guest.SetGuestTableId(conn, guestId, tableId);
            });

            return Json();
        }

        public ActionResult Expenses()
        {
            Business.Expense[] expenses = DatabaseFunction<Business.Expense[]>(conn =>
            {
                return Business.Expense.GetExpenses(conn);
            });

            return View(expenses);
        }

        [HttpPost]
        public ActionResult LoadExpensesTotals()
        {
            return DatabaseFunction<JsonResult>(conn =>
            {
                Business.Invite[] invites = Business.Invite.GetInvites(conn);
                Business.Expense[] expenses = Business.Expense.GetExpenses(conn);
                
                Decimal total = expenses.Sum(expense => expense.Cost);

                Decimal paid = expenses.Sum(expense => expense.Paid);
                Decimal totalToPay = total - paid;

                return Json(new
                {
                    total = total,
                    paid = paid,
                    totalToPay = totalToPay
                });
            });
        }

        [HttpPost]
        public ActionResult SaveExpense(Int32 expenseId, String name, Decimal cost, Decimal paid)
        {
            Int32 newExpenseId = DatabaseFunction<Int32>(conn =>
            {
                Business.Expense expense;

                if (expenseId == -1)
                    expense = new Business.Expense();
                else
                    expense = new Business.Expense(conn, expenseId);

                expense.Name = name;
                expense.Cost = cost;
                expense.Paid = paid;

                expense.Save(conn);
                return expense.Id;
            });

            return Json(newExpenseId);
        }

        [HttpPost]
        public ActionResult DeleteExpense(Int32 expenseId)
        {
            DatabaseAction(conn =>
            {
                Business.Expense expense = new Business.Expense(conn, expenseId);
                expense.Delete(conn);
            });

            return Json();
        }

        public ActionResult Accommodation()
        {
            AdminAccommodationModel model = DatabaseFunction<AdminAccommodationModel>(conn =>
            {
                return new AdminAccommodationModel()
                {
                    Rooms = Business.Room.GetRooms(conn),
                    UnassignedGuests = Business.Guest.GetUnassignedRoomGuests(conn)
                };
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveRoom(Int32 roomId, String name, Int32 beds)
        {
            Int32 newRoomId = DatabaseFunction<Int32>(conn =>
            {
                Business.Room room;

                if (roomId == -1)
                    room = new Business.Room();
                else
                    room = new Business.Room(conn, roomId);

                room.Name = name;
                room.Beds = beds;

                room.Save(conn);
                return room.Id;
            });

            return Json(newRoomId);
        }

        [HttpPost]
        public ActionResult DeleteRoom(Int32 roomId)
        {
            DatabaseAction(conn =>
            {
                Business.Room room = new Business.Room(conn, roomId);
                room.Delete(conn);
            });

            return Json();
        }

        [HttpPost]
        public ActionResult SetGuestRoomId(Int32 guestId, Int32? roomId)
        {
            DatabaseAction(conn =>
            {
                Business.Guest.SetGuestRoomId(conn, guestId, roomId);
            });

            return Json();
        }

        public ActionResult Errors()
        {
            Business.Error[] errors = DatabaseFunction<Business.Error[]>(conn =>
            {
                return Business.Error.GetErrors(conn);
            });

            return View(errors);
        }

        [HttpPost]
        public ActionResult DeleteError(Int32 errorId)
        {
            DatabaseAction(conn =>
            {
                Business.Error error = new Business.Error(conn, errorId);
                error.Delete(conn);
            });
            
            return Json();
        }

        #endregion
    }
}

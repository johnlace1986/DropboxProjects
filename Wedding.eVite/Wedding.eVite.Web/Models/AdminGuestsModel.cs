using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wedding.eVite.Web.Models
{
    public class AdminGuestsModel
    {
        #region Properties

        public String RSVP { get; set; }

        public IEnumerable<AdminGuestModel> Guests { get; set; }

        #endregion
    }
}
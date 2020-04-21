using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wedding.eVite.Business;

namespace Wedding.eVite.Web.Models
{
    public class AdminGuestModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the invite the guest belongs to
        /// </summary>
        public Invite Invite { get; set; }

        /// <summary>
        /// Gets or sets the guest
        /// </summary>
        public Guest Guest { get; set; }

        #endregion
    }
}
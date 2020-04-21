using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wedding.eVite.Business;

namespace Wedding.eVite.Web.Models
{
    public class AdminInviteModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the invite being edited
        /// </summary>
        public Invite Invite { get; set; }

        /// <summary>
        /// Gets or sets the action to return to after the user has finished editing the invite
        /// </summary>
        public String ReturnAction { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the AdminInviteModel class
        /// </summary>
        /// <param name="invite">Invite being edited</param>
        public AdminInviteModel(Invite invite)
        {
            Invite = invite;
            ReturnAction = String.Empty;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wedding.eVite.Business;

namespace Wedding.eVite.Web.Models
{
    public class AdminInviteController : IComparable<AdminInviteController>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the invite
        /// </summary>
        public Invite Invite { get; set; }

        /// <summary>
        /// Gets or sets the amount of unread messages that have been sent by the invite
        /// </summary>
        public Int32 UnreadMessageCount { get; set; }

        #endregion

        #region IComparable<AdminInviteController> Members

        public int CompareTo(AdminInviteController other)
        {
            if (other.Invite.IsAdmin == Invite.IsAdmin)
            {
                if (other.UnreadMessageCount == UnreadMessageCount)
                    return Invite.CompareTo(other.Invite);
                else
                    return other.UnreadMessageCount.CompareTo(UnreadMessageCount);
            }
            else
                return other.Invite.IsAdmin.CompareTo(Invite.IsAdmin);
        }

        #endregion
    }
}
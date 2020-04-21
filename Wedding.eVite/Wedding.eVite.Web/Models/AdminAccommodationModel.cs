using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wedding.eVite.Business;

namespace Wedding.eVite.Web.Models
{
    public class AdminAccommodationModel
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the rooms currently in the system
        /// </summary>
        public Room[] Rooms { get; set; }

        /// <summary>
        /// Gets or sets the guests that have yet to assigned a room
        /// </summary>
        public Guest[] UnassignedGuests { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wedding.eVite.Business;

namespace Wedding.eVite.Web.Models
{
    public class AdminSeatingPlanModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the tables in the seating plan
        /// </summary>
        public Table[] Tables { get; set; }

        /// <summary>
        /// Gets or sets the guests that have yet to assigned a table
        /// </summary>
        public Guest[] UnassignedGuests { get; set; }

        #endregion
    }
}
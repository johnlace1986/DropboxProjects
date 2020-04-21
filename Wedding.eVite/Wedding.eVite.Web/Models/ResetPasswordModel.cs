using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wedding.eVite.Web.Models
{
    public class ResetPasswordModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the email address of the invite having it's password reset
        /// </summary>
        public String EmailAddress { get; set; }

        #endregion
    }
}
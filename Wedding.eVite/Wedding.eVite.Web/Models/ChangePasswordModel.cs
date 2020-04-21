using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wedding.eVite.Web.Models
{
    public class ChangePasswordModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the new password
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Gets or sets the rentry of the password
        /// </summary>
        public String ReEnteredPassword { get; set; }

        #endregion
    }
}
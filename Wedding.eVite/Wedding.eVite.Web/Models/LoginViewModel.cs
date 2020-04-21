using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wedding.eVite.Web.Models
{
    public class LoginViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Gets or sets a value determining whether or not the authentication cookie should be persistant
        /// </summary>
        public Boolean RememberMe { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the LoginViewModel class
        /// </summary>
        public LoginViewModel()
        {
            Username = String.Empty;
            Password = String.Empty;
        }

        #endregion
    }
}
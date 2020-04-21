using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Business
{
    public class User : DbObject
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public Guid UserId { get; internal set; }

        /// <summary>
        /// Christian name for the user
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Family name for the user
        /// </summary>
        public String Surname { get; set; }

        /// <summary>
        /// Full name for the user
        /// </summary>
        public String FullName
        {
            get { return (FirstName + " " + Surname).Trim(); }
        }

        /// <summary>
        /// Contact email address for the user
        /// </summary>
        public String EmailAddress { get; set; }

        /// <summary>
        /// Password the user uses to log in
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Determines whether the user has administrator access rights
        /// </summary>
        public Boolean Admin { get; internal set; }

        /// <summary>
        /// Date the user's account was created
        /// </summary>
        public DateTime DateAdded { get; internal set; }

        /// <summary>
        /// List of albums that belong to the user
        /// </summary>
        public CustomerAlbum[] CustomerAlbums { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
            UserId = Guid.NewGuid();
            FirstName = String.Empty;
            Surname = String.Empty;
            EmailAddress = String.Empty;
            Password = String.Empty;
            Admin = false;
            DateAdded = DateTime.Now;
            CustomerAlbums = new CustomerAlbum[0];

            IsInDatabase = false;
        }

        /// <summary>
        /// Loads a new instance of the User class from the database
        /// </summary>
        /// <param name="userId">Unique identifier of the user</param>
        public User(Guid userId)
            : this(Data.User.GetUserByUserId(userId))
        {
        }

        /// <summary>
        /// Loads a new instance of the User class from the database
        /// </summary>
        /// <param name="emailAddress">Email address of the user being loaded</param>
        public User(String emailAddress)
            : this(Data.User.GetUserByEmailAddress(emailAddress))
        {
        }

        /// <summary>
        /// Loads a new instance of the User class from the database
        /// </summary>
        /// <param name="clone">User object loaded from the database</param>
        private User(User clone)
        {
            UserId = clone.UserId;
            FirstName = clone.FirstName;
            Surname = clone.Surname;
            EmailAddress = clone.EmailAddress;
            Password = clone.Password;
            Admin = clone.Admin;
            DateAdded = clone.DateAdded;
            CustomerAlbums = clone.CustomerAlbums;

            IsInDatabase = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds an album to the albums array
        /// </summary>
        /// <param name="customerAlbum">Album being added</param>
        internal void AddAlbum(CustomerAlbum customerAlbum)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot add an album to a user that does not exist in the database");

            if (!(customerAlbum.IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot add an ablum that does not exist in the database to a user");

            if (customerAlbum.customerId != UserId)
                throw new Exception.ChildBelongsToDifferentParentException("Cannot add the album to the user. The album belongs to another user");

            List<CustomerAlbum> albums = new List<CustomerAlbum>();

            foreach (CustomerAlbum album in CustomerAlbums)
            {
                if (album.Id == customerAlbum.Id)
                    throw new Exception.ChildAlreadyBelongsToParentException("The user already contains the album");

                albums.Add(album);
            }

            albums.Add(customerAlbum);
            CustomerAlbums = albums.ToArray();
        }

        /// <summary>
        /// Removes an album from the albums array
        /// </summary>
        /// <param name="customerAlbum">Album being removed</param>
        internal void RemoveAlbum(CustomerAlbum customerAlbum)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot remove an album from a user that does not exist in the database");

            if (!(customerAlbum.IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot remove an ablum that does not exist in the database from a user");

            if (customerAlbum.customerId != UserId)
                throw new Exception.ChildBelongsToDifferentParentException("Cannot remove the album from the user. The album belongs to another user");

            List<CustomerAlbum> albums = new List<CustomerAlbum>();
            Boolean blnFound = false;

            foreach (CustomerAlbum album in CustomerAlbums)
            {
                if (album.Id == customerAlbum.Id)
                    blnFound = true;
                else
                    albums.Add(album);
            }

            if (!(blnFound))
                throw new Exception.ChildDoesNotBelongToParentException("The user does not own the album");

            CustomerAlbums = albums.ToArray();
        }

        /// <summary>
        /// Gets a customer album with the specified unique identifier
        /// </summary>
        /// <param name="customerAlbumId">Unique identifier of the desired album</param>
        /// <returns>Album with the specified unique identifier</returns>
        public CustomerAlbum GetCustomerAlbumById(Int16 customerAlbumId)
        {
            foreach (CustomerAlbum album in CustomerAlbums)
                if (album.Id == customerAlbumId)
                    return album;

            throw new IndexOutOfRangeException("The user does not contain an album with the Id number \"" + customerAlbumId.ToString() + "\"");
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns all the users in the database
        /// </summary>
        /// <returns>Array containing all users in the database</returns>
        public static User[] GetUsers()
        {
            return Data.User.GetUsers();
        }

        /// <summary>
        /// Determines whether there is currently a user in the database that has a specified email address
        /// </summary>
        /// <param name="emailAddress">Email address to look for in the database</param>
        /// <returns>True if the email address exists, false if not</returns>
        public static Boolean EmailAddressExists(string emailAddress)
        {
            return Data.User.EmailAddressExists(emailAddress);
        }

        #endregion

        #region DbObject Members

        /// <summary>
        /// Executes the stored procedure that adds the object to the database
        /// </summary>
        protected override void ExecuteAddStoredProcedure()
        {
            Data.User.AddUser(this);
        }

        /// <summary>
        /// Executes the stored procedure that updates the row in the database representing the object
        /// </summary>
        protected override void ExecuteUpdateStoredProcedure()
        {
            Data.User.UpdateUser(this);
        }

        /// <summary>
        /// Executes the stored procedure that deletes the row from the database representing the object
        /// </summary>
        protected override void ExecuteDeleteStoredProcedure()
        {
            Data.User.DeleteUser(UserId);
        }

        /// <summary>
        /// Determines whether the current properties for the object are valid
        /// </summary>
        /// <param name="error">Exception thrown if property is invalid</param>
        /// <returns>True if properties are valid, false if not</returns>
        protected override bool ValidateProperties(out Exception.InvalidPropertyException error)
        {
            error = null;

            if (String.IsNullOrEmpty(FirstName))
                error = new Exception.InvalidPropertyException("User.FirstName cannot be empty");

            if (String.IsNullOrEmpty(Surname))
                error = new Exception.InvalidPropertyException("User.Surname cannot be empty");

            if (String.IsNullOrEmpty(EmailAddress))
                error = new Exception.InvalidPropertyException("User.EmailAddress cannot be empty");

            try
            {
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(EmailAddress);
            }
            catch (System.Exception e)
            {
                error = new Exception.InvalidPropertyException("Invalid email address", e);
            }

            if (EmailAddressExists(EmailAddress))
            {
                if (IsInDatabase)
                {
                    User clone = new User(EmailAddress);

                    if (clone.UserId != UserId)
                        error = new Exception.InvalidPropertyException("EmailAddress already exists");
                }
                else
                    error = new Exception.InvalidPropertyException("EmailAddress already exists");
            }
           
            if (String.IsNullOrEmpty(Password))
                error = new Exception.InvalidPropertyException("User.Password cannot be empty");

            return (error == null);
        }

        /// <summary>
        /// Executes the stored procedure that deletes the row from the database representing the object
        /// </summary>
        protected override void ResetIdToDefaultValue()
        {
            //do nothing
        }

        internal override Data.StoredProcedureParameter[] GetParametersForStoredProcedure(bool updatedStoredProc)
        {
            List<Data.StoredProcedureParameter> parameters = new List<Data.StoredProcedureParameter>();

            if (!(updatedStoredProc))
                parameters.Add(Data.Control.GetParameter("DateAdded", DbType.DateTime, DateAdded));

            parameters.Add(Data.Control.GetParameter("UserId", DbType.Guid, UserId));
            parameters.Add(Data.Control.GetParameter("FirstName", DbType.String, FirstName));
            parameters.Add(Data.Control.GetParameter("Surname", DbType.String, Surname));
            parameters.Add(Data.Control.GetParameter("EmailAddress", DbType.String, EmailAddress));
            parameters.Add(Data.Control.GetParameter("Password", DbType.String, Password));

            return parameters.ToArray();
        }
        
        #endregion
    }
}

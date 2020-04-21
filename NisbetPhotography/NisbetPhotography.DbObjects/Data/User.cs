using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Data
{
    internal static class User
    {
        /// <summary>
        /// Creates a new Business.User object from the data in the DataTableReader
        /// </summary>
        /// <param name="dtr">DataTableReader containing the User's data</param>
        /// <returns>Business.User object</returns>
        private static Business.User GetUserFromDataTableReader(DataTableReader dtr)
        {
            Business.User user = new Business.User();
            user.UserId = (Guid)dtr["UserId"];
            user.FirstName = (String)dtr["FirstName"];
            user.Surname = (String)dtr["Surname"];
            user.EmailAddress = (String)dtr["EmailAddress"];
            user.Password = (String)dtr["Password"];
            user.Admin = (Boolean)dtr["Admin"];
            user.DateAdded = (DateTime)dtr["DateAdded"];
            user.CustomerAlbums = Data.CustomerAlbum.GetCustomerAlbumsByCustomer(user);
            user.IsInDatabase = true;

            return user;
        }

        /// <summary>
        /// Returns all the users in the database
        /// </summary>
        /// <returns>Array containing all users in the database</returns>
        public static Business.User[] GetUsers()
        {
            List<Business.User> users = new List<Business.User>();

            using (DataTableReader dtr = Control.ExecuteReader("GetUsers"))
            {
                while (dtr.Read())
                {
                    users.Add(GetUserFromDataTableReader(dtr));
                }
            }

            return users.ToArray();
        }

        /// <summary>
        /// Loads a specific user from the database
        /// </summary>
        /// <param name="userId">Unique identifier of the user</param>
        /// <returns>User object that has the specified user Id</returns>
        public static Business.User GetUserByUserId(Guid userId)
        {
            using (DataTableReader dtr = Control.ExecuteReader("GetUserByUserId", Control.GetParameter("UserId", DbType.Guid, userId)))
            {
                if (dtr.Read())
                    return GetUserFromDataTableReader(dtr);
                else
                    throw new Exception.SpecifiedDbObjectNotFoundException("No user exists in the database with the user Id \"" + userId.ToString() + "\"");
            }
        }

        /// <summary>
        /// Loads a new instance of the User class from the database
        /// </summary>
        /// <param name="emailAddress">Email address of the user being loaded</param>
        /// <returns>User object that has the specified email address</returns>
        public static Business.User GetUserByEmailAddress(String emailAddress)
        {
            using (DataTableReader dtr = Control.ExecuteReader("GetUserByEmailAddress", Control.GetParameter("EmailAddress", DbType.String, emailAddress)))
            {
                if (dtr.Read())
                    return GetUserFromDataTableReader(dtr);
                else
                    throw new Exception.SpecifiedDbObjectNotFoundException("No user exists in the database with the email address \"" + emailAddress + "\"");
            }
        }

        /// <summary>
        /// Adds the user's details to the database
        /// </summary>
        /// <param name="user">User object being added to the database</param>
        public static void AddUser(Business.User user)
        {
            try
            {
                Control.ExecuteNonQuery("AddUser", user.GetParametersForStoredProcedure(false));
            }
            catch (System.Exception e)
            {
                throw new Exception.AddDbObjectException("Could not add user to the database", e);
            }
        }

        /// <summary>
        /// Updates the user's data in the database
        /// </summary>
        /// <param name="user">User object being updated in the database</param>
        public static void UpdateUser(Business.User user)
        {
            try
            {
                Control.ExecuteNonQuery("UpdateUser", user.GetParametersForStoredProcedure(true));
            }
            catch (System.Exception e)
            {
                throw new Exception.UpdateDbObjectException("Could not update user in the database", e);            }
        }

        /// <summary>
        /// Deletes the specified user from the database
        /// </summary>
        /// <param name="userId">Unique identifier of the user being deleted</param>
        public static void DeleteUser(Guid userId)
        {
            try
            {
                Control.ExecuteNonQuery("DeleteUser", Control.GetParameter("UserId", DbType.Guid, userId));
            }
            catch (System.Exception e)
            {
                throw new Exception.DeleteDbObjectException("Could not delete user", e);
            }
        }

        /// <summary>
        /// Determines whether there is currently a user in the database that has a specified email address
        /// </summary>
        /// <param name="emailAddress">Email address to look for in the database</param>
        /// <returns>True if the email address exists, false if not</returns>
        public static Boolean EmailAddressExists(String emailAddress)
        {
            return (Boolean)Control.ExecuteScalar("EmailAddressExists", Control.GetParameter("EmailAddress", DbType.String, emailAddress));
        }
    }
}

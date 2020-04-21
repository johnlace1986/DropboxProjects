using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Utilities.Data;

namespace Utilities.EventArgs
{
    public delegate void ObjectSavedEventHandler<T>(object sender, DbObjectSavedEventArgs<T> e);

    public class DbObjectSavedEventArgs<T> : System.EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets the object that was saved
        /// </summary>
        public DbObject<T> SavedObject { get; private set; }

        /// <summary>
        /// Gets or sets an open connection to the database
        /// </summary>
        public DbConnection Connection { get; private set; }

        /// <summary>
        /// Gets or sets a value determining whether or not the object was already in the database prior to saving
        /// </summary>
        public Boolean WasInDatabase { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ObjectSavedEventArgs class
        /// </summary>
        /// <param name="savedObject">The object that was saved</param>
        /// <param name="connection">Open connection to the database</param>
        /// <param name="wasInDatabase">Value determining whether or not the object was already in the database prior to saving</param>
        public DbObjectSavedEventArgs(DbObject<T> savedObject, DbConnection connection, Boolean wasInDatabase)
        {
            SavedObject = savedObject;
            Connection = connection;
            WasInDatabase = wasInDatabase;
        }

        #endregion
    }
}

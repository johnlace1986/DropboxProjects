using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Utilities.Data;

namespace Utilities.EventArgs
{
    public delegate void ObjectDeletedEventHanlder<T>(object sender, DbObjectDeletedEventArgs<T> e);

    public class DbObjectDeletedEventArgs<T> : System.EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets the object that was deleted
        /// </summary>
        public DbObject<T> DeletedObject { get; private set; }

        /// <summary>
        /// Gets or sets an open connection to the database
        /// </summary>
        public DbConnection Connection { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DbObjectDeletedEventArgs class
        /// </summary>
        /// <param name="deletedObject">The object that was deleted</param>
        /// <param name="connection">Open connection to the database</param>
        public DbObjectDeletedEventArgs(DbObject<T> deletedObject, DbConnection connection)
        {
            DeletedObject = deletedObject;
            Connection = connection;
        }

        #endregion
    }
}

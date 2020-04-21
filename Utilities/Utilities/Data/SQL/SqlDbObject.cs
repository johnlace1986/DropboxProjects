using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Utilities.EventArgs;
using Utilities.Business;
using System.Data.Common;
using sp = Utilities.Data.StoredProcedure;

namespace Utilities.Data.SQL
{
    [Serializable]
    public abstract class SqlDbObject<T> : DbObject<T>
    {        
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SqlDbObject class
        /// </summary>
        protected SqlDbObject()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the SqlDbObject class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="id">Unique identifier of the object</param>
        protected SqlDbObject(SqlConnection conn, T id)
            : base(conn, id)
        {

        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Loads the object from the database using the specified ID
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="id">Unique identifier of the object</param>
        protected virtual void Load(SqlConnection conn, T id)
        {
        }

        /// <summary>
        /// Loads the values in the in the data row into the object
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="row">Data row containing the data for the object</param>
        protected virtual void Load(SqlConnection conn, sp.DataRow row)
        {
            LoadPropertiesFromDataRow(conn, row);
        }
        
        /// <summary>
        /// Determines whether or not the properties currently set to the object are valid for the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="exception">Null if the properties are valid, otherwise set to the reason they are not</param>
        /// <returns>True if the properties of the object are valid, false if not</returns>
        protected virtual Boolean ValidateProperties(SqlConnection conn, out System.Exception exception)
        {
            exception = null;
            return true;
        }

        /// <summary>
        /// Adds the object to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        protected virtual void AddToDatabase(SqlConnection conn, String commandText)
        {            
            base.AddToDatabase(conn, commandText);
        }

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        protected virtual void UpdateInDatabase(SqlConnection conn, String commandText)
        {
            base.UpdateInDatabase(conn, commandText);
        }

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        protected virtual void DeleteFromDatabase(SqlConnection conn, String commandText)
        {
            base.DeleteFromDatabase(conn, commandText);
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Adds the object to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void AddToDatabase(SqlConnection conn);

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void UpdateInDatabase(SqlConnection conn);

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void DeleteFromDatabase(SqlConnection conn);

        #endregion

        #region DbObject Members

        internal override DbCommand GetCommand(DbConnection conn, String commandText)
        {
            return new SqlCommand(commandText, (SqlConnection)conn);
        }

        internal override void Load(DbConnection conn, T id)
        {
            this.Load((SqlConnection)conn, id);
        }

        internal override void Load(DbConnection conn, sp.DataRow row)
        {
            this.Load((SqlConnection)conn, row);
        }

        internal override Boolean ValidateProperties(DbConnection conn, out System.Exception exception)
        {
            return this.ValidateProperties((SqlConnection)conn, out exception);
        }

        internal override void AddToDatabase(DbConnection conn)
        {
            this.AddToDatabase((SqlConnection)conn);
        }

        internal override void UpdateInDatabase(DbConnection conn)
        {
            this.UpdateInDatabase((SqlConnection)conn);
        }

        internal override void DeleteFromDatabase(DbConnection conn)
        {
            this.DeleteFromDatabase((SqlConnection)conn);
        }

        internal override void AddToDatabase(DbConnection conn, String commandText)
        {
            this.AddToDatabase((SqlConnection)conn, commandText);
        }

        internal override void UpdateInDatabase(DbConnection conn, String commandText)
        {
            this.UpdateInDatabase((SqlConnection)conn, commandText);
        }

        internal override void DeleteFromDatabase(DbConnection conn, String commandText)
        {
            this.DeleteFromDatabase((SqlConnection)conn, commandText);
        }

        #endregion
    }
}

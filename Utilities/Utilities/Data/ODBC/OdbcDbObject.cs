using System;
using System.Data.Odbc;
using System.Data.Common;
using sp = Utilities.Data.StoredProcedure;

namespace Utilities.Data.ODBC
{
    [Serializable]
    public abstract class OdbcDbObject<T> : DbObject<T>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OdbcDbObject class
        /// </summary>
        protected OdbcDbObject()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the OdbcDbObject class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="id">Unique identifier of the object</param>
        protected OdbcDbObject(OdbcConnection conn, T id)
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
        protected virtual void Load(OdbcConnection conn, T id)
        {
        }

        /// <summary>
        /// Loads the values in the in the data row into the object
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="row">Data row containing the data for the object</param>
        protected virtual void Load(OdbcConnection conn, sp.DataRow row)
        {
            LoadPropertiesFromDataRow(conn, row);
        }

        /// <summary>
        /// Determines whether or not the properties currently set to the object are valid for the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="exception">Null if the properties are valid, otherwise set to the reason they are not</param>
        /// <returns>True if the properties of the object are valid, false if not</returns>
        protected virtual Boolean ValidateProperties(OdbcConnection conn, out System.Exception exception)
        {
            exception = null;
            return true;
        }

        /// <summary>
        /// Adds the object to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        protected virtual void AddToDatabase(OdbcConnection conn, String commandText)
        {
            base.AddToDatabase(conn, commandText);
        }

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        protected virtual void UpdateInDatabase(OdbcConnection conn, String commandText)
        {
            base.UpdateInDatabase(conn, commandText);
        }

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        protected virtual void DeleteFromDatabase(OdbcConnection conn, String commandText)
        {
            base.DeleteFromDatabase(conn, commandText);
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Adds the object to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void AddToDatabase(OdbcConnection conn);

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void UpdateInDatabase(OdbcConnection conn);

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void DeleteFromDatabase(OdbcConnection conn);

        #endregion

        #region DbObject Members

        internal override DbCommand GetCommand(DbConnection conn, string commandText)
        {
            return new OdbcCommand(commandText, (OdbcConnection)conn);
        }

        internal override void Load(DbConnection conn, T id)
        {
            this.Load((OdbcConnection)conn, id);
        }

        internal override void Load(DbConnection conn, sp.DataRow row)
        {
            this.Load((OdbcConnection)conn, row);
        }

        internal override Boolean ValidateProperties(DbConnection conn, out System.Exception exception)
        {
            return this.ValidateProperties((OdbcConnection)conn, out exception);
        }

        internal override void AddToDatabase(DbConnection conn)
        {
            this.AddToDatabase((OdbcConnection)conn);
        }

        internal override void UpdateInDatabase(DbConnection conn)
        {
            this.UpdateInDatabase((OdbcConnection)conn);
        }

        internal override void DeleteFromDatabase(DbConnection conn)
        {
            this.DeleteFromDatabase((OdbcConnection)conn);
        }
        
        internal override void AddToDatabase(DbConnection conn, String commandText)
        {
            this.AddToDatabase((OdbcConnection)conn, commandText);
        }

        internal override void UpdateInDatabase(DbConnection conn, String commandText)
        {
            this.UpdateInDatabase((OdbcConnection)conn, commandText);
        }

        internal override void DeleteFromDatabase(DbConnection conn, String commandText)
        {
            this.DeleteFromDatabase((OdbcConnection)conn, commandText);
        }

        #endregion
    }
}

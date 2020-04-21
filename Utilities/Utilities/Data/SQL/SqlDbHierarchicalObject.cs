using System;
using System.Data.Common;
using System.Data.SqlClient;
using sp = Utilities.Data.StoredProcedure;

namespace Utilities.Data.SQL
{
    public abstract class SqlDbHierarchicalObject<T> : DbHierarchicalObject<T>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SqlDbHierarchicalObject class
        /// </summary>
        protected SqlDbHierarchicalObject()
            : this(null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the SqlDbHierarchicalObject class
        /// </summary>
        /// <param name="parent">Parent object</param>
        protected SqlDbHierarchicalObject(SqlDbHierarchicalObject<T> parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initialises a new instance of the SqlDbHierarchicalObject class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="id">Unique identifier of the object</param>
        protected SqlDbHierarchicalObject(SqlConnection conn, T id)
            :base(conn, id)
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
        protected virtual Boolean ValidateSqlProperties(SqlConnection conn, out System.Exception exception)
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
        /// Gets the child object that belongs to the specified parent
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="parent">Parent of the desired child</param>
        /// <returns>Child object that belongs to the specified parent</returns>
        protected abstract DbHierarchicalObject<T> GetChildByParent(SqlConnection conn, DbHierarchicalObject<T> parent);

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

        #region DbHierarchicalObject Members

        internal override DbCommand GetCommand(DbConnection conn, string commandText)
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

        internal override DbHierarchicalObject<T> GetChildByParent(DbConnection conn, DbHierarchicalObject<T> parent)
        {
            return this.GetChildByParent((SqlConnection)conn, parent);
        }

        internal override bool ValidateProperties(DbConnection conn, out System.Exception exception)
        {
            return ValidateSqlProperties((SqlConnection)conn, out exception);
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

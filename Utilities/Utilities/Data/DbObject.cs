using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.Business;
using Utilities.EventArgs;
using sp = Utilities.Data.StoredProcedure;

namespace Utilities.Data
{
    [Serializable]
    public abstract class DbObject<T> : NotifyPropertyChangedObject
    {
        #region Events

        /// <summary>
        /// Fires when the object is saved to the database
        /// </summary>
        public event ObjectSavedEventHandler<T> Saved;

        /// <summary>
        /// Fires when the object is deleted from the database
        /// </summary>
        public event ObjectDeletedEventHanlder<T> Deleted;

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the unique identifier of the object
        /// </summary>
        private T id;

        /// <summary>
        /// Gets or sets a value that determines whether or not the object has been added to the database
        /// </summary>
        private Boolean isInDatabase;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the object
        /// </summary>
        public T Id
        {
            get { return id; }
            protected set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether or not the object has been added to the database
        /// </summary>
        public Boolean IsInDatabase
        {
            get { return isInDatabase; }
            protected set
            {
                isInDatabase = value;
                OnPropertyChanged("IsInDatabase");
            }
        }

        #endregion
        
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DbObject class
        /// </summary>
        protected DbObject()
        {
            if (GetType().IsDefined(typeof(DbObjectMetaDataAttribute), false) == false)
                throw new InvalidOperationException("DbObjects must have a DbObjectMetaData attribute");

            ResetProperties();
            IsInDatabase = false;
        }

        /// <summary>
        /// Initialises a new instance of the DbObject class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="id">Unique identifier of the object</param>
        protected DbObject(DbConnection conn, T id)
            : this()
        {
            Load(conn, id);
            IsInDatabase = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the Saved event
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="wasInDatabase">Value determining whether or not the object was already in the database prior to saving</param>
        protected void OnSaved(DbConnection conn, Boolean wasInDatabase)
        {
            if (Saved != null)
                Saved(this, new DbObjectSavedEventArgs<T>(this, conn, wasInDatabase));
        }

        /// <summary>
        /// Fires the Deleted event
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected void OnDeleted(DbConnection conn)
        {
            if (Deleted != null)
                Deleted(this, new DbObjectDeletedEventArgs<T>(this, conn));
        }

        /// <summary>
        /// Saves the object to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        public void Save(DbConnection conn)
        {
            System.Exception exception;

            if (!ValidateProperties(conn, out exception))
                throw exception;

            Boolean wasInDatabase;

            if (IsInDatabase)
            {
                wasInDatabase = true;
                UpdateInDatabase(conn);
            }
            else
            {
                wasInDatabase = false;
                AddToDatabase(conn);
                IsInDatabase = true;
            }

            OnSaved(conn, wasInDatabase);
        }

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        public void Delete(DbConnection conn)
        {
            if (IsInDatabase)
            {
                DeleteFromDatabase(conn);
                IsInDatabase = false;
                ResetProperties();
            }

            OnDeleted(conn);
        }

        /// <summary>
        /// Sets the IsInDatabase property of the DB object
        /// </summary>
        /// <param name="isInDatabase">Value that determines whether or not the object has been added to the database</param>
        internal void SetIsInDatabase(Boolean isInDatabase)
        {
            IsInDatabase = isInDatabase;
        }

        /// <summary>
        /// Converts each property in the object that is to be saved to the database to a parameter to pass to a command
        /// </summary>
        /// <param name="includeId">Value determining whether or not to include the object's unique identifier in the returned array</param>
        /// <returns>Array containing a command parameter for each property in the obect that is to be saved to the database</returns>
        internal virtual sp.ParameterCollection GetParametersForStoredProcedure(Boolean includeId)
        {
            sp.ParameterCollection parameters = new sp.ParameterCollection();
            DbObjectMetaDataAttribute metaData = GetType().GetCustomAttribute<DbObjectMetaDataAttribute>();

            if (includeId)
                parameters.AddWithValue(metaData.IdParameterName, metaData.IdParameterDataType, Id);

            foreach (PropertyInfo pi in GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(sp.DataColumnAttribute))))
            {
                sp.DataColumnAttribute attribute = pi.GetCustomAttribute<sp.DataColumnAttribute>();
                parameters.AddWithValue(attribute.ColumnName, attribute.DataType, pi.GetValue(this));
            }

            return parameters;
        }
        
        /// <summary>
        /// Loads the values in the in the data row into the object
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="row">Data row containing the data for the object</param>
        internal virtual void LoadPropertiesFromDataRow(DbConnection conn, sp.DataRow row)
        {
            Type type = GetType();

            //get the value of the ID property
            DbObjectMetaDataAttribute metaData = type.GetCustomAttribute<DbObjectMetaDataAttribute>();
            PropertyInfo idPi = type.GetProperty("Id");
            idPi.SetValue(this, row[metaData.IdParameterName]);

            //load other properties
            foreach (PropertyInfo pi in type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(sp.DataColumnAttribute))))
            {
                sp.DataColumnAttribute attribute = pi.GetCustomAttribute<sp.DataColumnAttribute>();
                pi.SetValue(this, row[attribute.ColumnName]);
            }

            IsInDatabase = true;
        }

        /// <summary>
        /// Adds the object to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        internal virtual void AddToDatabase(DbConnection conn, String commandText)
        {
            try
            {
                DbCommand cmd = GetCommand(conn, commandText);
                cmd.Parameters.AddRange(GetParametersForStoredProcedure(false));

                Id = cmd.ExecuteScalar<T>();
            }
            catch (System.Exception e)
            {
                throw new Exception.AddSqlDbObjectException(String.Format("Could not add {0}.", GetType().Name), e);
            }
        }

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        internal virtual void UpdateInDatabase(DbConnection conn, String commandText)
        {
            try
            {
                DbCommand cmd = GetCommand(conn, commandText);
                cmd.Parameters.AddRange(GetParametersForStoredProcedure(true));

                cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                throw new Exception.UpdateSqlDbObjectException(String.Format("Could not update {0}.", GetType().Name), e);
            }
        }

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        internal virtual void DeleteFromDatabase(DbConnection conn, String commandText)
        {
            try
            {
                DbCommand cmd = GetCommand(conn, commandText);

                DbObjectMetaDataAttribute metaData = GetType().GetCustomAttribute<DbObjectMetaDataAttribute>();
                cmd.Parameters.AddWithValue(metaData.IdParameterName, metaData.IdParameterDataType, Id);

                cmd.ExecuteNonQuery();
            }
            catch(System.Exception e)
            {
                throw new Exception.DeleteSqlDbObjectException(String.Format("Could not delete {0}.", GetType().Name), e);
            }
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Loads the object from the database using the specified ID
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="id">Unique identifier of the object</param>
        internal abstract void Load(DbConnection conn, T id);

        /// <summary>
        /// Loads the values in the in the data row into the object
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="row">Data row containing the data for the object</param>
        internal abstract void Load(DbConnection conn, sp.DataRow row);

        /// <summary>
        /// Determines whether or not the properties currently set to the object are valid for the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="exception">Null if the properties are valid, otherwise set to the reason they are not</param>
        /// <returns>True if the properties of the object are valid, false if not</returns>
        internal abstract Boolean ValidateProperties(DbConnection conn, out System.Exception exception);

        /// <summary>
        /// Gets a new command object
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        /// <returns>New command object</returns>
        internal abstract DbCommand GetCommand(DbConnection conn, String commandText);

        /// <summary>
        /// Adds the object to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        internal abstract void AddToDatabase(DbConnection conn);

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        internal abstract void UpdateInDatabase(DbConnection conn);

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        internal abstract void DeleteFromDatabase(DbConnection conn);

        /// <summary>
        /// Resets properties of the object to their default values
        /// </summary>
        protected abstract void ResetProperties();

        #endregion
    }
}

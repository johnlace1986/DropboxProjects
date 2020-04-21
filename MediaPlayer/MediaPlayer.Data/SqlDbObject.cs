using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using MediaPlayer.Data.EventArgs;
using sp = MediaPlayer.Data.StoredProcedure;

namespace MediaPlayer.Data
{
    [Serializable]
    public abstract class SqlDbObject : NotifyPropertyChangedObject
    {
        #region Events

        /// <summary>
        /// Fires when the object is saved to the database
        /// </summary>
        public event ObjectSavedEventHandler Saved;

        /// <summary>
        /// Fires when the object is deleted from the database
        /// </summary>
        public event ObjectDeletedEventHanlder Deleted;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets a value that determines whether or not the object has been added to the database
        /// </summary>
        public Boolean IsInDatabase { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SqlSbObject class
        /// </summary>
        protected SqlDbObject()
        {
            ResetProperties();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the Saved event
        /// </summary>
        private void OnSaved()
        {
            if (Saved != null)
                Saved(this, new ObjectSavedEventArgs(this));
        }

        /// <summary>
        /// Fires the Deleted event
        /// </summary>
        private void OnDeleted()
        {
            if (Deleted != null)
                Deleted(this, new ObjectDeletedEventArgs(this));
        }

        /// <summary>
        /// Saves the object to the database
        /// </summary>
        public void Save()
        {
            System.Exception exception;

            if (!ValidateProperties(out exception))
                throw exception;

            if (IsInDatabase)
                UpdateInDatabase();
            else
            {
                IsInDatabase = true;
                AddToDatabase();
            }

            OnSaved();
        }

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        public void Delete()
        {
            if (IsInDatabase)
            {
                DeleteFromDatabase();
                IsInDatabase = false;
                ResetProperties();
            }

            OnDeleted();
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Determines whether or not the properties currently set to the object are valid for the database
        /// </summary>
        /// <param name="exception">Null if the properties are valid, otherwise set to the reason they are not</param>
        /// <returns>True if the properties of the object are valid, false if not</returns>
        protected abstract Boolean ValidateProperties(out System.Exception exception);

        /// <summary>
        /// Adds the object to the database
        /// </summary>
        protected abstract void AddToDatabase();

        /// <summary>
        /// Updates the object in the database
        /// </summary>
        protected abstract void UpdateInDatabase();

        /// <summary>
        /// Deletes the object from the database
        /// </summary>
        protected abstract void DeleteFromDatabase();

        /// <summary>
        /// Resets properties of the object to their default values
        /// </summary>
        protected abstract void ResetProperties();

        /// <summary>
        /// Converts each property in the object that is to be saved to the database to a parameter to pass to a stored procedure
        /// </summary>
        /// <param name="includeId">Value determining whether or not to include the object's unique identifier in the returned array</param>
        /// <returns>Array containing a stored procedure parameter for each property in the obect that is to be saved to the database</returns>
        public abstract sp.Parameter[] GetParametersForStoredProcedure(Boolean includeId);

        /// <summary>
        /// Clones the SqlDbObject
        /// </summary>
        /// <returns>Cloned object</returns>
        public abstract SqlDbObject Clone();

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the default date and time used to represent null in the database
        /// </summary>
        public static DateTime DefaultDate
        {
            get { return new DateTime(1900, 1, 1, 0, 0, 0); }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates the connection based on the connection string in the .config file
        /// </summary>
        /// <returns>Connection to the database</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ConnectionString);
        }
        
        /// <summary>
        /// Creates a new SQL command
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="parameters">Collection of parameters to pass to the stored procedure</param>
        /// <returns>SQL command with the specified properties</returns>
        private static SqlCommand GetCommand(String storedProcedureName, sp.Parameter[] parameters)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            foreach (sp.Parameter parameter in parameters)
                command.Parameters.Add(parameter.GetSqlParameter());

            return command;
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        public static void ExecuteNonQuery(String storedProcedureName)
        {
            ExecuteNonQuery(storedProcedureName, new sp.Parameter[0]);
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="conn">Open connection to the database</param>
        public static void ExecuteNonQuery(String storedProcedureName, SqlConnection conn)
        {
            ExecuteNonQuery(storedProcedureName, new sp.Parameter[0], conn);
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameter">Single parameter to pass to the stored procedure</param>
        public static void ExecuteNonQuery(String storedProcedureName, sp.Parameter parameter)
        {
            ExecuteNonQuery(storedProcedureName, new sp.Parameter[1] { parameter });
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameter">Single parameter to pass to the stored procedure</param>
        /// <param name="conn">Open connection to the database</param>
        public static void ExecuteNonQuery(String storedProcedureName, sp.Parameter parameter, SqlConnection conn)
        {
            ExecuteNonQuery(storedProcedureName, new sp.Parameter[1] { parameter }, conn);
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameters">Collection of parameters to pass to the stored procedure</param>
        public static void ExecuteNonQuery(String storedProcedureName, sp.Parameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    ExecuteNonQuery(storedProcedureName, parameters, conn);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameters">Collection of parameters to pass to the stored procedure</param>
        /// <param name="conn">Open connection to the database</param>
        public static void ExecuteNonQuery(String storedProcedureName, sp.Parameter[] parameters, SqlConnection conn)
        {
            using (SqlCommand cmd = GetCommand(storedProcedureName, parameters))
            {
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the selected data
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <returns>The data selected by the stored procedure</returns>
        public static sp.DataTable ExecuteReader(String storedProcedureName)
        {
            return ExecuteReader(storedProcedureName, new sp.Parameter[0]);
        }

        /// <summary>
        /// Executes a stored procedure and returns the selected data
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>The data selected by the stored procedure</returns>
        public static sp.DataTable ExecuteReader(String storedProcedureName, SqlConnection conn)
        {
            return ExecuteReader(storedProcedureName, new sp.Parameter[0], conn);
        }

        /// <summary>
        /// Executes a stored procedure and returns the selected data
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameter">Single parameter to pass to the stored procedure</param>
        /// <returns>The data selected by the stored procedure</returns>
        public static sp.DataTable ExecuteReader(String storedProcedureName, sp.Parameter parameter)
        {
            return ExecuteReader(storedProcedureName, new sp.Parameter[1] { parameter });
        }

        /// <summary>
        /// Executes a stored procedure and returns the selected data
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameter">Single parameter to pass to the stored procedure</param>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>The data selected by the stored procedure</returns>
        public static sp.DataTable ExecuteReader(String storedProcedureName, sp.Parameter parameter, SqlConnection conn)
        {
            return ExecuteReader(storedProcedureName, new sp.Parameter[1] { parameter }, conn);
        }

        /// <summary>
        /// Executes a stored procedure and returns the selected data
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameters">Collection of parameters to pass to the stored procedure</param>
        /// <returns>The data selected by the stored procedure</returns>
        public static sp.DataTable ExecuteReader(String storedProcedureName, sp.Parameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return ExecuteReader(storedProcedureName, parameters, conn);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the selected data
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameters">Collection of parameters to pass to the stored procedure</param>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>The data selected by the stored procedure</returns>
        public static sp.DataTable ExecuteReader(String storedProcedureName, sp.Parameter[] parameters, SqlConnection conn)
        {
            using (SqlCommand cmd = GetCommand(storedProcedureName, parameters))
            {
                cmd.Connection = conn;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    return MediaPlayer.Data.StoredProcedure.DataTable.FromSqlDataReader(dr);
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the scalar result
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <returns>The scalar result of executing the stored procedure</returns>
        public static object ExecuteScalar(String storedProcedureName)
        {
            return ExecuteScalar(storedProcedureName, new sp.Parameter[0]);
        }

        /// <summary>
        /// Executes a stored procedure and returns the scalar result
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>The scalar result of executing the stored procedure</returns>
        public static object ExecuteScalar(String storedProcedureName, SqlConnection conn)
        {
            return ExecuteScalar(storedProcedureName, new sp.Parameter[0], conn);
        }

        /// <summary>
        /// Executes a stored procedure and returns the scalar result
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameter">Single parameter to pass to the stored procedure</param>
        /// <returns>The scalar result of executing the stored procedure</returns>
        public static object ExecuteScalar(String storedProcedureName, sp.Parameter parameter)
        {
            return ExecuteScalar(storedProcedureName, new sp.Parameter[1] { parameter });
        }

        /// <summary>
        /// Executes a stored procedure and returns the scalar result
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameter">Single parameter to pass to the stored procedure</param>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>The scalar result of executing the stored procedure</returns>
        public static object ExecuteScalar(String storedProcedureName, sp.Parameter parameter, SqlConnection conn)
        {
            return ExecuteScalar(storedProcedureName, new sp.Parameter[1] { parameter }, conn);
        }

        /// <summary>
        /// Executes a stored procedure and returns the scalar result
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameters">Collection of parameters to pass to the stored procedure</param>
        /// <returns>The scalar result of executing the stored procedure</returns>
        public static object ExecuteScalar(String storedProcedureName, sp.Parameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return ExecuteScalar(storedProcedureName, parameters, conn);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the scalar result
        /// </summary>
        /// <param name="storedProcedureName">Name of the of the stored procedure being executed</param>
        /// <param name="parameters">Collection of parameters to pass to the stored procedure</param>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>The scalar result of executing the stored procedure</returns>
        public static object ExecuteScalar(String storedProcedureName, sp.Parameter[] parameters, SqlConnection conn)
        {
            using (SqlCommand cmd = GetCommand(storedProcedureName, parameters))
            {
                cmd.Connection = conn;
                return cmd.ExecuteScalar();
            }
        }

        #endregion
    }
}

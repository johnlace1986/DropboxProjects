using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace NisbetPhotography.DbObjects.Data
{
    public static class Control
    {
        #region Parameters

        /// <summary>
        /// Creates a SqlParameter object with the appropriate parameters set
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="dbType">Data type of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <returns>SqlParameter object with the appropriate parameters set</returns>
        internal static StoredProcedureParameter GetParameter(String parameterName, DbType dataType, Object value)
        {
            return new StoredProcedureParameter() { ParameterName = parameterName, DataType = dataType, Value = value };
        }

        #endregion

        #region Commands
        
        /// <summary>
        /// Creates a SqlCommand object with the relevant properties set
        /// </summary>
        /// <param name="conn">SqlConnection used to execute this command</param>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <returns>SqlCommand object with the relevant properties set</returns>
        internal static SqlCommand GetCommand(SqlConnection conn, String commandText)
        {
            return GetCommand(conn, commandText, new StoredProcedureParameter[0]);
        }

        /// <summary>
        /// Creates a SqlCommand object with the relevant properties set
        /// </summary>
        /// <param name="conn">SqlConnection used to execute this command</param>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameter">Parameter needed to be passed to the stored procedure</param>
        /// <returns>SqlCommand object with the relevant properties set</returns>
        internal static SqlCommand GetCommand(SqlConnection conn, String commandText, StoredProcedureParameter parameter)
        {
            return GetCommand(conn, commandText, new StoredProcedureParameter[1] { parameter });
        }

        /// <summary>
        /// Creates a SqlCommand object with the relevant properties set
        /// </summary>
        /// <param name="conn">SqlConnection used to execute this command</param>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameters">Parameters needed to be passed to the stored procedure</param>
        /// <returns>SqlCommand object with the relevant properties set</returns>
        internal static SqlCommand GetCommand(SqlConnection conn, String commandText, StoredProcedureParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = commandText;

            foreach (StoredProcedureParameter param in parameters)
                cmd.Parameters.Add(new SqlParameter() { ParameterName = param.ParameterName, DbType = param.DataType, Value = param.Value });

            return cmd;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes a stored procedure and returns the data it loads
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <returns>DataTableReader object containing all the rows selected by the stored procedure</returns>
        internal static DataTableReader ExecuteReader(String commandText)
        {
            return ExecuteReader(commandText, new StoredProcedureParameter[0]);
        }

        /// <summary>
        /// Executes a stored procedure and returns the data it loads
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameter">Parameter needed to be passed to the stored procedure</param>
        /// <returns>DataTableReader object containing all the rows selected by the stored procedure</returns>
        internal static DataTableReader ExecuteReader(String commandText, StoredProcedureParameter parameter)
        {
            return ExecuteReader(commandText, new StoredProcedureParameter[1] { parameter });
        }

        /// <summary>
        /// Executes a stored procedure and returns the data it loads
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameters">Parameters needed to be passed to the stored procedure</param>
        /// <returns>DataTableReader object containing all the rows selected by the stored procedure</returns>
        internal static DataTableReader ExecuteReader(String commandText, StoredProcedureParameter[] parameters)
        {
            using (SqlConnection conn = Business.ConfigSettings.MasterDbConnection)
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = GetCommand(conn, commandText, parameters))
                    {
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());

                        return dt.CreateDataReader();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        internal static void ExecuteNonQuery(String commandText)
        {
            ExecuteNonQuery(commandText, new StoredProcedureParameter[0]);
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameter">Parameter needed to be passed to the stored procedure</param>
        internal static void ExecuteNonQuery(String commandText, StoredProcedureParameter parameter)
        {
            ExecuteNonQuery(commandText, new StoredProcedureParameter[1] { parameter });
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameters">Parameters needed to be passed to the stored procedure</param>
        internal static void ExecuteNonQuery(String commandText, StoredProcedureParameter[] parameters)
        {
            using (SqlConnection conn = Business.ConfigSettings.MasterDbConnection)
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = GetCommand(conn, commandText, parameters))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes a stored procedure and returns the single value it generates
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <returns>Single value generated by the stored procedure</returns>
        internal static object ExecuteScalar(String commandText)
        {
            return ExecuteScalar(commandText, new StoredProcedureParameter[0]);
        }

        /// <summary>
        /// Executes a stored procedure and returns the single value it generates
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameter">Parameter needed to be passed to the stored procedure</param>
        /// <returns>Single value generated by the stored procedure</returns>
        internal static object ExecuteScalar(String commandText, StoredProcedureParameter parameter)
        {
            return ExecuteScalar(commandText, new StoredProcedureParameter[1] { parameter });
        }

        /// <summary>
        /// Executes a stored procedure and returns the single value it generates
        /// </summary>
        /// <param name="commandText">Name of the stored procedure being executed</param>
        /// <param name="parameters">Parameters needed to be passed to the stored procedure</param>
        /// <returns>Single value generated by the stored procedure</returns>
        internal static object ExecuteScalar(String commandText, StoredProcedureParameter[] parameters)
        {
            using (SqlConnection conn = Business.ConfigSettings.MasterDbConnection)
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = GetCommand(conn, commandText, parameters))
                    {
                        return cmd.ExecuteScalar();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region TestConnections

        /// <summary>
        /// Tests the master database connection
        /// </summary>
        /// <param name="e">Reason the database connection failed</param>
        /// <returns>True if the connection is OK, false if not</returns>
        public static Boolean TestConnection(out System.Exception e)
        {
            using (SqlConnection conn = Business.ConfigSettings.MasterDbConnection)
            {
                try
                {
                    conn.Open();
                    e = null;
                }
                catch (System.Exception ex)
                {
                    e = ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            return (e == null);
        }

        #endregion
    }
}

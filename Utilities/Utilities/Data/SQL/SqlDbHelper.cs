using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Utilities.Data.SQL
{
    public static class SqlDbHelper
    {
        #region Static Properties

        /// <summary>
        /// Gets the default date and time used to represent null in the database
        /// </summary>
        public static DateTime DefaultDate
        {
            get { return new DateTime(1899, 12, 30, 0, 0, 0); }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Opens a connection to the database, performs an action and closes the connection to the database
        /// </summary>
        /// <param name="connectionString">Connection string pointing to the database</param>
        /// <param name="action">Action to perform</param>
        public static void DatabaseAction(String connectionString, Action<SqlConnection> action)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                DatabaseAction(conn, () =>
                {
                    action(conn);
                });
            }
        }

        /// <summary>
        /// Opens a connection to the database, performs an action and closes the connection to the database
        /// </summary>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="action">Action to perform</param>
        public static void DatabaseAction(SqlConnection conn, Action action)
        {
            DbHelper.DatabaseAction(conn, () =>
            {
                action();
            });
        }

        /// <summary>
        /// Opens a connection to the database, performs an function, closes the connection to the database and returns the result of the function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="connectionString">Connection string pointing to the database</param>
        /// <param name="function">Function to perform</param>
        /// <returns>Result of the function</returns>
        public static T DatabaseFunction<T>(String connectionString, Func<SqlConnection, T> function)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                return DatabaseFunction<T>(conn, () =>
                {
                    return function(conn);
                });
            }
        }

        /// <summary>
        /// Opens a connection to the database, performs an function, closes the connection to the database and returns the result of the function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="function">Function to perform</param>
        /// <returns>Result of the function</returns>
        public static T DatabaseFunction<T>(SqlConnection conn, Func<T> function)
        {
            return DbHelper.DatabaseFunction<T>(conn, () =>
            {
                return function();
            });
        }

        #endregion
    }
}

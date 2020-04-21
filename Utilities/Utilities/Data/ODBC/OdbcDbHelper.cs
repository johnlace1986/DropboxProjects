using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace Utilities.Data.ODBC
{
    public static class OdbcDbHelper
    {
        #region Static Methods

        /// <summary>
        /// Opens a connection to the database, performs an action and closes the connection to the database
        /// </summary>
        /// <param name="connectionString">Connection string pointing to the database</param>
        /// <param name="action">Action to perform</param>
        public static void DatabaseAction(String connectionString, Action<OdbcConnection> action)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                DatabaseAction(conn, action);
            }
        }

        /// <summary>
        /// Opens a connection to the database, performs an action and closes the connection to the database
        /// </summary>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="action">Action to perform</param>
        public static void DatabaseAction(OdbcConnection conn, Action<OdbcConnection> action)
        {
            DbHelper.DatabaseAction(conn, () =>
            {
                action(conn);
            });
        }

        /// <summary>
        /// Opens a connection to the database, performs an function, closes the connection to the database and returns the result of the function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="connectionString">Connection string pointing to the database</param>
        /// <param name="function">Function to perform</param>
        /// <returns>Result of the function</returns>
        public static T DatabaseFunction<T>(String connectionString, Func<OdbcConnection, T> function)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                return DatabaseFunction<T>(conn, function);
            }
        }

        /// <summary>
        /// Opens a connection to the database, performs an function, closes the connection to the database and returns the result of the function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="function">Function to perform</param>
        /// <returns>Result of the function</returns>
        public static T DatabaseFunction<T>(OdbcConnection conn, Func<OdbcConnection, T> function)
        {
            return DbHelper.DatabaseFunction<T>(conn, () =>
            {
                return function(conn);
            });
        }

        #endregion
    }
}

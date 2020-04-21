using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using sp = Utilities.Data.StoredProcedure;

namespace Utilities.Data
{
    internal static class DbHelper
    {
        #region Static Methods

        /// <summary>
        /// Opens a connection to the database, performs an action and closes the connection to the database
        /// </summary>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="action">Action to perform</param>
        public static void DatabaseAction(DbConnection conn, Action action)
        {
            DatabaseAction(conn, dummy =>
            {
                action();
            });
        }

        /// <summary>
        /// Opens a connection to the database, performs an action and closes the connection to the database
        /// </summary>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="action">Action to perform</param>
        public static void DatabaseAction(DbConnection conn, Action<DbConnection> action)
        {
            DatabaseFunction<object>(conn, () =>
            {
                action(conn);
                return null;
            });
        }

        /// <summary>
        /// Opens a connection to the database, performs an function, closes the connection to the database and returns the result of the function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="function">Function to perform</param>
        /// <returns>Result of the function</returns>
        public static T DatabaseFunction<T>(DbConnection conn, Func<T> function)
        {
            return DatabaseFunction<T>(conn, dummy =>
            {
                return function();
            });
        }

        /// <summary>
        /// Opens a connection to the database, performs an function, closes the connection to the database and returns the result of the function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="conn">Unopened connection to the database</param>
        /// <param name="function">Function to perform</param>
        /// <returns>Result of the function</returns>
        public static T DatabaseFunction<T>(DbConnection conn, Func<DbConnection, T> function)
        {
            try
            {
                conn.Open();
                return function(conn);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Gets a value from the data reader but returns the default value if the database value is null
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="reader">Data reader containing the value</param>
        /// <param name="ordinal">Index in the reader of the desired field</param>
        /// <returns>Value read from the database</returns>
        public static T ValueFromDataReader<T>(DbDataReader reader, Int32 ordinal)
        {
            return ValueFromDataReader<T>(reader, ordinal, default(T));
        }

        /// <summary>
        /// Gets a value from the data reader but returns the default value if the database value is null
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="reader">Data reader containing the value</param>
        /// <param name="fieldName">Name of the field of the value</param>
        /// <returns>Value read from the database</returns>
        public static T ValueFromDataReader<T>(DbDataReader reader, String fieldName)
        {
            return ValueFromDataReader<T>(reader, fieldName, default(T));
        }

        /// <summary>
        /// Gets a value from the data reader but returns the default value if the database value is null
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="reader">Data reader containing the value</param>
        /// <param name="ordinal">Index in the reader of the desired field</param>
        /// <param name="defaultValue">Default value to return if the database value is null</param>
        /// <returns>Value read from the database</returns>
        public static T ValueFromDataReader<T>(DbDataReader reader, Int32 ordinal, T defaultValue)
        {
            String fieldName = reader.GetName(ordinal);
            return ValueFromDataReader<T>(reader[fieldName], defaultValue);
        }

        /// <summary>
        /// Gets a value from the data reader but returns the default value if the database value is null
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="reader">Data reader containing the value</param>
        /// <param name="fieldName">Name of the field of the value</param>
        /// <param name="defaultValue">Default value to return if the database value is null</param>
        /// <returns>Value read from the database</returns>
        public static T ValueFromDataReader<T>(DbDataReader reader, String fieldName, T defaultValue)
        {
            return ValueFromDataReader<T>(reader[fieldName], defaultValue);
        }

        /// <summary>
        /// Gets a value from the data reader but returns the default value if the database value is null
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="value">Value read from the database</param>
        /// <param name="defaultValue">Default value to return if the database value is null</param>
        /// <returns>Value read from the database</returns>
        private static T ValueFromDataReader<T>(object value, T defaultValue)
        {
            if (value == DBNull.Value)
                return defaultValue;

            return (T)value;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using db = System.Data.Common;
using System.Linq;
using System.Text;
using Utilities.Business;
using sp = Utilities.Data.StoredProcedure;
using Utilities.Data.StoredProcedure;

namespace Utilities.Data
{
    public abstract class DbCommand : NotifyPropertyChangedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets an open connection to the database
        /// </summary>
        protected db.DbConnection connection;

        /// <summary>
        /// Gets or sets the T-SQL statement or name of the command being executed
        /// </summary>
        private String commandText;

        /// <summary>
        /// Gets or sets the type of command being executed
        /// </summary>
        protected CommandType commandType;

        /// <summary>
        /// Gets or sets the collection of parameters to pass to the command
        /// </summary>
        private ParameterCollection parameters;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the T-SQL statement or name of the command being executed
        /// </summary>
        public String CommandText
        {
            get { return commandText; }
            private set
            {
                commandText = value;
                OnPropertyChanged("CommandText");
            }
        }

        /// <summary>
        /// Gets or sets the collection of parameters to pass to the command
        /// </summary>
        public ParameterCollection Parameters
        {
            get { return parameters; }
            private set
            {
                parameters = value;
                OnPropertyChanged("Parameters");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DbCommand class
        /// </summary>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        protected DbCommand(String commandText)
            : base()
        {
            this.connection = null;
            this.commandType = CommandType.Text;

            CommandText = commandText;
            Parameters = new ParameterCollection();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Generates a DB command based on the properties in this object
        /// </summary>
        /// <returns>DB command generated from the properties in this object</returns>
        private db.DbCommand ToDbCommand()
        {
            db.DbCommand cmd = GetEmptyCommand();
            cmd.Connection = connection;
            cmd.CommandText = CommandText;
            cmd.CommandType = commandType;

            foreach (sp.Parameter parameter in Parameters)
            {
                db.DbParameter dbParameter = GetEmptyParameter();
                dbParameter.ParameterName = parameter.Name;
                dbParameter.DbType = parameter.Type;
                dbParameter.Value = GetCommandParameterValue(parameter.Value);

                cmd.Parameters.Add(dbParameter);
            }

            return cmd;
        }

        /// <summary>
        /// Executes a command
        /// </summary>
        public void ExecuteNonQuery()
        {
            using (db.DbCommand cmd = ToDbCommand())
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a command and returns the selected data
        /// </summary>
        /// <returns>The data selected by the command</returns>
        public sp.DataTable ExecuteReader()
        {
            using (db.DbCommand cmd = ToDbCommand())
            {
                using (db.DbDataReader dr = cmd.ExecuteReader())
                {
                    return sp.DataTable.FromDataReader(dr);
                }
            }
        }

        /// <summary>
        /// Executes a command and returns the scalar result
        /// </summary>
        /// <typeparam name="T">Type of scalar result to return</typeparam>
        /// <returns>The scalar result of executing the command</returns>
        public T ExecuteScalar<T>()
        {
            using (db.DbCommand cmd = ToDbCommand())
            {
                return (T)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes a command and returns the only row selected
        /// </summary>
        /// <returns>The only row selected by the command</returns>
        public sp.DataRow ExecuteSingleReader()
        {
            sp.DataTable dt = ExecuteReader();

            if (dt.RowCount != 1)
                throw new Exception.SpecifiedSqlDbObjectNotFoundException(String.Format("{0} rows found in the database", dt.RowCount));

            return dt[0];
        }

        /// <summary>
        /// Executes a command and maps the single selected row to an object
        /// </summary>
        /// <typeparam name="T">Type of object to map the data to</typeparam>
        /// <typeparam name="TIdType">Type of the unique identifier of the object</typeparam>
        /// <returns>Single row selected by the command, mapped to an object</returns>
        public T ExecuteMappedSingleReader<T, TIdType>() where T : DbObject<TIdType>
        {
            sp.DataRow row = ExecuteSingleReader();
            return ExecuteMappedSingleReader<T, TIdType>(connection, row);
        }

        /// <summary>
        /// Executes a command and maps the single selected row to an object
        /// </summary>
        /// <typeparam name="T">Type of object to map the data to</typeparam>
        /// <param name="mapper">Mapper used to map the data to an object</param>
        /// <returns>Single row selected by the command, mapped to an object</returns>
        public T ExecuteMappedSingleReader<T>(Func<sp.DataRow, T> mapper)
        {
            sp.DataRow row = ExecuteSingleReader();
            return mapper(row);
        }

        /// <summary>
        /// Executes a command and maps each row to an object
        /// </summary>
        /// <typeparam name="T">Type of object to map the data to</typeparam>
        /// <typeparam name="TIdType">Type of the unique identifier of the object</typeparam>
        /// <returns>Each row selected by the command, mapped to an object</returns>
        public T[] ExecuteMappedReader<T, TIdType>() where T : DbObject<TIdType>
        {
            sp.DataTable table = ExecuteReader();

            return ExecuteMappedReader<T>(table, row =>
            {
                return ExecuteMappedSingleReader<T, TIdType>(connection, row);
            });
        }

        /// <summary>
        /// Executes a command and maps each row to an object
        /// </summary>
        /// <typeparam name="T">Type of object to map the data to</typeparam>
        /// <typeparam name="TIdType">Type of the unique identifier of the object</typeparam>
        /// <param name="mapper">Mapper used to map the data to an object</param>
        /// <returns>Each row selected by the command, mapped to an object</returns>
        public T[] ExecuteMappedReader<T, TIdType>(Func<sp.DataRow, T> mapper)
        {
            sp.DataTable dt = ExecuteReader();

            return ExecuteMappedReader<T>(dt, row =>
            {
                T item = mapper(row);

                DbObject<TIdType> dbObject = item as DbObject<TIdType>;

                if (dbObject != null)
                    dbObject.SetIsInDatabase(true);

                return item;
            });
        }

        /// <summary>
        /// Maps a data row to an object
        /// </summary>
        /// <typeparam name="T">Type of object to map the data to</typeparam>
        /// <typeparam name="TIdType">Type of the unique identifier of the object</typeparam>
        /// <param name="row">Data row containing the row being mapped</param>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Data row, mapped to an object</returns>
        private static T ExecuteMappedSingleReader<T, TIdType>(db.DbConnection conn, sp.DataRow row) where T : DbObject<TIdType>
        {
            //create a new item
            DbObject<TIdType> item = Activator.CreateInstance<T>() as DbObject<TIdType>;
            item.Load(conn, row);

            return (T)item;
        }

        /// <summary>
        /// Maps each row in a data table to an object
        /// </summary>
        /// <typeparam name="T">Type of object to map the data to</typeparam>
        /// <param name="table">Data table containing the rows being mapped</param>
        /// <param name="mapper">Mapper used to map the data to an object</param>
        /// <returns>Each row in the data table, mapped to an object</returns>
        private static T[] ExecuteMappedReader<T>(sp.DataTable table, Func<sp.DataRow, T> mapper)
        {
            List<T> items = new List<T>();

            foreach (sp.DataRow dr in table)
            {
                items.Add(mapper(dr));
            }

            if ((typeof(IComparable).IsAssignableFrom(typeof(T))) || (typeof(IComparable<T>).IsAssignableFrom(typeof(T))))
                items.Sort();

            return items.ToArray();
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Gets a new command object
        /// </summary>
        /// <returns>New command object</returns>
        protected abstract db.DbCommand GetEmptyCommand();

        /// <summary>
        /// Gets a new parameter object
        /// </summary>
        /// <returns>New parameter object</returns>
        protected abstract db.DbParameter GetEmptyParameter();
        
        #endregion

        #region Static Methods

        /// <summary>
        /// Converts the specified value to a DB safe value
        /// </summary>
        /// <param name="value">Value being converted</param>
        /// <returns>DB safe value</returns>
        private static object GetCommandParameterValue(object value)
        {
            if (value == null)
                return DBNull.Value;

            if (value is DateTime)
                return ((DateTime)value).ToUniversalTime();

            return value;
        }


        #endregion
    }
}

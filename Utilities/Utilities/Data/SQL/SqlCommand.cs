using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using db = System.Data.Common;
using sql = System.Data.SqlClient;
using System.Linq;
using System.Text;
using sp = Utilities.Data.StoredProcedure;

namespace Utilities.Data.SQL
{
    public class SqlCommand : DbCommand
    {
        #region Properties

        /// <summary>
        /// Gets or sets an open connection to the database
        /// </summary>
        public sql.SqlConnection Connection
        {
            get { return (sql.SqlConnection)connection; }
            private set
            {
                connection = value;
                OnPropertyChanged("Connection");
            }
        }

        /// <summary>
        /// Gets or sets the type of command being executed
        /// </summary>
        public CommandType CommandType
        {
            get { return commandType; }
            set
            {
                commandType = value;
                OnPropertyChanged("CommandType");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SqlCommand class
        /// </summary>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        /// <param name="connection">Open connection to the database</param>
        public SqlCommand(String commandText, sql.SqlConnection connection)
            : base(commandText)
        {
            Connection = connection;
            CommandType = CommandType.StoredProcedure;
        }

        #endregion

        #region DbCommand Members

        protected override db.DbCommand GetEmptyCommand()
        {
            return new sql.SqlCommand();
        }

        protected override db.DbParameter GetEmptyParameter()
        {
            return new sql.SqlParameter();
        }

        #endregion
    }
}

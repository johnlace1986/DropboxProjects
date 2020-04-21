using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using db = System.Data.Common;
using odbc = System.Data.Odbc;
using System.Linq;
using System.Text;
using sp = Utilities.Data.StoredProcedure;

namespace Utilities.Data.ODBC
{
    public class OdbcCommand : DbCommand
    {
        #region Properties

        /// <summary>
        /// Gets or sets an open connection to the database
        /// </summary>
        public odbc.OdbcConnection Connection
        {
            get { return (odbc.OdbcConnection)connection; }
            private set
            {
                connection = value;
                OnPropertyChanged("Connection");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OdbcCommand class
        /// </summary>
        /// <param name="commandText">T-SQL statement or name of the command being executed</param>
        /// <param name="connection">Open connection to the database</param>
        public OdbcCommand(String commandText, odbc.OdbcConnection connection)
            : base(commandText)
        {
            this.commandType = CommandType.Text;

            Connection = connection;
        }

        #endregion

        #region DbHelper Members

        protected override db.DbCommand GetEmptyCommand()
        {
            return new odbc.OdbcCommand();
        }

        protected override db.DbParameter GetEmptyParameter()
        {
            return new odbc.OdbcParameter();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Data.StoredProcedure
{
    public class DataColumnAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the database column
        /// </summary>
        public String ColumnName { get; private set; }

        /// <summary>
        /// Gets or sets the data type of the database column
        /// </summary>
        public DbType DataType { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DataColumnAttribute class
        /// </summary>
        /// <param name="dbType">Data type of the database column</param>
        /// <param name="columnName">Name of the database column</param>
        public DataColumnAttribute(String columnName, DbType dataType)
        {
            ColumnName = columnName;
            DataType = dataType;
        }

        #endregion
    }
}

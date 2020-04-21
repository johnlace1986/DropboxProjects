using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MediaPlayer.Data.StoredProcedure
{
    public struct Parameter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the parameter
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter
        /// </summary>
        public DbType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Creates a new StoredProcedureParameter object
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="type">Type of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        public Parameter(String name, DbType type, object value)
            : this()
        {
            Name = name;
            Type = type;
            Value = value;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Converts the stored procedure parameter to a SQL parameter
        /// </summary>
        /// <returns>SQL parameter representing this stored procedure parameter</returns>
        internal SqlParameter GetSqlParameter()
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = Name;
            sqlParameter.DbType = Type;
            sqlParameter.Value = Value;

            return sqlParameter;
        }

        #endregion
    }
}

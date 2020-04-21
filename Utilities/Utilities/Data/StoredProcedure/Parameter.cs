using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Utilities.Data.StoredProcedure
{
    public class Parameter
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
        public Parameter()
        {
            Name = String.Empty;
            Type = DbType.Object;
            Value = null;
        }

        #endregion
    }
}

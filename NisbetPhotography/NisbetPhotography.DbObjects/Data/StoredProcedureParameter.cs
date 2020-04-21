using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Data
{
    internal struct StoredProcedureParameter
    {
        /// <summary>
        /// Name of the parameter
        /// </summary>
        public String ParameterName { get; set; }

        /// <summary>
        /// Data type of the parameter
        /// </summary>
        public DbType DataType { get; set; }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        public Object Value { get; set; }
    }
}

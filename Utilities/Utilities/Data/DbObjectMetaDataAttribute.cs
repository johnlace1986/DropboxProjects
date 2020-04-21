using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Data
{
    public class DbObjectMetaDataAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name used for parameters for the ID property of the object
        /// </summary>
        public String IdParameterName { get; private set; }

        /// <summary>
        /// Gets or sets the data type used for parameters for the ID property of the object
        /// </summary>
        public DbType IdParameterDataType { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initiliases a new instance of the DbObjectMetaDataAttribute class
        /// </summary>
        /// <param name="idParameterName">Name used for parameters for the ID property of the object</param>
        /// <param name="idParameterDataType">Data type used for parameters for the ID property of the object</param>
        public DbObjectMetaDataAttribute(String idParameterName, DbType idParameterDataType)
        {
            IdParameterName = idParameterName;
            IdParameterDataType = idParameterDataType;
        }

        #endregion
    }
}

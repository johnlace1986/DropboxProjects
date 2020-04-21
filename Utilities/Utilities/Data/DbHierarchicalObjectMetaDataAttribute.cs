using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Data
{
    public class DbHierarchicalObjectMetaDataAttribute : DbObjectMetaDataAttribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name used for parameters for the parent ID property of the object
        /// </summary>
        public String ParentIdParameterName { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initiliases a new instance of the DbHierarchicalObjectMetaDataAttribute class
        /// </summary>
        /// <param name="idParameterName">Name used for parameters for the ID property of the object</param>
        /// <param name="idParameterDataType">Data type used for parameters for the ID property of the object</param>
        public DbHierarchicalObjectMetaDataAttribute(String idParameterName, DbType idParameterDataType, String parentIdParameterName)
            : base(idParameterName, idParameterDataType)
        {
            ParentIdParameterName = parentIdParameterName;
        }

        #endregion
    }
}

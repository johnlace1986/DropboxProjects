using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utilities.Business;

namespace Utilities.Data.StoredProcedure
{
    public class ParameterCollection : NotifyCollectionChangedObject<Parameter>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ParameterCollection class
        /// </summary>
        public ParameterCollection()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the ParameterCollection class
        /// </summary>
        /// <param name="parameters">The collection whose parameters are copied into the new collection</param>
        public ParameterCollection(IEnumerable<Parameter> parameters)
            : base(parameters)
        {
            
        }

        #endregion

        #region Instance Methods
        
        /// <summary>
        /// Adds a new parameter with the specified value to the collection
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="type">Type of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        public void AddWithValue(String name, DbType type, object value)
        {
            Add(new Parameter()
            {
                Name = name,
                Type = type,
                Value = value
            });
        }

        #endregion
    }
}

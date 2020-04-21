using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Exception
{
    public class DuplicatePropertyValueException : System.Exception
    {
        #region Fields

        /// <summary>
        /// Gets the type of object whose property value is a duplicate
        /// </summary>
        private Type objectType;

        /// <summary>
        /// Gets the name of the property value is a duplicate
        /// </summary>
        private String propertyName;

        /// <summary>
        /// Gets or sets the value of the property that already exists
        /// </summary>
        private object propertyValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of object whose property value is a duplicate
        /// </summary>
        public Type ObjectType
        {
            get { return objectType; }
        }

        /// <summary>
        /// Gets the name of the property value is a duplicate
        /// </summary>
        public String PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Gets or sets the value of the property that already exists
        /// </summary>
        public object PropertyValue
        {
            get { return propertyValue; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DuplicatePropertyValueException class
        /// </summary>
        /// <param name="objectType">The type of object whose property value is a duplicate</param>
        /// <param name="propertyName">Name of the property value is a duplicate</param>
        /// <param name="propertyValue">Value of the property that already exists</param>
        public DuplicatePropertyValueException(Type objectType, String propertyName, object propertyValue)
            : this(objectType, propertyName, propertyValue, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the DuplicatePropertyValueException class
        /// </summary>
        /// <param name="objectType">The type of object whose property value is a duplicate</param>
        /// <param name="propertyName">Name of the property value is a duplicate</param>
        /// <param name="propertyValue">Value of the property that already exists</param>
        /// <param name="innerException">Inner exception of the DuplicatePropertyValueException</param>
        public DuplicatePropertyValueException(Type objectType, String propertyName, object propertyValue, System.Exception innerException)
            : base(objectType.Name + " already exists with " + propertyName + " value of " + propertyValue.ToString(), innerException)
        {
            this.objectType = objectType;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }

        #endregion
    }
}

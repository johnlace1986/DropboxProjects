using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPlayer.Data.Exception
{
    public class PropertyNotSetException : System.Exception
    {
        #region Fields

        /// <summary>
        /// Gets or sets the type of object whose property was not set
        /// </summary>
        private Type objectType;

        /// <summary>
        /// Gets or sets the name of the property that was not set
        /// </summary>
        private String propertyName;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of object whose property was not set
        /// </summary>
        public Type ObjectType
        {
            get { return objectType; }
        }

        /// <summary>
        /// Gets the name of the property that was not set
        /// </summary>
        public String PropertyName
        {
            get { return propertyName; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the PropertyNotSetException class
        /// </summary>
        /// <param name="objectType">The type of object whose property was not set</param>
        /// <param name="propertyName">Name of the property that was not set</param>
        public PropertyNotSetException(Type objectType, String propertyName)
            : this(objectType, propertyName, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the PropertyNotSetException class
        /// </summary>
        /// <param name="objectType">The type of object whose property was not set</param>
        /// <param name="propertyName">Name of the property that was not set</param>
        /// <param name="innerException">Inner exception of the PropertyNotSetException</param>
        public PropertyNotSetException(Type objectType, String propertyName, System.Exception innerException)
            : base(propertyName + " property of " + objectType.Name + " object not set", innerException)
        {
            this.objectType = objectType;
            this.propertyName = PropertyName;
        }

        #endregion
    }
}

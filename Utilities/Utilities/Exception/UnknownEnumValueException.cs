using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Exception
{
    public class UnknownEnumValueException : System.Exception
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the UnknownEnumValueException class
        /// </summary>
        /// <param name="value">Unknown enum value</param>
        public UnknownEnumValueException(Enum value)
            : this(value, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the UnknownEnumValueException class
        /// </summary>
        /// <param name="value">Unknown enum value</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public UnknownEnumValueException(Enum value, System.Exception innerException)
            : this(value.GetType().Name, value.ToString(), innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the UnknownEnumValueException class
        /// </summary>
        /// <param name="type">Type of the enum value</param>
        /// <param name="value">Unknown enum value</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        private UnknownEnumValueException(String type, String value, System.Exception innerException)
            : base(String.Format("Unknown {0} value: {1}", type, value), innerException)
        {
        }

        #endregion
    }
}

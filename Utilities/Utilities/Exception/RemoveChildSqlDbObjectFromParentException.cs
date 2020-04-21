﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Exception
{
    public class RemoveChildSqlDbObjectFromParentException : System.Exception
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RemoveChildSqlDbObjectFromParentException class
        /// </summary>
        public RemoveChildSqlDbObjectFromParentException()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the RemoveChildSqlDbObjectFromParentException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public RemoveChildSqlDbObjectFromParentException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the RemoveChildSqlDbObjectFromParentException class with a specified error message and reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public RemoveChildSqlDbObjectFromParentException(String message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}

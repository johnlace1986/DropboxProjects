using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPlayer.Data.Exception
{
    public class SpecifiedSqlDbObjectNotFoundException : System.Exception
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SpecifiedSqlDbObjectNotFoundException class
        /// </summary>
        public SpecifiedSqlDbObjectNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the SpecifiedSqlDbObjectNotFoundException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public SpecifiedSqlDbObjectNotFoundException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the SpecifiedSqlDbObjectNotFoundException class with a specified error message and reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public SpecifiedSqlDbObjectNotFoundException(String message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}

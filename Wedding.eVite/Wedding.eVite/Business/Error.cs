using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Utilities.Data;
using Utilities.Data.SQL;
using sp = Utilities.Data.StoredProcedure;

namespace Wedding.eVite.Business
{
    [DbHierarchicalObjectMetaData("ErrorId", DbType.Int32, "OuterErrorId")]
    public class Error : SqlDbHierarchicalObject<Int32>, IComparable<Error>
    {
        #region Fields
        
        /// <summary>
        /// Gets or sets the name of the error
        /// </summary>
        private String name;

        /// <summary>
        /// Gets or sets the date and time the error was thrown
        /// </summary>
        private DateTime dateThrown;

        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        private String message;

        /// <summary>
        /// Gets or sets the stack trace of the error
        /// </summary>
        private String stackTrace;

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the name of the error
        /// </summary>
        [sp.DataColumn("Name", DbType.String)]
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the date and time the error was thrown
        /// </summary>
        [sp.DataColumn("DateThrown", DbType.DateTime)]
        public DateTime DateThrown
        {
            get { return dateThrown; }
            set
            {
                dateThrown = value;
                OnPropertyChanged("DateThrown");
            }
        }

        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        [sp.DataColumn("Message", DbType.String)]
        public String Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// Gets or sets the stack trace of the error
        /// </summary>
        [sp.DataColumn("StackTrace", DbType.String)]
        public String StackTrace
        {
            get { return stackTrace; }
            set
            {
                stackTrace = value;
                OnPropertyChanged("StackTrace");
            }
        }

        /// <summary>
        /// Gets the outer error this error belongs to
        /// </summary>
        public Error OuterError
        {
            get { return (Error)parent; }
        }

        /// <summary>
        /// Gets or sets the inner error that belongs to this error
        /// </summary>
        public Error InnerError
        {
            get { return (Error)child; }
            set
            {
                OnPropertyChanged("InnerError");
                child = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Error class
        /// </summary>
        public Error()
            : this(null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the Error class
        /// </summary>
        /// <param name="parent">Parent error</param>
        private Error(Error parent)
            : base(parent)
        {
            Id = -1;
            Name = String.Empty;
            DateThrown = DateTime.Now;
            Message = String.Empty;
            StackTrace = String.Empty;
        }

        /// <summary>
        /// Initialises a new instance of the Error class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="errorId">Unique identifier of the error</param>
        public Error(SqlConnection conn, Int32 errorId)
            : base(conn, errorId)
        {
        }

        #endregion
        
        #region Static Methods

        /// <summary>
        /// Creates an instance of the Error class based on the specified exception
        /// </summary>
        /// <param name="e">Exception being used to generate the Error object</param>
        /// <returns>Instance of the Error class generated from the specified exception</returns>
        public static Error FromException(System.Exception e)
        {
            return FromException(e, null);
        }

        /// <summary>
        /// Creates an instance of the Error class based on the specified exception
        /// </summary>
        /// <param name="e">Exception being used to generate the Error object</param>
        /// <param name="outerException">Outer exception of the error</param>
        /// <returns>Instance of the Error class generated from the specified exception</returns>
        private static Error FromException(System.Exception e, Error outerException)
        {
            Error error = new Error(outerException);
            error.Name = e.GetType().FullName;
            error.message = e.Message;
            error.StackTrace = (e.StackTrace == null ? String.Empty : e.StackTrace);

            if (e.InnerException != null)
                error.InnerError = FromException(e.InnerException, error);

            return error;
        }
        
        /// <summary>
        /// Gets all errors currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All errors currently in the system</returns>
        public static Error[] GetErrors(SqlConnection conn)
        {
            return Data.Error.GetErrors(conn);
        }

        /// <summary>
        /// Gets the inner error of the specified error from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="outerError">Outer error</param>
        /// <returns>Inner error of the specified error, loaded from the database</returns>
        private static Error GetErrorByOuterError(SqlConnection conn, Error outerError)
        {
            return Data.Error.GetErrorByOuterError(conn, outerError);
        }

        #endregion

        #region SqlDbObject<Int32> Members

        protected override DbHierarchicalObject<Int32> GetChildByParent(SqlConnection conn, DbHierarchicalObject<Int32> parent)
        {
            return GetErrorByOuterError(conn, (Error)parent);
        }

        protected override void Load(SqlConnection conn, Int32 id)
        {
            Error error = Data.Error.GetErrorById(conn, id);

            Id = error.Id;
            Name = error.Name;
            DateThrown = error.DateThrown;
            Message = error.Message;
            StackTrace = error.StackTrace;
            InnerError = error.InnerError;
        }

        protected override void AddToDatabase(SqlConnection conn)
        {
            AddToDatabase(conn, "AddError");
        }

        protected override void UpdateInDatabase(SqlConnection conn)
        {
            UpdateInDatabase(conn, "UpdateError");
        }

        protected override void DeleteFromDatabase(SqlConnection conn)
        {
            DeleteFromDatabase(conn, "DeleteError");
        }

        protected override void ResetProperties()
        {
            Id = -1;
        }

        #endregion

        #region IComparable<Error> Members

        public int CompareTo(Error other)
        {
            return other.DateThrown.CompareTo(DateThrown);
        }

        #endregion
    }
}

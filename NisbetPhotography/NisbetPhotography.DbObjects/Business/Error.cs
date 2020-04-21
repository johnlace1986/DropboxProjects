using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Business
{
    public class Error : DbObject
    {
        #region Fields

        internal Int16 outerErrorId;

        private Error outerError;

        internal Int16 innerErrorId;

        private Error innerError;

        #endregion

        #region Properties

        /// <summary>
        /// Unique identifier for the error
        /// </summary>
        public Int16 Id { get; internal set; }

        /// <summary>
        /// Name of the exception that was thrown
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Message passed by the error
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// Substring of message passed by the error
        /// </summary>
        public String MessageSubstring
        {
            get
            {
                if (Message.Length <= ConfigSettings.MaxErrorMessageLength)
                    return Message;

                return Message.Substring(0, ConfigSettings.MaxErrorMessageLength - 3) + "...";
            }
        }

        /// <summary>
        /// Stack trace for the error
        /// </summary>
        public String StrackTrace { get; set; }

        /// <summary>
        /// Error's parent
        /// </summary>
        public Error OuterError
        {
            get
            {
                if (outerErrorId == 0)
                    return null;

                if (outerError == null)
                    outerError = new Error(outerErrorId);

                return outerError;
            }
            internal set
            {
                if (value == null)
                    outerErrorId = 0;
                else
                    outerErrorId = value.Id;

                outerError = value;
            }
        }

        /// <summary>
        /// Error's child
        /// </summary>
        public Error InnerError
        {
            get
            {
                if (innerErrorId == 0)
                    return null;

                if (innerError == null)
                    innerError = new Error(innerErrorId);

                return innerError;
            }
            internal set
            {
                if (value == null)
                    innerErrorId = 0;
                else
                    innerErrorId = value.Id;

                innerError = value;
            }
        }

        /// <summary>
        /// Date and time the error was thrown
        /// </summary>
        public DateTime DateThrown { get; internal set; }

        #endregion

        #region Constructors

        public Error()
        {
            Id = -1;
            Name = String.Empty;
            Message = String.Empty;
            StrackTrace = String.Empty;
            OuterError = null;
            InnerError = null;
            DateThrown = DateTime.Now;

            IsInDatabase = false;
        }

        public Error(Int16 errorId)
        {
            Error clone = Data.Error.GetErrorById(errorId);

            Id = clone.Id;
            Name = clone.Name;
            Message = clone.Message;
            StrackTrace = clone.StrackTrace;
            outerErrorId = clone.outerErrorId;
            innerErrorId = clone.innerErrorId;
            DateThrown = clone.DateThrown;

            IsInDatabase = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds an inner error
        /// </summary>
        /// <param name="innerError">New inner error</param>
        public void AddInnerError(Error innerError)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot add an inner error to an error that is not in the database");

            if (!(innerError.IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot add an inner error that is not in the datbase to an error");

            if (innerErrorId != 0)
                throw new Exception.ParentAlreadyHasChildException("Cannot add an inner error to an error that already has one");

            if (innerError.outerErrorId != 0)
                throw new Exception.ChildAlreadyHasParentException("Cannot set the outer error of a child that already has one");

            InnerError = innerError;
            innerError.OuterError = this;

            Data.Error.AddInnerError(Id, innerError.Id);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns all top-level errors in the database
        /// </summary>
        /// <returns>Array containing all top-level errors in the database</returns>
        public static Error[] GetErrors()
        {
            return Data.Error.GetErrors();
        }

        /// <summary>
        /// Generates a new error object based on a System.Exception object
        /// </summary>
        /// <param name="e">System.Exception object used to generate the error</param>
        /// <returns>Error object generated from System.Exception object</returns>
        public static Error FromException(System.Exception e)
        {
            Error child = new Error();

            child.Name = Business.Global.RemoveHtmlTags(e.GetType().Name);
            child.Message = Business.Global.RemoveHtmlTags(e.Message);
            child.StrackTrace = Business.Global.RemoveHtmlTags(e.StackTrace);

            return child;
        }

        /// <summary>
        /// Converts a System.Exception to an Error object and saves it to the database, including inner exceptions
        /// </summary>
        /// <param name="e">System.Exception object used to generate the error</param>
        /// <returns>Error object generated from System.Exception object</returns>
        public static Error SaveFromExceptionIncludingInnerErrors(System.Exception e)
        {
            Error parent = null;

            //traverse down inner exceptions
            do
            {
                Error child = FromException(e);
                child.Save();

                if (parent != null)
                    parent.AddInnerError(child);

                parent = child;

                e = e.InnerException;
            }
            while (e != null);

            return parent;
        }

        #endregion

        #region DbObject Memebers

        protected override void ExecuteAddStoredProcedure()
        {
            Id = Data.Error.AddError(this);
        }

        protected override void ExecuteUpdateStoredProcedure()
        {
            throw new NotSupportedException("Cannot update an error");
        }

        protected override void ExecuteDeleteStoredProcedure()
        {
            if (InnerError != null)
                InnerError.Delete();

            Data.Error.DeleteError(Id);
        }

        protected override bool ValidateProperties(out Exception.InvalidPropertyException error)
        {
            error = null;

            if (String.IsNullOrEmpty(Name))
                error = new Exception.InvalidPropertyException("Error.Name property cannot be empty");

            if (String.IsNullOrEmpty(Message))
                error = new Exception.InvalidPropertyException("Error.Message property cannot be empty");

            return (error == null);
        }

        protected override void ResetIdToDefaultValue()
        {
            Id = -1;
        }

        internal override Data.StoredProcedureParameter[] GetParametersForStoredProcedure(bool updatedStoredProc)
        {
            List<Data.StoredProcedureParameter> parameters = new List<Data.StoredProcedureParameter>();

            parameters.Add(Data.Control.GetParameter("Name", DbType.String, Name));
            parameters.Add(Data.Control.GetParameter("Message", DbType.String, Message));
            parameters.Add(Data.Control.GetParameter("StackTrace", DbType.String, StrackTrace));
            parameters.Add(Data.Control.GetParameter("OuterErrorId", DbType.Int16, outerErrorId));
            parameters.Add(Data.Control.GetParameter("InnerErrorId", DbType.Int16, innerErrorId));
            parameters.Add(Data.Control.GetParameter("DateThrown", DbType.DateTime, DateThrown));

            return parameters.ToArray();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NisbetPhotography.DbObjects.Business
{
    public abstract class DbObject
    {
        #region Properties

        /// <summary>
        /// Determines whether this database object currently exists in the database
        /// </summary>
        public Boolean IsInDatabase { get; internal set; }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Saves the details of the database object to the database
        /// </summary>
        public void Save()
        {
            Exception.InvalidPropertyException e;

            if (!(ValidateProperties(out e)))
                throw e;

            if (IsInDatabase)
                ExecuteUpdateStoredProcedure();
            else
            {
                IsInDatabase = true;

                try
                {
                    ExecuteAddStoredProcedure();
                }
                catch (System.Exception ex)
                {
                    IsInDatabase = false;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Deletes the details of the database object from the database
        /// </summary>
        public void Delete()
        {
            if (IsInDatabase)
                ExecuteDeleteStoredProcedure();

            IsInDatabase = false;
            ResetIdToDefaultValue();
        }

        #region Abstract Instance Methods

        /// <summary>
        /// Executes the stored procedure that adds the object to the database
        /// </summary>
        protected abstract void ExecuteAddStoredProcedure();

        /// <summary>
        /// Executes the stored procedure that updates the row in the database representing the object
        /// </summary>
        protected abstract void ExecuteUpdateStoredProcedure();

        /// <summary>
        /// Executes the stored procedure that deletes the row from the database representing the object
        /// </summary>
        protected abstract void ExecuteDeleteStoredProcedure();

        /// <summary>
        /// Determines whether the current properties for the object are valid
        /// </summary>
        /// <param name="error">Exception thrown if property is invalid</param>
        /// <returns>True if properties are valid, false if not</returns>
        protected abstract Boolean ValidateProperties(out Exception.InvalidPropertyException error);

        /// <summary>
        /// Sets the unique identifier of the object back to it's default value
        /// </summary>
        protected abstract void ResetIdToDefaultValue();

        /// <summary>
        /// Creates an array of stored procedure parameters representing each property of the object
        /// </summary>
        /// <param name="updatedStoredProc">Determines whether or not the parameters are being passed to an Update stored procedure</param>
        /// <returns>Array of stored procedure parameters representing each property of the object</returns>
        internal abstract Data.StoredProcedureParameter[] GetParametersForStoredProcedure(Boolean updatedStoredProc);

        #endregion

        #endregion
    }
}

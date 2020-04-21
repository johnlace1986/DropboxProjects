using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Data;
using Utilities.Exception;
using sp = Utilities.Data.StoredProcedure;
using sql = Utilities.Data.SQL;
using Utilities.Business;

namespace Wedding.eVite.Data
{
    internal static class Error
    {
        #region Static Methods

        /// <summary>
        /// Loads the error with the specified unique identifier from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="errorId">Unique identifier of the error</param>
        /// <returns>Error with the specified unique identifier, loaded from the database</returns>
        public static Business.Error GetErrorById(SqlConnection conn, Int32 errorId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetErrorById", conn);
                cmd.Parameters.AddWithValue("ErrorId", DbType.Int32, errorId);

                return cmd.ExecuteMappedSingleReader<Business.Error, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load error", e);
            }
        }

        /// <summary>
        /// Gets all errors currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All errors currently in the system</returns>
        public static Business.Error[] GetErrors(SqlConnection conn)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetErrors", conn);                
                return cmd.ExecuteMappedReader<Business.Error, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load errors", e);
            }
        }

        /// <summary>
        /// Gets the inner error of the specified error from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="outerError">Outer error</param>
        /// <returns>Inner error of the specified error, loaded from the database</returns>
        public static Business.Error GetErrorByOuterError(SqlConnection conn, Business.Error outerError)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetErrorByOuterErrorId", conn);
                cmd.Parameters.AddWithValue("OuterErrorId", DbType.Int32, outerError.Id);

                Business.Error[] errors = cmd.ExecuteMappedReader<Business.Error, Int32>();

                switch (errors.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return errors[0];
                    default:
                        throw new SpecifiedSqlDbObjectNotFoundException(String.Format("{0} rows found in the database", errors.Length));
                }
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load error", e);
            }
        }
        
        #endregion
    }
}

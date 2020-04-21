using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exception;
using sql = Utilities.Data.SQL;
namespace Wedding.eVite.Data
{
    internal static class Table
    {
        #region Static Methods

        /// <summary>
        /// Gets all tables currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All tables currently in the system</returns>
        public static Business.Table[] GetTables(SqlConnection conn)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetTables", conn);

                return cmd.ExecuteMappedReader<Business.Table, Int32>();
            }
            catch(System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get tables", e);
            }
        }

        #endregion
    }
}

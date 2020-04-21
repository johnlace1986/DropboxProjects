using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Data;
using Utilities.Exception;
using Utilities.Data.StoredProcedure;
using sql = Utilities.Data.SQL;

namespace Wedding.eVite.Data
{
    internal static class Invite
    {
        #region Static Methods

        /// <summary>
        /// Gets all invites currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All invites currently in the system</returns>
        public static Business.Invite[] GetInvites(SqlConnection conn)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetInvites", conn);
                return cmd.ExecuteMappedReader<Business.Invite, Int32>();
            }
            catch(System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load invites", e);
            }
        }

        /// <summary>
        /// Loads the invite with the specified unique identifier from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <returns>Invite with the specified unique identifier, loaded from the database</returns>
        public static Business.Invite GetInviteById(SqlConnection conn, Int32 inviteId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetInviteById", conn);
                cmd.Parameters.AddWithValue("InviteId", DbType.Int32, inviteId);

                return cmd.ExecuteMappedSingleReader<Business.Invite, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load invite", e);
            }
        }
        
        #endregion
    }
}

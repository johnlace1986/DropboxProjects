using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Data;
using Utilities.Exception;
using Utilities.Data.StoredProcedure;
using sql = Utilities.Data.SQL;
using System.Data;

namespace Wedding.eVite.Data
{
    internal static class Message
    {
        #region Static Methods

        /// <summary>
        /// Loads the messages that belong to the invite with the specified unique identifier from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <returns>Messages that belong to the invite with the specified unique identifier from the database</returns>
        internal static Business.Message[] GetMessagesByInviteId(SqlConnection conn, Int32 inviteId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetMessagesByInviteId", conn);
                cmd.Parameters.AddWithValue("InviteId", DbType.Int32, inviteId);

                return cmd.ExecuteMappedReader<Business.Message, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load messages.", e);
            }
        }

        /// <summary>
        /// Loads the unread messages that belong to the invite with the specified unique identifier from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <param name="isRequesterAdmin">Value determining whether or not the invite requesting the messages is an admin user</param>
        /// <returns>Unread messages that belong to the invite with the specified unique identifier from the database</returns>
        internal static Business.Message[] GetUnreadMessagesByInviteId(SqlConnection conn, Int32 inviteId, Boolean isRequesterAdmin)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetUnreadMessagesByInviteId", conn);
                cmd.Parameters.AddWithValue("InviteId", DbType.Int32, inviteId);
                cmd.Parameters.AddWithValue("IsRequesterAdmin", DbType.Boolean, isRequesterAdmin);

                return cmd.ExecuteMappedReader<Business.Message, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load messages.", e);
            }
        }

        /// <summary>
        /// Gets the number of unread messages that belong to the invite with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <param name="isRequesterAdmin">Value determining whether or not the invite requesting the messages is an admin user</param>
        /// <returns>Number of unread messages that belong to the invite with the specified unique identifier</returns>
        internal static Int32 GetInviteUnreadMessageCount(SqlConnection conn, Int32 inviteId, Boolean isRequesterAdmin)
        {
            sql.SqlCommand cmd = new sql.SqlCommand("GetInviteUnreadMessageCount", conn);
            cmd.Parameters.AddWithValue("InviteId", DbType.Int32, inviteId);
            cmd.Parameters.AddWithValue("IsRequesterAdmin", DbType.Boolean, isRequesterAdmin);

            return cmd.ExecuteScalar<Int32>();
        }

        #endregion
    }
}

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
    internal static class Guest
    {
        #region Static Methods

        /// <summary>
        /// Gets the guests that are associated with the invite with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <returns>Guests that are associated with the invite with the specified unique identifier</returns>
        public static Business.Guest[] GetGuestsByInviteId(SqlConnection conn, Int32 inviteId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetGuestsByInviteId", conn);
                cmd.Parameters.AddWithValue("InviteId", DbType.Int32, inviteId);

                return cmd.ExecuteMappedReader<Business.Guest, Int32>((row => Business.Guest.FromDataRow(row)));
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get guests", e);
            }
        }

        /// <summary>
        /// Gets the guests that are assigned to the table with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="tableId">Unique identifier of the table</param>
        /// <returns>Guests that are assigned to the table with the specified unique identifier</returns>
        public static Business.Guest[] GetGuestsByTableId(SqlConnection conn, Int32 tableId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetGuestsByTableId", conn);
                cmd.Parameters.AddWithValue("TableId", DbType.Int32, tableId);

                return cmd.ExecuteMappedReader<Business.Guest, Int32>((row => Business.Guest.FromDataRow(row)));
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get guests.", e);
            }
        }


        /// <summary>
        /// Gets the guests that are assigned to the room with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="roomId">Unique identifier of the room</param>
        /// <returns>Guests that are assigned to the room with the specified unique identifier</returns>
        public static Business.Guest[] GetGuestsByRoomId(SqlConnection conn, Int32 roomId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetGuestsByRoomId", conn);
                cmd.Parameters.AddWithValue("RoomId", DbType.Int32, roomId);

                return cmd.ExecuteMappedReader<Business.Guest, Int32>((row => Business.Guest.FromDataRow(row)));
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get guests.", e);
            }
        }

        /// <summary>
        /// Gets the guests that have not yet been assigned to a table
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Guests that have not yet been assigned to a table</returns>
        public static Business.Guest[] GetUnassignedTableGuests(SqlConnection conn)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetUnassignedTableGuests", conn);

                return cmd.ExecuteMappedReader<Business.Guest, Int32>((row => Business.Guest.FromDataRow(row)));
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get guests", e);
            }
        }

        /// <summary>
        /// Gets the guests that have not yet been assigned to a room
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Guests that have not yet been assigned to a room</returns>
        public static Business.Guest[] GetUnassignedRoomGuests(SqlConnection conn)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetUnassignedRoomGuests", conn);

                return cmd.ExecuteMappedReader<Business.Guest, Int32>((row => Business.Guest.FromDataRow(row)));
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get guests", e);
            }
        }

        /// <summary>
        /// Adds a guest to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite the guest is a member of</param>
        /// <param name="guest">Guest being added to the database</param>
        /// <returns>Unique identifier of the guest, generated by the database</returns>
        public static Int32 AddGuest(SqlConnection conn, Int32 inviteId, Business.Guest guest)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("AddGuest", conn);
                cmd.Parameters.AddRange(guest.GetParametersForStoredProcedure(inviteId, false));

                return cmd.ExecuteScalar<Int32>();
            }
            catch(System.Exception e)
            {
                throw new AddSqlDbObjectException("Could not add guest", e);
            }
        }

        /// <summary>
        /// Updates a guest in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite the guest is a member of</param>
        /// <param name="guest">Guest being updated in the database</param>
        public static void UpdateGuest(SqlConnection conn, Int32 inviteId, Business.Guest guest)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("UpdateGuest", conn);
                cmd.Parameters.AddRange(guest.GetParametersForStoredProcedure(inviteId, true));

                cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not update guest", e);
            }
        }

        /// <summary>
        /// Deletes a guest from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="guest">Guest being deleted from the database</param>
        public static void DeleteGuest(SqlConnection conn, Int32 guestId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("DeleteGuest", conn);
                cmd.Parameters.AddWithValue("GuestId", DbType.Int32, guestId);

                cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                throw new DeleteSqlDbObjectException("Could not delete guest", e);
            }
        }

        /// <summary>
        /// Sets the table ID of the guest with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="guestId">Unique identifier of the guest</param>
        /// <param name="tableId">Unique identifier of the table</param>
        public static void SetGuestTableId(SqlConnection conn, Int32 guestId, Int32? tableId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("SetGuestTableId", conn);
                cmd.Parameters.AddWithValue("GuestId", DbType.Int32, guestId);
                cmd.Parameters.AddWithValue("TableId", DbType.Int32, tableId);

                cmd.ExecuteNonQuery();
            }
            catch(System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not set guest table ID", e);
            }
        }

        /// <summary>
        /// Sets the room ID of the guest with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="guestId">Unique identifier of the guest</param>
        /// <param name="roomId">Unique identifier of the room</param>
        public static void SetGuestRoomId(SqlConnection conn, Int32 guestId, Int32? roomId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("SetGuestRoomId", conn);
                cmd.Parameters.AddWithValue("GuestId", DbType.Int32, guestId);
                cmd.Parameters.AddWithValue("RoomId", DbType.Int32, roomId);

                cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not set guest room ID", e);
            }
        }

        #endregion
    }
}

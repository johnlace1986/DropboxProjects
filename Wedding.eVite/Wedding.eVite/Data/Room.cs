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
    internal static class Room
    {
        #region Static Methods

        /// <summary>
        /// Gets the rooms currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Rooms currently in the system</returns>
        public static Business.Room[] GetRooms(SqlConnection conn)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetRooms", conn);

                return cmd.ExecuteMappedReader<Business.Room, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load rooms.", e);
            }
        }

        /// <summary>
        /// Loads the room with the specified unique identifier from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="roomId">Unique identifier of the room</param>
        /// <returns>Room with the specified unique identifier, loaded from the database</returns>
        internal static Business.Room GetRoomById(SqlConnection conn, Int32 roomId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetRoomById", conn);
                cmd.Parameters.AddWithValue("RoomId", DbType.Int32, roomId);

                return cmd.ExecuteMappedSingleReader<Business.Room, Int32>();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load room.", e);
            }
        }
        #endregion
    }
}

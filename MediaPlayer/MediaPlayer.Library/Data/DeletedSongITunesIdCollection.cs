using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utilities.Data;
using Utilities.Exception;
using sp = Utilities.Data.StoredProcedure;

namespace MediaPlayer.Library.Data
{
    internal static class DeletedSongITunesIdCollection
    {
        #region Static Methods

        /// <summary>
        /// Gets all deleted song iTunes IDs from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All deleted song iTunes IDs from the database</returns>
        public static Int16[] GetDeletedSongITunesIds(SqlConnection conn)
        {
            try
            {
                sp.DataTable table = SqlDbObject.ExecuteReader("GetDeletedSongITunesIds", CommandType.StoredProcedure, conn);

                List<Int16> deletedSongITunesIds = new List<Int16>();

                foreach (sp.DataRow row in table)
                    deletedSongITunesIds.Add((Int16)row["iTunesId"]);

                deletedSongITunesIds.Sort();
                return deletedSongITunesIds.ToArray();
            }
            catch(System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not get deleted song iTunes IDs", e);
            }
        }

        /// <summary>
        /// Adds a deleted song iTunes ID to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="iTunesId">iTunes ID being added to the database</param>
        public static void AddDeletedSongITunesId(SqlConnection conn, Int16 iTunesId)
        {
            try
            {
                SqlDbObject.ExecuteNonQuery("AddDeletedSongITunesId", CommandType.StoredProcedure, new sp.Parameter("iTunesId", DbType.Int16, iTunesId), conn);
            }
            catch(System.Exception e)
            {
                throw new AddSqlDbObjectException("Could not add deleted song iTunes ID", e);
            }
        }

        /// <summary>
        /// Deletes a deleted song iTunes ID from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="iTunesId">iTunes ID being deleted from the database</param>
        public static void DeleteDeletedSongITunesId(SqlConnection conn, Int16 iTunesId)
        {
            try
            {
                SqlDbObject.ExecuteNonQuery("DeleteDeletedSongITunesId", CommandType.StoredProcedure, new sp.Parameter("iTunesId", DbType.Int16, iTunesId), conn);
            }
            catch(System.Exception e)
            {
                throw new DeleteSqlDbObjectException("Could not delete deleted song iTunes ID", e);
            }
        }

        #endregion
    }
}

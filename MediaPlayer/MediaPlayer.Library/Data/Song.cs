using System;
using System.Linq;
using System.Data;
using Utilities.Exception;
using sp = Utilities.Data.StoredProcedure;
using Utilities.Data;
using System.Data.SqlClient;

namespace MediaPlayer.Library.Data
{
    internal static class Song
    {
        #region Static Methods

        /// <summary>
        /// Loads a song from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="songId">Unique identifier of the song being loaded</param>
        /// <returns>Song object loaded from the database</returns>
        public static Business.Song GetSongById(SqlConnection conn, Int64 songId)
        {
            try
            {
                sp.DataTable dt = SqlDbObject.ExecuteReader("GetSongById", CommandType.StoredProcedure, new sp.Parameter("SongId", DbType.Int64, songId), conn);

                if (dt.RowCount == 1)
                    return Business.MediaItem.FromDataRow(conn, dt[0]) as Business.Song;
                else
                    throw new SpecifiedSqlDbObjectNotFoundException("GetSongById returned " + dt.RowCount.ToString() + " rows");
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load specified song", e);
            }
        }

        /// <summary>
        /// Adds the specified song to the database and returns the Id that was generated
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="song">Song object being added to the database</param>
        /// <returns>New unique identifier of the song</returns>
        public static Int64 AddSong(SqlConnection conn, Business.Song song)
        {
            try
            {
                return (Int64)SqlDbObject.ExecuteScalar("AddSong", CommandType.StoredProcedure, song.GetMediaItemParametersForStoredProcedure(false), conn);
            }
            catch (System.Exception e)
            {
                throw new AddSqlDbObjectException("Could not add song to database", e);
            }
        }

        /// <summary>
        /// Updates the specified song in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="song">Song object being updated in the database</param>
        public static void UpdateSong(SqlConnection conn, Business.Song song)
        {
            try
            {
                SqlDbObject.ExecuteNonQuery("UpdateSong", CommandType.StoredProcedure, song.GetMediaItemParametersForStoredProcedure(true), conn);
            }
            catch (System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not update song in database", e);
            }
        }



        #endregion
    }
}

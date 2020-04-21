using System;
using System.Linq;
using System.Data;
using Utilities.Exception;
using sp = Utilities.Data.StoredProcedure;
using Utilities.Data;
using System.Data.SqlClient;
using sql = Utilities.Data.SQL;

namespace MediaPlayer.Library.Data
{
    internal static class Video
    {
        #region Static Methods

        /// <summary>
        /// Loads a video from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="videoId">Unique identifier of the video being loaded</param>
        /// <returns>Video object loaded from the database</returns>
        public static Business.Video GetVideoById(SqlConnection conn, Int64 videoId)
        {
            try
            {
                sql.SqlCommand cmd = new sql.SqlCommand("GetVideoById", conn);
                cmd.Parameters.AddWithValue("VideoId", DbType.Int64, videoId);

                return cmd.ExecuteMappedSingleReader<Business.Video>(row => Business.MediaItem.FromDataRow(conn, row) as Business.Video);
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load specified video", e);
            }
        }

        /// <summary>
        /// Adds the specified video to the database and returns the Id that was generated
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="video">Video object being added to the database</param>
        /// <returns>New unique identifier of the video</returns>
        public static Int64 AddVideo(SqlConnection conn, Business.Video video)
        {
            try
            {
                return (Int64)SqlDbObject.ExecuteScalar("AddVideo", CommandType.StoredProcedure, video.GetMediaItemParametersForStoredProcedure(false), conn);
            }
            catch (System.Exception e)
            {
                throw new AddSqlDbObjectException("Could not add video to database", e);
            }
        }

        /// <summary>
        /// Updates the specified video in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="video">Video object being updated in the database</param>
        public static void UpdateVideo(SqlConnection conn, Business.Video video)
        {
            try
            {
                SqlDbObject.ExecuteNonQuery("UpdateVideo", CommandType.StoredProcedure, video.GetMediaItemParametersForStoredProcedure(true), conn);
            }
            catch (System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not update video in database", e);
            }
        }

        #endregion
    }
}

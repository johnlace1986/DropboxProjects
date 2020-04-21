using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Utilities.Business;
using Utilities.Exception;
using sp = Utilities.Data.StoredProcedure;
using Utilities.Data;
using System.Data.SqlClient;

namespace MediaPlayer.Library.Data
{
    internal static class MediaItem
    {
        #region Static Methods

        /// <summary>
        /// Gets all media items in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All media items in the system</returns>
        public static Business.MediaItem[] GetMediaItems(SqlConnection conn)
        {
            try
            {
                sp.DataTable dt = SqlDbObject.ExecuteReader("GetMediaItems", CommandType.StoredProcedure, conn);
                List<Business.MediaItem> mediaItems = new List<Business.MediaItem>();

                foreach (sp.DataRow dr in dt)
                    mediaItems.Add(Business.MediaItem.FromDataRow(conn, dr));

                mediaItems.Sort();

                return mediaItems.ToArray();
            }
            catch(System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load media items", e);
            }
        }

        /// <summary>
        /// Deletes the media item with the specified Id from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item being deleted</param>
        /// <param name="type">Type of media item being deleted</param>
        public static void DeleteMediaItem(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum type)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("Type", DbType.Int16, Convert.ToInt16(type)));

                SqlDbObject.ExecuteNonQuery("DeleteMediaItem", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new DeleteSqlDbObjectException("Could not delete media item from database", e);
            }
        }

        /// <summary>
        /// Loads the play history for the specfied media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item having it's play history loaded</param>
        /// <param name="mediaItemType">Type of media item having it's play history loaded</param>
        /// <returns>Collection of dates and times the specified media item has been played, sorted ascendingly</returns>
        public static DateTime[] GetMediaItemPlayHistoryById(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));

                sp.DataTable table = SqlDbObject.ExecuteReader("GetMediaItemPlayHistoryById", CommandType.StoredProcedure, parameters.ToArray(), conn);
                List<DateTime> playHistory = new List<DateTime>();

                foreach (sp.DataRow row in table)
                    playHistory.Add((DateTime)row["DatePlayed"]);

                playHistory.Sort();
                return playHistory.ToArray();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load play history for media item", e);
            }
        }

        /// <summary>
        /// Adds a date and time to the specified media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item having it's play history updated</param>
        /// <param name="mediaItemType">Type of media item having it's play history updated</param>
        /// <param name="datePlayed">Date and time the media item was played to the end</param>
        public static void AddMediaItemPlayHistory(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType, DateTime datePlayed)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("DatePlayed", DbType.DateTime, datePlayed));

                SqlDbObject.ExecuteNonQuery("AddMediaItemPlayHistory", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new AddChildSqlDbObjectToParentException("Could not add play history item", e);
            }
        }

        /// <summary>
        /// Loads the tags for the specfied media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item having it's tags loaded</param>
        /// <param name="mediaItemType">Type of media item having it's tags loaded</param>
        /// <returns>Collection of tags assigned to the specified media item, sorted ascendingly</returns>
        public static IntelligentString[] GetMediaItemTagsById(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));

                sp.DataTable table = SqlDbObject.ExecuteReader("GetMediaItemTagsById", CommandType.StoredProcedure, parameters.ToArray(), conn);
                List<IntelligentString> tags = new List<IntelligentString>();

                foreach (sp.DataRow row in table)
                    tags.Add((String)row["Tag"]);

                tags.Sort();
                return tags.ToArray();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load tags for media item", e);
            }
        }

        /// <summary>
        /// Adds a tag to the specfied media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item the tag is being added to</param>
        /// <param name="mediaItemType">Type of media item the tag is being added to</param>
        /// <param name="tag">Tag being added to the media item</param>
        public static void AddMediaItemTag(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType, String tag)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Tag", DbType.String, tag));

                SqlDbObject.ExecuteNonQuery("AddMediaItemTag", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not add tag to media item", e);
            }
        }

        /// <summary>
        /// Deletes a tag from the specfied media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item the tag is being added to</param>
        /// <param name="mediaItemType">Type of media item the tag is being added to</param>
        /// <param name="tag">Tag being added to the media item</param>
        public static void DeleteMediaItemTag(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType, String tag)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Tag", DbType.String, tag));

                SqlDbObject.ExecuteNonQuery("DeleteMediaItemTag", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not delete tag from media item", e);
            }
        }

        #endregion
    }
}

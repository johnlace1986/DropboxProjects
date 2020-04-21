using System;
using System.Collections.Generic;
using System.Linq;
using sp = Utilities.Data.StoredProcedure;
using System.Data;
using Utilities.Business;
using Utilities.Exception;
using Utilities.Data;
using System.Data.SqlClient;

namespace MediaPlayer.Library.Data
{
    internal static class MediaItemPart
    {
        /// <summary>
        /// Loads the parts for the specfied media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item</param>
        /// <param name="mediaItemType">Type of the media item</param>
        /// <returns>Collection of parts belonging to the specified media item, sorted ascendingly</returns>
        public static Business.MediaItemPartCollection GetMediaItemPartsById(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));

                sp.DataTable table = SqlDbObject.ExecuteReader("GetMediaItemPartsById", CommandType.StoredProcedure, parameters.ToArray(), conn);
                Business.MediaItemPartCollection parts = new Business.MediaItemPartCollection();

                foreach (sp.DataRow row in table)
                {
                    IntelligentString location = (String)row["Location"];
                    Int64 size = (Int64)row["Size"];
                    Int32 milliseconds = (Int32)row["Duration"];
                    Int16 index = (Int16)row["Index"];

                    parts.Add(location, size, milliseconds, index);
                }

                parts.Sort();
                return parts;
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load play history for media item", e);
            }
        }

        /// <summary>
        /// Adds a part to the specified media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item the part is being added to</param>
        /// <param name="type">Type of media item being the part is being added to</param>
        /// <param name="part">The media item part being added to the media item</param>
        public static void AddMediaItemPart(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType, Business.MediaItemPart part)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("Location", DbType.String, part.Location.Value));
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Size", DbType.Int64, part.Size));
                parameters.Add(new sp.Parameter("Duration", DbType.Int32, part.Duration.TotalMilliseconds));
                parameters.Add(new sp.Parameter("Index", DbType.Int16, part.Index));

                SqlDbObject.ExecuteNonQuery("AddMediaItemPart", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new AddChildSqlDbObjectToParentException("Could not add part to media item", e);
            }
        }

        /// <summary>
        /// Updates a media item part in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemId">Unique identifier of the media item the part belongs to</param>
        /// <param name="type">Type of media item being the part belongs to</param>
        /// <param name="part">The media item part being updated in the database</param>
        public static void UpdateMediaItemPart(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType, Business.MediaItemPart part)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("Location", DbType.String, part.Location.Value));
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Size", DbType.Int64, part.Size));
                parameters.Add(new sp.Parameter("Duration", DbType.Int32, part.Duration.TotalMilliseconds));
                parameters.Add(new sp.Parameter("Index", DbType.Int16, part.Index));

                SqlDbObject.ExecuteNonQuery("UpdateMediaItemPart", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not update media item part", e);
            }
        }

        /// <summary>
        /// Deletes a media item part from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="location">Location of the part being deleted</param>
        public static void DeleteMediaItemPart(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType, String location)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("Location", DbType.String, location));
                parameters.Add(new sp.Parameter("MediaItemId", DbType.Int64, mediaItemId));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));

                SqlDbObject.ExecuteNonQuery("DeleteMediaItemPart", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new DeleteSqlDbObjectException("Could not delete media item part", e);
            }
        }

        /// <summary>
        /// Determines whether or a part already exists in the database with the specified location
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="location">Location to the check for</param>
        /// <returns>True if a part already exists in the database with the specified location, false if not</returns>
        public static Boolean MediaItemPartLocationExists(SqlConnection conn, String location)
        {
            return (Boolean)SqlDbObject.ExecuteScalar("MediaItemPartLocationExists", CommandType.StoredProcedure, new sp.Parameter("Location", DbType.String, location), conn);
        }
    }
}

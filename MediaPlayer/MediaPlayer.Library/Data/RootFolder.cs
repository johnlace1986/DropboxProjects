using System;
using System.Collections.Generic;
using System.Linq;
using sp = Utilities.Data.StoredProcedure;
using Utilities.Business;
using System.Data;
using Utilities.Exception;
using Utilities.Data;
using System.Data.SqlClient;

namespace MediaPlayer.Library.Data
{
    internal static class RootFolder
    {
        #region Static Methods

        /// <summary>
        /// Determines whether a root folder already exists with specified type and path
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemType">Type associated with the desired root folders</param>
        /// <param name="path">Path to the desired root folder</param>
        /// <returns>True if a root folder already exists with specified type and path, false if not</returns>
        public static Boolean RootFolderPathExists(SqlConnection conn, Business.MediaItemTypeEnum mediaItemType, String path)
        {
            List<sp.Parameter> parameters = new List<sp.Parameter>();
            parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
            parameters.Add(new sp.Parameter("Path", DbType.String, path));

            return (Boolean)SqlDbObject.ExecuteScalar("RootFolderPathExists", CommandType.StoredProcedure, parameters.ToArray(), conn);
        }

        /// <summary>
        /// Gets all root folders in the system that are associated with the specified type
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemType">Type associated with the desired root folders</param>
        /// <returns>All root folders in the system that are associated with the specified type</returns>
        public static Business.RootFolder[] GetRootFoldersByType(SqlConnection conn, Business.MediaItemTypeEnum mediaItemType)
        {
            try
            {
                sp.DataTable dt = SqlDbObject.ExecuteReader("GetRootFoldersByType", CommandType.StoredProcedure, new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)), conn);
                List<Business.RootFolder> rootFolders = new List<Business.RootFolder>();

                foreach (sp.DataRow row in dt)
                    rootFolders.Add(Business.RootFolder.FromDataRow(conn, row));

                rootFolders.Sort();
                return rootFolders.ToArray();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load root folders", e);
            }
        }

        /// <summary>
        /// Gets the root folder from the database with the specified priority
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemType">Type of media item the desired root folder is associated with</param>
        /// <param name="priority">Priority of the desired root folder</param>
        /// <returns>Root folder in the database with the specified priority</returns>
        public static Business.RootFolder GetRootFolderByPriority(SqlConnection conn, Business.MediaItemTypeEnum mediaItemType, Int16 priority)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Priority", DbType.Int16, priority));

                sp.DataTable dt = SqlDbObject.ExecuteReader("GetRootFolderByPriority", CommandType.StoredProcedure, parameters.ToArray(), conn);

                if (dt.RowCount == 1)
                    return Business.RootFolder.FromDataRow(conn, dt[0]);
                else
                    throw new SpecifiedSqlDbObjectNotFoundException("GetRootFolderByPriority returned " + dt.RowCount.ToString() + " rows");
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load specified root folder", e);
            }
        }

        /// <summary>
        /// Loads the tags for the specfied root folder
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemType">Type associated with the root folder the tags are being loaded for</param>
        /// <param name="rootFolderPriority">Priority of the root folder the tags are being loaded for</param>
        /// <returns>Collection of tags assigned to the specified root folder, sorted ascendingly</returns>
        public static IntelligentString[] GetRootFolderTagsByPriority(SqlConnection conn, Business.MediaItemTypeEnum mediaItemType, Int16 rootFolderPriority)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("RootFolderPriority", DbType.Int16, rootFolderPriority));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));

                sp.DataTable table = SqlDbObject.ExecuteReader("GetRootFolderTagsByPriority", CommandType.StoredProcedure, parameters.ToArray(), conn);
                List<IntelligentString> tags = new List<IntelligentString>();

                foreach (sp.DataRow row in table)
                    tags.Add((String)row["Tag"]);

                tags.Sort();
                return tags.ToArray();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load tags for root folder", e);
            }
        }

        /// <summary>
        /// Adds a new root folder to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="rootFolder">Root folder being added</param>
        /// <returns>Priority of the new root folder</returns>
        public static Int16 AddRootFolder(SqlConnection conn, Business.RootFolder rootFolder)
        {
            try
            {
                return (Int16)SqlDbObject.ExecuteScalar("AddRootFolder", CommandType.StoredProcedure, rootFolder.GetParametersForStoredProcedure(false), conn);
            }
            catch (System.Exception e)
            {
                throw new AddSqlDbObjectException("Could not add root folder", e);
            }
        }

        /// <summary>
        /// Updates a root folder in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="rootFolder">Root folder being updated</param>
        public static void UpdateRootFolder(SqlConnection conn, Business.RootFolder rootFolder)
        {
            try
            {
                SqlDbObject.ExecuteNonQuery("UpdateRootFolder", CommandType.StoredProcedure, rootFolder.GetParametersForStoredProcedure(true), conn);
            }
            catch (System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not update root folder", e);
            }
        }

        /// <summary>
        /// Deletes a root folder from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemType">Type associated with the root folder being deleted</param>
        /// <param name="priority">Priority of the root folder being deleted</param>
        public static void DeleteRootFolder(SqlConnection conn, Business.MediaItemTypeEnum mediaItemType, Int16 priority)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Priority", DbType.Int16, priority));

                SqlDbObject.ExecuteNonQuery("DeleteRootFolder", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new DeleteSqlDbObjectException("Could not delete root folder", e);
            }
        }

        /// <summary>
        /// Adds a tag to the specfied root folder
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemType">Type associated with the root folder the tag is being added to</param>
        /// <param name="rootFolderPriority">Priority of the root folder the tags are being loaded for</param>
        /// <param name="tag">Tag being added to the root folder</param>
        public static void AddRootFolderTag(SqlConnection conn, Business.MediaItemTypeEnum mediaItemType, Int16 rootFolderPriority, String tag)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("RootFolderPriority", DbType.Int16, rootFolderPriority));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Tag", DbType.String, tag));

                SqlDbObject.ExecuteNonQuery("AddRootFolderTag", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not add tag to root folder", e);
            }
        }

        /// <summary>
        /// Deletes a tag from the specfied root folder
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="mediaItemType">Type associated with the root folder the tag is being deleted from</param>
        /// <param name="rootFolderPriority">Priority of the root folder the tags are being loaded for</param>
        /// <param name="tag">Tag being added to the root folder</param>
        public static void DeleteRootFolderTag(SqlConnection conn, Business.MediaItemTypeEnum mediaItemType, Int16 rootFolderPriority, String tag)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("RootFolderPriority", DbType.Int16, rootFolderPriority));
                parameters.Add(new sp.Parameter("MediaItemType", DbType.Int16, Convert.ToInt16(mediaItemType)));
                parameters.Add(new sp.Parameter("Tag", DbType.String, tag));

                SqlDbObject.ExecuteNonQuery("DeleteRootFolderTag", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not delete tag from root folder", e);
            }
        }

        #endregion
    }
}

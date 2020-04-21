using System;
using System.Collections.Generic;
using System.Linq;
using sp = Utilities.Data.StoredProcedure;
using System.Data;
using Utilities.Exception;
using Utilities.Data;
using System.Data.SqlClient;

namespace MediaPlayer.Library.Data
{
    internal static class FileType
    {
        #region Static Methods

        /// <summary>
        /// Loads a file type from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="fileTypeId">Unique identifier of the file type being loaded</param>
        /// <returns>FileType object loaded from the database</returns>
        public static Business.FileType GetFileTypeById(SqlConnection conn, Int16 fileTypeId)
        {
            try
            {
                sp.DataTable dt = SqlDbObject.ExecuteReader("GetFileTypeById", CommandType.StoredProcedure, new sp.Parameter("FileTypeId", DbType.Int16, fileTypeId), conn);

                if (dt.RowCount == 1)
                    return Business.FileType.FromDataRow(conn, dt[0]);
                else
                    throw new SpecifiedSqlDbObjectNotFoundException("GetFileTypeById returned " + dt.RowCount.ToString() + " rows");
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load specified file type", e);
            }
        }

        /// <summary>
        /// Gets all file types in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All file types in the system</returns>
        public static Business.FileType[] GetFileTypes(SqlConnection conn)
        {
            try
            {
                sp.DataTable dt = SqlDbObject.ExecuteReader("GetFileTypes", CommandType.StoredProcedure, conn);
                List<Business.FileType> fileTypes = new List<Business.FileType>();

                foreach (sp.DataRow dr in dt)
                    fileTypes.Add(Business.FileType.FromDataRow(conn, dr));

                fileTypes.Sort();
                return fileTypes.ToArray();
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load file types", e);
            }
        }

        /// <summary>
        /// Adds the specified file type to the database and returns the Id that was generated
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="fileType">FileType object being added to the database</param>
        /// <returns>New unique identifier of the file type</returns>
        public static Int16 AddFileType(SqlConnection conn, Business.FileType fileType)
        {
            try
            {
                return (Int16)SqlDbObject.ExecuteScalar("AddFileType", CommandType.StoredProcedure, fileType.GetParametersForStoredProcedure(false), conn);
            }
            catch (System.Exception e)
            {
                throw new AddSqlDbObjectException("Could not file type video to database", e);
            }
        }

        /// <summary>
        /// Updates the specified file type in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="fileType">FileType object being updated in the database</param>
        public static void UpdateFileType(SqlConnection conn, Business.FileType fileType)
        {
            try
            {
                SqlDbObject.ExecuteNonQuery("UpdateFileType", CommandType.StoredProcedure, fileType.GetParametersForStoredProcedure(true), conn);
            }
            catch (System.Exception e)
            {
                throw new UpdateSqlDbObjectException("Could not update file type in database", e);
            }
        }

        /// <summary>
        /// Deletes the file type with the specified Id from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="fileTypeId">Unique identifier of the file type being deleted</param>
        public static void DeleteFileType(SqlConnection conn, Int16 fileTypeId)
        {
            try
            {
                SqlDbObject.ExecuteNonQuery("DeleteFileType", CommandType.StoredProcedure, new sp.Parameter("FileTypeId", DbType.Int64, fileTypeId), conn);
            }
            catch (System.Exception e)
            {
                throw new DeleteSqlDbObjectException("Could not delete file tye from database", e);
            }
        }

        /// <summary>
        /// Gets the extensions in the database for the specified file type
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="fileType">File type who's extensions are being loaded</param>
        /// <returns>Extensions in the database for the specified file type</returns>
        public static Business.FileExtensionCollection GetExtensionsByFileTypeId(SqlConnection conn, Int16 fileTypeId)
        {
            try
            {
                sp.DataTable dt = SqlDbObject.ExecuteReader("GetExtensionsByFileTypeId", CommandType.StoredProcedure, new sp.Parameter("FileTypeId", DbType.Int16, fileTypeId), conn);
                Business.FileExtensionCollection extensions = new Business.FileExtensionCollection();

                foreach (sp.DataRow dr in dt)
                    extensions.extensions.Add((String)dr["Extension"]);

                extensions.Sort();
                return extensions;
            }
            catch (System.Exception e)
            {
                throw new SpecifiedSqlDbObjectNotFoundException("Could not load extensions for file type", e);
            }
        }

        /// <summary>
        /// Adds an extension to a file type
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="fileTypeId">Unique identifier of the file type the extension is being added to</param>
        /// <param name="extension">Extension being added to the file type</param>
        public static void AddExtensionToFileType(SqlConnection conn, Int16 fileTypeId, String extension)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("FileTypeId", DbType.Int16, fileTypeId));
                parameters.Add(new sp.Parameter("Extension", DbType.String, extension));

                SqlDbObject.ExecuteNonQuery("AddExtensionToFileType", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new AddChildSqlDbObjectToParentException("Could not add file type extension", e);
            }
        }

        /// <summary>
        /// Removes an extension from a file type
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="fileTypeId">Unique identifier of the file type the extension is being removed from</param>
        /// <param name="extension">Extension being removed from the file type</param>
        public static void RemoveExtensionFromFileType(SqlConnection conn, Int16 fileTypeId, String extension)
        {
            try
            {
                List<sp.Parameter> parameters = new List<sp.Parameter>();
                parameters.Add(new sp.Parameter("FileTypeId", DbType.Int16, fileTypeId));
                parameters.Add(new sp.Parameter("Extension", DbType.String, extension));

                SqlDbObject.ExecuteNonQuery("RemoveExtensionFromFileType", CommandType.StoredProcedure, parameters.ToArray(), conn);
            }
            catch (System.Exception e)
            {
                throw new RemoveChildSqlDbObjectFromParentException("Could not remove file type extension", e);
            }
        }

        /// <summary>
        /// Determines whether or not a file type currently exists with the specified name
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="name">Name to check for</param>
        /// <returns>True if a file type currently exists with the specified name, false if not</returns>
        public static Boolean FileTypeNameExists(SqlConnection conn, String name)
        {
            return (Boolean)SqlDbObject.ExecuteScalar("FileTypeNameExists", CommandType.StoredProcedure, new sp.Parameter("Name", DbType.String, name), conn);
        }

        #endregion
    }
}

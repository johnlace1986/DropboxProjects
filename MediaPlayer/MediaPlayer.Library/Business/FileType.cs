using System;
using System.Collections.Generic;
using System.Linq;
using sp = Utilities.Data.StoredProcedure;
using Utilities.Exception;
using System.Data;
using Utilities.Data;
using Utilities.Business;
using System.Data.SqlClient;
using Utilities.Data.SQL;
using System.Data.Common;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public class FileType : SqlDbObject<Int16>, IComparable, ICloneable
    {
        #region Fields

        /// <summary>
        /// Gets or sets the name of the file type
        /// </summary>
        private String name;

        /// <summary>
        /// Gets or sets the type of media item the file type is associated with
        /// </summary>
        private MediaItemTypeEnum mediaItemType;

        /// <summary>
        /// Gets or sets the extensions associated with the file type
        /// </summary>
        private FileExtensionCollection extensions;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the file type
        /// </summary>
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the type of media item the file type is associated with
        /// </summary>
        public MediaItemTypeEnum MediaItemType
        {
            get { return mediaItemType; }
            set
            {
                mediaItemType = value;
                OnPropertyChanged("MediaItemType");
            }
        }

        /// <summary>
        /// Gets or sets the extensions associated with the file type
        /// </summary>
        public FileExtensionCollection Extensions
        {
            get { return extensions; }
            private set
            {
                extensions = value;
                OnPropertyChanged("Extensions");
            }
        }

        /// <summary>
        /// Gets the fiter text for the file type
        /// </summary>
        public String FilterText
        {
            get { return Name + "|" + Extensions.FilterText; }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileType class
        /// </summary>
        public FileType()
            : base()
        {
            Id = -1;
            Name = String.Empty;
            MediaItemType = MediaItemTypeEnum.Video;
            Extensions = new FileExtensionCollection();
        }

        /// <summary>
        /// Initialises a new instance of the FileType class with the properties loaded from the database
        /// </summary>
        /// <param name="fileTypeId">Unique identifier of the file type</param>
        public FileType(Int16 fileTypeId)
            : this()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    FileType clone = Data.FileType.GetFileTypeById(conn, fileTypeId);

                    id = clone.Id;
                    Name = clone.Name;
                    MediaItemType = clone.MediaItemType;

                    foreach (IntelligentString extension in clone.Extensions)
                        extensions.Add(extension);

                    IsInDatabase = true;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Determines whether or not a file type currently exists with the specified name
        /// </summary>
        /// <param name="name">Name to check for</param>
        /// <returns>True if a file type currently exists with the specified name, false if not</returns>
        public static Boolean FileTypeNameExists(String name)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return Data.FileType.FileTypeNameExists(conn, name);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Creates a new FileType object using the data in a data row
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="row">DataRow containing the data for the file type</param>
        /// <returns>FileType object with properties loaded from the database</returns>
        internal static FileType FromDataRow(SqlConnection conn, sp.DataRow row)
        {
            FileType fileType = new FileType();
            fileType.id = (Int16)row["Id"];
            fileType.name = (String)row["Name"];
            fileType.mediaItemType = (MediaItemTypeEnum)((Int16)row["MediaItemType"]);
            fileType.extensions = Data.FileType.GetExtensionsByFileTypeId(conn, fileType.Id);

            fileType.IsInDatabase = true;

            return fileType;
        }

        /// <summary>
        /// Gets all file types in the system
        /// </summary>
        /// <returns>All file types in the system</returns>
        public static FileType[] GetFileTypes()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return Data.FileType.GetFileTypes(conn);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region SqlDbObject Members

        protected override bool ValidateProperties(DbConnection conn, out Exception exception)
        {            
            if (String.IsNullOrEmpty(Name))
            {
                exception = new PropertyNotSetException(typeof(FileType), "Name");
                return false;
            }

            if (IsInDatabase)
            {
                FileType clone = new FileType(Id);

                if (clone.Name != Name)
                {
                    if (FileTypeNameExists(Name))
                    {
                        exception = new DuplicatePropertyValueException(typeof(FileType), "Name", Name);
                        return false;
                    }
                }
            }
            else
            {
                if (FileTypeNameExists(Name))
                {
                    exception = new DuplicatePropertyValueException(typeof(FileType), "Name", Name);
                    return false;
                }
            }

            exception = null;
            return true;
        }

        protected override void AddToSqlDatabase(SqlConnection conn)
        {            
            id = Data.FileType.AddFileType(conn, this);

            foreach (IntelligentString extension in Extensions)
                Data.FileType.AddExtensionToFileType(conn, Id, extension.Value);
        }

        protected override void UpdateInSqlDatabase(SqlConnection conn)
        {            
            Data.FileType.UpdateFileType(conn, this);

            FileType clone = new FileType(Id);

            List<IntelligentString> expected = new List<IntelligentString>(Extensions);
            List<IntelligentString> current = new List<IntelligentString>(clone.Extensions);

            while (expected.Count > 0)
            {
                if (current.Contains(expected[0]))
                    current.Remove(expected[0]);
                else
                    Data.FileType.AddExtensionToFileType(conn, Id, expected[0].Value);

                expected.RemoveAt(0);
            }

            foreach (IntelligentString extension in current)
                Data.FileType.RemoveExtensionFromFileType(conn, Id, extension.Value);
        }

        protected override void DeleteFromSqlDatabase(SqlConnection conn)
        {            
            Data.FileType.DeleteFileType(conn, Id);
        }

        protected override void ResetProperties()
        {
            id = -1;
        }

        public override sp.ParameterCollection GetParametersForStoredProcedure(Boolean includeId)
        {
            sp.ParameterCollection parameters = new sp.ParameterCollection();

            if (includeId)
                parameters.AddWithValue("FileTypeId", DbType.Int16, Id);

            parameters.AddWithValue("Name", DbType.String, Name);
            parameters.AddWithValue("MediaItemType", DbType.Int16, Convert.ToInt16(MediaItemType));

            return parameters;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            FileType fileType = obj as FileType;
            return Name.CompareTo(fileType.Name);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return GeneralMethods.Clone<FileType>(this);
        }

        #endregion
    }
}

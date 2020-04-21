using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using sp = Utilities.Data.StoredProcedure;
using System.IO;
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
    public class RootFolder : SqlDbObject, IComparable, ICloneable
    {
        #region Fields

        /// <summary>
        /// Gets or sets the type of media item the root folder is associated with
        /// </summary>
        private MediaItemTypeEnum mediaItemType;

        /// <summary>
        /// Gets or sets the priority of the root folder
        /// </summary>
        internal Int16 priority;

        /// <summary>
        /// Gets or sets the path to the root folder
        /// </summary>
        private IntelligentString path;

        /// <summary>
        /// Gets or sets the tags assigned to the root folder
        /// </summary>
        private ObservableCollection<IntelligentString> tags;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the type of media item the root folder is associated with
        /// </summary>
        public MediaItemTypeEnum MediaItemType
        {
            get { return mediaItemType; }
            private set
            {
                mediaItemType = value;
                OnPropertyChanged("MediaItemType");
            }
        }

        /// <summary>
        /// Gets the priority of the root folder
        /// </summary>
        public Int16 Priority
        {
            get { return priority; }
            set
            {
                priority = value;
                OnPropertyChanged("Priority");
            }
        }

        /// <summary>
        /// Gets the priority of the root folder, expressed as a string
        /// </summary>
        public IntelligentString PriorityString
        {
            get
            {
                if (priority == -1)
                    return IntelligentString.Empty;

                return (priority + 1).ToString();
            }
        }

        /// <summary>
        /// Gets or sets the path to the root folder
        /// </summary>
        public IntelligentString Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        /// <summary>
        /// Gets or sets the tags assigned to the root folder
        /// </summary>
        public ObservableCollection<IntelligentString> Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                OnPropertyChanged("Tags");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RootFolder class
        /// </summary>
        public RootFolder()
            :base()
        {
            MediaItemType = MediaItemTypeEnum.NotSet;
            Priority = -1;
            Path = IntelligentString.Empty;
            Tags = new ObservableCollection<IntelligentString>();
        }

        /// <summary>
        /// Initialises a new instance of the RootFolder class
        /// </summary>
        /// <param name="mediaItemType">Type of media item the root folder is associated with</param>
        public RootFolder(MediaItemTypeEnum mediaItemType)
            : this()
        {
            MediaItemType = mediaItemType;
        }

        /// <summary>
        /// Initialises a new instance of the RootFolder class
        /// </summary>
        /// <param name="mediaItemType">Type of media item the root folder is associated with</param>
        /// <param name="priority">Priority of the root folder</param>
        public RootFolder(MediaItemTypeEnum mediaItemType, Int16 priority)
            : this(mediaItemType)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    RootFolder clone = Data.RootFolder.GetRootFolderByPriority(conn, mediaItemType, priority);

                    MediaItemType = clone.MediaItemType;
                    Priority = clone.Priority;
                    Path = clone.Path;
                    Tags = clone.Tags;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Updates the tags in the root folder
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="clone">This root folder as it is currently saved in the database</param>
        private void UpdateTags(SqlConnection conn, RootFolder clone)
        {
            List<IntelligentString> expected = new List<IntelligentString>(Tags);
            List<IntelligentString> toDelete = new List<IntelligentString>(clone.Tags);
            List<IntelligentString> toAdd = new List<IntelligentString>();

            while (expected.Count > 0)
            {
                if (toDelete.Any(p => p.ToLower() == expected[0].ToLower()))
                    toDelete.Remove(toDelete.Single(p => p.ToLower() == expected[0].ToLower()));
                else
                    toAdd.Add(expected[0]);

                expected.Remove(expected[0]);
            }

            foreach (IntelligentString tag in toDelete)
                Data.RootFolder.DeleteRootFolderTag(conn, MediaItemType, Priority, tag.Value);

            foreach (IntelligentString tag in toAdd)
                Data.RootFolder.AddRootFolderTag(conn, MediaItemType, Priority, tag.Value);
        }

        /// <summary>
        /// Removes any empty folders from the root folder
        /// </summary>
        /// <param name="filenamesToIgnore">Collection of file names that root folders ignore when cleaning up</param>
        /// <param name="directoryNamesToIgnore">Collection of directory names that root folders ignore when cleaning up</param>
        /// <returns>Collection of filenames found in the root folder that do not belong to the media library</returns>
        public String[] CleanUp(String[] filenamesToIgnore, String[] directoryNamesToIgnore)
        {
            List<String> filenames = new List<String>();
            CleanUp(Path.Value, filenames, filenamesToIgnore, directoryNamesToIgnore, true);

            return filenames.ToArray();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Determines whether a root folder already exists with specified type and path
        /// </summary>
        /// <param name="mediaItemType">Type associated with the desired root folders</param>
        /// <param name="path">Path to the desired root folder</param>
        /// <returns>True if a root folder already exists with specified type and path, false if not</returns>
        public static Boolean RootFolderPathExists(MediaItemTypeEnum mediaItemType, IntelligentString path)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return Data.RootFolder.RootFolderPathExists(conn, mediaItemType, path.Value);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Creates a new RootFolder object using the data in a data row
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="row">DataRow containing the data for the root folder</param>
        /// <returns>RootFolder object with properties loaded from the database</returns>
        internal static RootFolder FromDataRow(SqlConnection conn, sp.DataRow row)
        {
            RootFolder rootFolder = new RootFolder();
            rootFolder.MediaItemType = (MediaItemTypeEnum)Convert.ToInt16(row["MediaItemType"]);
            rootFolder.Priority = (Int16)row["Priority"];
            rootFolder.Path = (String)row["Path"];
            rootFolder.Tags = new ObservableCollection<IntelligentString>(Data.RootFolder.GetRootFolderTagsByPriority(conn, rootFolder.MediaItemType, rootFolder.priority));
            rootFolder.IsInDatabase = true;

            return rootFolder;
        }

        /// <summary>
        /// Gets all root folders in the system that are associated with the specified type
        /// </summary>
        /// <param name="mediaItemType">Type associated with the desired root folders</param>
        /// <returns>All root folders in the system that are associated with the specified type</returns>
        public static RootFolder[] GetRootFoldersByType(MediaItemTypeEnum mediaItemType)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return Data.RootFolder.GetRootFoldersByType(conn, mediaItemType);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Traverses a directory structure and cleans up the directories
        /// </summary>
        /// <param name="directoryPath">Path to the directory to clean up</param>
        /// <param name="filenames">List of filenames found in the directory that do not belong to the media library</param>
        /// <param name="filenamesToIgnore">Collection of file names that root folders ignore when cleaning up</param>
        /// <param name="directoryNamesToIgnore">Collection of directory names that root folders ignore when cleaning up</param>
        /// <param name="directoryPathIsRoot">Value determining whether or not the path the directoryPath parameter points to is a root folder</param>
        public static void CleanUp(String directoryPath, List<String> filenames, String[] filenamesToIgnore, String[] directoryNamesToIgnore, Boolean directoryPathIsRoot)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            Boolean containsNonHiddenFile = false;

            CleanUp(di, filenames, filenamesToIgnore, directoryNamesToIgnore, true, ref containsNonHiddenFile);

            di = null;
            GC.Collect();
        }

        /// <summary>
        /// Traverses a directory structure and cleans up the directories
        /// </summary>
        /// <param name="di">Directory to clean up</param>
        /// <param name="filenames">List of filenames found in the directory that do not belong to the media library</param>
        /// <param name="filenamesToIgnore">Collection of file names that root folders ignore when cleaning up</param>
        /// <param name="directoryNamesToIgnore">Collection of directory names that root folders ignore when cleaning up</param>
        /// <param name="diIsRoot">Value determining whether or not the di parameter is a root folder</param>
        private static void CleanUp(DirectoryInfo di, List<String> filenames, String[] filenamesToIgnore, String[] directoryNamesToIgnore, Boolean diIsRoot, ref Boolean containsNonHiddenItem)
        {
            if (di.Exists)
            {
                try
                {
                    Boolean containsDirectory = false;

                    DirectoryInfo[] directories = di.GetDirectories();
                    directories = directories.Where(p => !directoryNamesToIgnore.Any(f => f.ToLower() == p.Name.ToLower())).ToArray();

                    foreach (DirectoryInfo child in directories)
                    {
                        containsDirectory = true;
                        Boolean childContainsNonHiddenItem = false;

                        CleanUp(child, filenames, filenamesToIgnore, directoryNamesToIgnore, false, ref childContainsNonHiddenItem);

                        if (Directory.Exists(child.FullName))
                        {
                            try
                            {
                                if (childContainsNonHiddenItem)
                                {
                                    containsNonHiddenItem = true;
                                    child.Attributes = FileAttributes.Normal;
                                }
                                else
                                    child.Attributes = FileAttributes.Hidden;
                            }
                            catch (ArgumentException)
                            {
                                //couldn't set attributes, do nothing
                            }
                        }
                    }

                    FileInfo[] files = di.GetFiles();
                    files = files.Where(p => !filenamesToIgnore.Any(f => f.ToLower() == p.Name.ToLower())).ToArray();

                    if (files.Length == 0)
                    {
                        if (!diIsRoot)
                            if (!containsDirectory)
                                di.Delete(true);
                    }
                    else
                    {
                        foreach (FileInfo fi in files)
                        {
                            Boolean isFileHidden = ((fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);

                            if (!isFileHidden)
                                containsNonHiddenItem = true;

                            if (!MediaItem.MediaItemPartLocationExists(fi.FullName))
                                filenames.Add(fi.FullName);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    //do nothing
                }
                catch (IOException)
                {
                    //do nothing
                }
            }
        }

        /// <summary>
        /// Switches paths of two root folders
        /// </summary>
        /// <param name="rootFolder1">Root folder being switched</param>
        /// <param name="rootFolder2">Root folder being switched</param>
        public static void SwitchRootFolderPaths(RootFolder rootFolder1, RootFolder rootFolder2)
        {
            //get the path of the root folder moving up
            IntelligentString path = rootFolder1.Path;

            //at this point we cannot simply set the path of rootFolder2 to the path of rootFolder1 and save because
            //there would be a duplicate paths in the database. Therefore we need to set the path of rootFolder1 to
            //something unique first. our unique path will be the original path of rootFolder2 with a number on the end
            IntelligentString tempPath;
            int tempPtr = 0;

            do
            {
                tempPath = rootFolder2.Path + tempPtr.ToString();
                tempPtr++;
            }
            while (RootFolder.RootFolderPathExists(rootFolder2.MediaItemType, tempPath));

            rootFolder1.Path = tempPath;
            rootFolder1.Save();

            //now we can set the path of rootFolder2 to the original path of rootFolder1 and save
            rootFolder2.Path = path;
            rootFolder2.Save();

            //we can now remove the number from the end of rootFolder1.Path which will give us the original
            //path or rootFolder2. our root folders will then have had their paths switched
            rootFolder1.Path = rootFolder1.Path.Substring(0, rootFolder1.Path.Length - tempPtr.ToString().Length);
            rootFolder1.Save();
        }

        #endregion

        #region SqlDbObject Members

        protected override bool ValidateProperties(DbConnection conn, out Exception exception)
        {
            if (IntelligentString.IsNullOrEmpty(Path))
            {
                exception = new PropertyNotSetException(GetType(), "Path");
                return false;
            }

            Boolean checkPathExists = true;
            if (IsInDatabase)
            {
                RootFolder clone = new RootFolder(MediaItemType, Priority);

                if (clone.Path == Path)
                    checkPathExists = false;
            }

            if (checkPathExists)
            {
                if (RootFolderPathExists(MediaItemType, Path))
                {
                    exception = new DuplicatePropertyValueException(GetType(), "Path", Path);
                    return false;
                }
            }

            exception = null;
            return true;
        }

        protected override void AddToSqlDatabase(SqlConnection conn)
        {
            Priority = Data.RootFolder.AddRootFolder(conn, this);
        }

        protected override void UpdateInSqlDatabase(SqlConnection conn)
        {
            Data.RootFolder.UpdateRootFolder(conn, this);

            RootFolder clone = new RootFolder(MediaItemType, Priority);
            UpdateTags(conn, clone);
        }

        protected override void DeleteFromSqlDatabase(SqlConnection conn)
        {
            Data.RootFolder.DeleteRootFolder(conn, MediaItemType, Priority);
        }

        protected override void ResetProperties()
        {
            Priority = -1;
        }

        public override sp.ParameterCollection GetParametersForStoredProcedure(Boolean includeId)
        {
            sp.ParameterCollection parameters = new sp.ParameterCollection();

            if (includeId)
                parameters.AddWithValue("Priority", DbType.Int16, Priority);

            parameters.AddWithValue("MediaItemType", DbType.Int16, Convert.ToInt16(MediaItemType));
            parameters.AddWithValue("Path", DbType.String, Path.Value);

            return parameters;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            RootFolder rootFolder = obj as RootFolder;

            return Priority.CompareTo(rootFolder.Priority);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return GeneralMethods.Clone<RootFolder>(this);
        }

        #endregion
    }
}

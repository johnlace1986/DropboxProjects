using System;
using System.Linq;
using MediaPlayer.Library.Business;
using Utilities.Business;
using System.IO;

namespace MediaPlayer.Business
{
    public class NonLibraryFile
    {
        #region Fields

        /// <summary>
        /// Gets or sets the root folder the file was found in
        /// </summary>
        private readonly RootFolder rootFolder;

        /// <summary>
        /// Gets or sets the sub folder(s) within the root folder the file was found in
        /// </summary>
        private readonly IntelligentString subFolderPath;

        /// <summary>
        /// Gets or sets the name of the file that was found in the root folder that doesn't belong to the media library
        /// </summary>
        private readonly IntelligentString name;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root folder the file was found in
        /// </summary>
        public RootFolder RootFolder
        {
            get { return rootFolder; }
        }

        /// <summary>
        /// Gets the sub folder(s) within the root folder the file was found in
        /// </summary>
        public IntelligentString SubFolderPath
        {
            get { return subFolderPath; }
        }

        /// <summary>
        /// Gets the name of the file that was found in the root folder that doesn't belong to the media library
        /// </summary>
        public IntelligentString Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the full name of the file that was found in the root folder that doesn't belong to the media library
        /// </summary>
        public IntelligentString FullName
        {
            get
            {     
                //get the path to the root folder
                IntelligentString rootFolderPath = AppendTrailingSlash(RootFolder.Path);

                //get the sub folder within the root folder of the file
                IntelligentString subFolder = SubFolderPath;

                if (!IntelligentString.IsNullOrEmpty(subFolder))
                    subFolder = AppendTrailingSlash(subFolder);

                //get path to the file
                return rootFolderPath + subFolder + Name;
            }
        }

        /// <summary>
        /// Gets the name of the folder the root folder the file was found in
        /// </summary>
        public IntelligentString SubFolderName
        {
            get
            {
                FileInfo fi = new FileInfo(FullName.Value);
                IntelligentString subFolderName = fi.Directory.Name;

                fi = null;
                GC.Collect();

                return subFolderName;
            }
        }

        /// <summary>
        /// Gets a value determining whether or not the file is directly in the root folder
        /// </summary>
        public Boolean IsInRootFolder
        {
            get
            {
                IntelligentString rootPath = AppendTrailingSlash(RootFolder.Path) + Name;

                return rootPath == FullName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the NonLibraryFile class
        /// </summary>
        /// <param name="rootFolder">Root folder the file was found in</param>
        /// <param name="subFolder">Sub folder within the root folder the file was found in</param>
        /// <param name="name">Name of the file that was found in the root folder that doesn't belong to the media library</param>
        public NonLibraryFile(RootFolder rootFolder, IntelligentString subFolder, IntelligentString name)
        {
            this.rootFolder = rootFolder;
            this.subFolderPath = subFolder;
            this.name = name;
        }

        #endregion

        #region Static Methods

        private static IntelligentString AppendTrailingSlash(IntelligentString value)
        {
            if (!value.EndsWith("\\"))
                value += "\\";

            return value;
        }

        /// <summary>
        /// Converts a filename to a NonLibraryFile object
        /// </summary>
        /// <param name="rootFolder">Root folder the non-library file was found in</param>
        /// <param name="filename">Full filename of the non-library file</param>
        /// <returns>NonLibraryFile object derived from the specified file name</returns>
        public static NonLibraryFile FromFilename(RootFolder rootFolder, IntelligentString filename)
        {
            IntelligentString rootFolderPath = AppendTrailingSlash(rootFolder.Path);

            filename = filename.Substring(rootFolderPath.Length);

            IntelligentString[] subFolders = filename.Split("\\");
            IntelligentString subFolder = IntelligentString.Empty;

            for (int i = 0; i < subFolders.Length; i++)
            {
                if (i == (subFolders.Length - 1))
                    filename = subFolders[i];
                else
                    subFolder += subFolders[i] + "\\";
            }

            return new NonLibraryFile(rootFolder, subFolder, filename);
        }

        #endregion
    }
}

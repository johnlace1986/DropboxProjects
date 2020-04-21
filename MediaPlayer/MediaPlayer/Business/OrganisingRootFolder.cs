using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Business;
using MediaPlayer.Library.Business;

namespace MediaPlayer.Business
{
    public class OrganisingRootFolder
    {
        #region Fields

        /// <summary>
        /// Root folder this object is wrapping around
        /// </summary>
        private readonly RootFolder rootFolder;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of media item the root folder is associated with
        /// </summary>
        public MediaItemTypeEnum MediaItemType
        {
            get
            {
                if (rootFolder == null)
                    return MediaItemTypeEnum.NotSet;

                return rootFolder.MediaItemType;
            }
        }

        /// <summary>
        /// Gets the path to the physical root folder
        /// </summary>
        public IntelligentString Path
        {
            get
            {
                if (rootFolder == null)
                    return IntelligentString.Empty;

                return rootFolder.Path;
            }
        }

        /// <summary>
        /// Gets the tags assigned to the root folder
        /// </summary>
        public IntelligentString[] Tags
        {
            get
            {
                List<IntelligentString> tags = new List<IntelligentString>();

                if (rootFolder != null)
                    tags = new List<IntelligentString>(rootFolder.Tags);

                return tags.ToArray();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OrganisingRootFolder class
        /// </summary>
        /// <param name="rootFolder">Root folder this object is wrapping around</param>
        public OrganisingRootFolder(RootFolder rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Sorts the specified root folders based on the media item being oragnised into them
        /// </summary>
        /// <param name="mediaItem">Media item being oragnised into the specified root folders</param>
        /// <param name="rootFolders">Root folders the media item is being organised into</param>
        /// <returns>Specified root folders sorted based on the media item being oragnised into them</returns>
        public static OrganisingRootFolder[] SortForMediaItem(MediaItem mediaItem, OrganisingRootFolder[] rootFolders)
        {
            List<MatchedOrganisingRootFolder> sorted = new List<MatchedOrganisingRootFolder>();

            foreach (OrganisingRootFolder rootFolder in rootFolders)
            {
                if (rootFolder.MediaItemType == mediaItem.Type)
                {
                    Int32 matches = 0;

                    foreach (IntelligentString tag in rootFolder.Tags)
                        if (mediaItem.ContainsSearchText(tag.Value))
                            matches++;

                    sorted.Add(new MatchedOrganisingRootFolder(rootFolder, matches));
                }
            }

            sorted.Sort();

            return (from sortedRootFolder in sorted
                    select sortedRootFolder.RootFolder).ToArray();
        }

        #endregion
    }
}

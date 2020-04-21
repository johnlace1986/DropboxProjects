using System;
using System.Linq;

namespace MediaPlayer.Business
{
    public class MatchedOrganisingRootFolder : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the root folder that was matched to a media item
        /// </summary>
        public OrganisingRootFolder RootFolder { get; private set; }

        /// <summary>
        /// Gets or sets the number times the root folder could be matched to a media item
        /// </summary>
        public Int32 Matches { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MatchedOrganisingRootFolder class
        /// </summary>
        /// <param name="rootFolder">Root folder that was matched to a media item</param>
        /// <param name="matches">Number times the root folder could be matched to a media item</param>
        public MatchedOrganisingRootFolder(OrganisingRootFolder rootFolder, Int32 matches)
        {
            RootFolder = rootFolder;
            Matches = matches;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            MatchedOrganisingRootFolder rootFolder = obj as MatchedOrganisingRootFolder;

            return rootFolder.Matches.CompareTo(Matches);
        }

        #endregion
    }
}

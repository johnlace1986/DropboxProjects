using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Business;
using MediaPlayer.Library.Business;
using System.IO;

namespace MediaPlayer.Business
{
    public class MediaItemExists : NotifyPropertyChangedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the media item the part that doesn't exist belongs to
        /// </summary>
        private readonly MediaItem mediaItem;

        /// <summary>
        /// Gets or sets the part that doesn't exist
        /// </summary>
        private readonly MediaItemPart part;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the media item the part that doesn't exist belongs to
        /// </summary>
        public MediaItem MediaItem
        {
            get { return mediaItem; }
        }

        /// <summary>
        /// Gets the part that doesn't exist
        /// </summary>
        public MediaItemPart Part
        {
            get { return part; }
        }

        /// <summary>
        /// Gets the index of the part that doesn't exist, expressed as a string
        /// </summary>
        public IntelligentString IndexString
        {
            get
            {
                return ((Part.Index + 1).ToString() + " of " + MediaItem.Parts.Count.ToString());
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemExists class
        /// </summary>
        /// <param name="mediaItem">Media item the part that doesn't exist belongs to</param>
        /// <param name="partIndex">Index in the media item's collection of the part that doesn't exist</param>
        public MediaItemExists(MediaItem mediaItem, Int32 partIndex)
        {
            this.mediaItem = mediaItem;
            this.part = mediaItem.Parts[partIndex];
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the parts of the media items in the specified collection that do not exist
        /// </summary>
        /// <param name="mediaItems">Media items to check</param>
        /// <param name="showHidden">Value determining whether or not hidden media items should be displayed</param>
        /// <returns>Parts of the media items in the specified collection that do not exist</returns>
        public static MediaItemExists[] GetPartsThatDoNotExist(IEnumerable<MediaItem> mediaItems, Boolean? showHidden)
        {
            List<MediaItemExists> partsThatDoNotExist = new List<MediaItemExists>();

            foreach (MediaItem mediaItem in mediaItems.Where(p => showHidden == null || p.IsHidden == showHidden))
                foreach (MediaItemPart part in mediaItem.Parts)
                    if (!File.Exists(part.Location.Value))
                        partsThatDoNotExist.Add(new MediaItemExists(mediaItem, part.Index));

            return partsThatDoNotExist.ToArray();
        }

        #endregion
    }
}

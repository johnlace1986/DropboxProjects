using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Utilities.Business;
using System.Collections.Specialized;
using System.IO;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public class MediaItemPartCollection : NotifyCollectionChangedObject<MediaItemPart>, IEnumerable<MediaItemPart>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the media item parts in the collection
        /// </summary>
        private readonly List<MediaItemPart> parts = new List<MediaItemPart>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the part in the collection at the specified index
        /// </summary>
        /// <param name="index">Index of the desired part</param>
        /// <returns>Part in the collection at the specified index</returns>
        public MediaItemPart this[int index]
        {
            get { return parts[index]; }
            set
            {
                MediaItemPart oldItem = parts[index];
                parts[index] = value;

                OnItemReplaced(value, oldItem, index);
            }
        }

        /// <summary>
        /// Gets the number of parts in the collection
        /// </summary>
        public int Count
        {
            get { return parts.Count; }
        }

        /// <summary>
        /// Gets the size of all parts in the collection
        /// </summary>
        public Int64 Size
        {
            get
            {
                Int64 size = 0;

                foreach (MediaItemPart part in parts)
                    size += part.Size;

                return size;
            }
        }

        /// <summary>
        /// Gets the size of all parts in the collection expressed as a string
        /// </summary>
        public IntelligentString SizeString
        {
            get { return IntelligentString.FormatSize(Size); }
        }

        /// <summary>
        /// Gets the duration of all parts in the collection
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                TimeSpan duration = new TimeSpan();

                foreach (MediaItemPart part in parts)
                    duration += part.Duration;

                return duration;
            }
        }

        /// <summary>
        /// Gets the duration of all parts in the collection expressed as a string
        /// </summary>
        public IntelligentString DurationString
        {
            get { return IntelligentString.FormatDuration(Duration, false); }
        }

        /// <summary>
        /// Gets or sets the location of the first part in the collection
        /// </summary>
        public IntelligentString FirstLocation
        {
            get
            {
                if (parts.Count == 1)
                    return parts[0].Location;

                return IntelligentString.Empty;
            }
        }

        /// <summary>
        /// Gets a value determining whether all parts in the collection exist in the file system
        /// </summary>
        public Boolean PartsExist
        {
            get
            {
                foreach (MediaItemPart part in parts)
                    if (!File.Exists(part.Location.Value))
                        return false;

                return true;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemPartCollection class
        /// </summary>
        public MediaItemPartCollection()
        {
        }

        /// <summary>
        /// Initialises a new instance of the MediaItemPartCollection class
        /// </summary>
        /// <param name="parts">Collection of parts to add to this collection</param>
        public MediaItemPartCollection(IEnumerable<MediaItemPart> parts)
        {
            this.parts = new List<MediaItemPart>(parts);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a new part to the end of the collection
        /// </summary>
        /// <param name="location">Location of the physical file of the media item part</param>
        /// <param name="size">Size of the media item part in bytes</param>
        /// <param name="milliseconds">Duration of the part in milliseconds</param>
        /// <returns>New part added to the end of the collection</returns>
        public MediaItemPart Add(IntelligentString location, Int64 size, Int32 milliseconds)
        {
            return Add(location, size, TimeSpan.FromMilliseconds(milliseconds), Convert.ToInt16(parts.Count));
        }

        /// <summary>
        /// Adds a new part to the end of the collection
        /// </summary>
        /// <param name="location">Location of the physical file of the media item part</param>
        /// <param name="size">Size of the media item part in bytes</param>
        /// <param name="milliseconds">Duration of the part in milliseconds</param>
        /// <param name="index">Index in the collection of the new part</param>
        /// <returns>New part added to the end of the collection</returns>
        internal MediaItemPart Add(IntelligentString location, Int64 size, Int32 milliseconds, Int16 index)
        {
            return Add(location, size, TimeSpan.FromMilliseconds(milliseconds), index);
        }

        /// <summary>
        /// Adds a new part to the end of the collection
        /// </summary>
        /// <param name="location">Location of the physical file of the media item part</param>
        /// <param name="size">Size of the media item part in bytes</param>
        /// <param name="duration">Duration of the part</param>
        /// <returns>New part added to the end of the collection</returns>
        public MediaItemPart Add(IntelligentString location, Int64 size, TimeSpan duration)
        {
            return Add(location, size, duration, Convert.ToInt16(parts.Count));
        }

        /// <summary>
        /// Adds a new part to the end of the collection
        /// </summary>
        /// <param name="location">Location of the physical file of the media item part</param>
        /// <param name="size">Size of the media item part in bytes</param>
        /// <param name="duration">Duration of the part</param>
        /// <param name="index">Index in the collection of the new part</param>
        /// <returns>New part added to the end of the collection</returns>
        internal MediaItemPart Add(IntelligentString location, Int64 size, TimeSpan duration, Int16 index)
        {
            MediaItemPart part = new MediaItemPart();
            part.location = location;
            part.size = size;
            part.duration = duration;
            part.Index = index;

            parts.Add(part);

            OnCollectionChanged(NotifyCollectionChangedAction.Add, part);

            return part;
        }

        /// <summary>
        /// Removes the part from the collection
        /// </summary>
        /// <param name="location">Location of the part being removed</param>
        public void Remove(IntelligentString location)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].Location == location)
                {
                    RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes the part from the collection at the specified index
        /// </summary>
        /// <param name="index">Index in the collection of the part being removed</param>
        public void RemoveAt(int index)
        {
            MediaItemPart toRemove = parts[index];
            parts.RemoveAt(index);

            foreach (MediaItemPart part in parts.Where(p => p.Index > index))
                part.Index--;

            OnRemovedFromCollection(toRemove, index);
        }

        /// <summary>
        /// Removes all parts from the collection
        /// </summary>
        public void Clear()
        {
            while (parts.Count != 0)
                RemoveAt(0);
        }
        
        /// <summary>
        /// Sorts the parts in the collection by Index
        /// </summary>
        public void Sort()
        {
            parts.Sort();
            OnCollectionSorted();
        }

        /// <summary>
        /// Gets the organised path of a media item part
        /// </summary>
        /// <param name="mediaItemOrganisedPath">Organised path of the media item the part belongs to</param>
        /// <param name="index">Index in the collection of the part being organised</param>
        /// <returns>Organised path of a media item part</returns>
        public IntelligentString GetPartOrganisedPath(IntelligentString mediaItemOrganisedPath, int index)
        {
            MediaItemPart part = parts[index];

            if (!IntelligentString.IsNullOrEmpty(part.Location))
            {
                if (parts.Count > 1)
                {
                    String format = "0";

                    for (int i = 1; i < parts.Count.ToString().Length; i++)
                        format += "0";

                    mediaItemOrganisedPath += " (Part " + (part.Index + 1).ToString(format) + ")";
                }

                FileInfo fi = new FileInfo(part.Location.Value);
                mediaItemOrganisedPath += fi.Extension;
            }

            return mediaItemOrganisedPath;
        }

        /// <summary>
        /// Gets the index of the part that is found at the specified duration
        /// </summary>
        /// <param name="duration">Overall duration of the media item</param>
        /// <param name="index">Index of the part that is found at the specified duration</param>
        /// <param name="position">Position within the part</param>
        public void GetPartFromDuration(TimeSpan duration, out Int32 index, out TimeSpan position)
        {
            index = 0;
            position = duration;

            while (parts[index].Duration < position)
            {
                position = position - parts[index].Duration;
                index++;
            }
        }

        #endregion

        #region IEnumerable<MediaItemPart> Members

        public IEnumerator<MediaItemPart> GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        #endregion
    }
}

using System;
using System.Linq;
using Utilities.Business;
using System.Data.SqlClient;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public class MediaItemPart : NotifyPropertyChangedObject, IComparable
    {
        #region Fields

        /// <summary>
        /// Gets or sets the location of the physical file of the media item part
        /// </summary>
        internal IntelligentString location;

        /// <summary>
        /// Gets or sets the size of the media item part in bytes
        /// </summary>
        internal Int64 size;

        /// <summary>
        /// Gets or sets the duration of the media item part
        /// </summary>
        internal TimeSpan duration;

        /// <summary>
        /// Gets or sets the index in the media item of the part
        /// </summary>
        private Int16 index;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the location of the physical file of the media item part
        /// </summary>
        public IntelligentString Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
                OnPropertyChanged("Uri");
            }
        }

        /// <summary>
        /// Gets the location as a uri
        /// </summary>
        public Uri Uri
        {
            get { return new Uri(Location.Value); }
        }
        
        /// <summary>
        /// Gets or sets the size of the media item part in bytes
        /// </summary>
        public Int64 Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged("Size");
                OnPropertyChanged("SizeString");
            }
        }

        /// <summary>
        /// Gets the size of the media item part in bytes expressed as a string
        /// </summary>
        public IntelligentString SizeString
        {
            get
            {
                if (Size == 0)
                    return IntelligentString.Empty;

                return IntelligentString.FormatSize(Size);
            }
        }
        
        /// <summary>
        /// Gets or sets the duration of the media item part
        /// </summary>
        public TimeSpan Duration
        {
            get { return duration; }
            internal set
            {
                duration = value;
                OnPropertyChanged("Duration");
                OnPropertyChanged("DurationString");
            }
        }

        /// <summary>
        /// Gets the duration of the media item part expressed as a string
        /// </summary>
        public IntelligentString DurationString
        {
            get
            {
                if (Duration.TotalMilliseconds == 0)
                    return IntelligentString.Empty;

                return IntelligentString.FormatDuration(Duration, false);
            }
        }

        /// <summary>
        /// Gets or sets the index in the media item of the part
        /// </summary>
        public Int16 Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged("Index");
            }
        }

        /// <summary>
        /// Gets the index in the media item of the part expressed as a string
        /// </summary>
        public IntelligentString IndexString
        {
            get { return (index + 1).ToString(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItemPart class
        /// </summary>
        internal MediaItemPart()
        {
            this.location = IntelligentString.Empty;
            this.size = 0;
            this.duration = new TimeSpan(0);
            this.index = -1;
        }

        /// <summary>
        /// Initialises a new instance of the MediaItemPart class
        /// </summary>
        /// <param name="location">Location of the media item part</param>
        public MediaItemPart(IntelligentString location)
            : this()
        {
            this.location = location;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Loads the parts for the specfied media item
        /// </summary>MediaItemPartLocationExists
        /// <param name="mediaItemId">Unique identifier of the media item</param>
        /// <param name="mediaItemType">Type of the media item</param>
        /// <returns>Collection of parts belonging to the specified media item, sorted ascendingly</returns>
        internal static Business.MediaItemPartCollection GetMediaItemPartsById(SqlConnection conn, Int64 mediaItemId, Business.MediaItemTypeEnum mediaItemType)
        {
            return Data.MediaItemPart.GetMediaItemPartsById(conn, mediaItemId, mediaItemType);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (!(obj is MediaItemPart))
                throw new ArgumentException("Both items being compared must be MediaItemPart objects");

            MediaItemPart part = obj as MediaItemPart;
            return Index.CompareTo(part.Index);
        }

        #endregion
    }
}

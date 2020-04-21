using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using sp = Utilities.Data.StoredProcedure;
using Utilities.Exception;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Collections.ObjectModel;
using Utilities.Data;
using Utilities.Business;
using System.Data.SqlClient;
using Utilities.Data.SQL;
using System.Data.Common;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public abstract class MediaItem : SqlDbObject, IComparable, ICloneable
    {
        #region Fields

        /// <summary>
        /// Gets or sets the unique identifier of the media item
        /// </summary>
        private Int64 id;

        /// <summary>
        /// Gets or sets the name of the media item
        /// </summary>
        private IntelligentString name;

        /// <summary>
        /// Gets or sets the genre of the media item
        /// </summary>
        private IntelligentString genre;

        /// <summary>
        /// Gets or sets a value determining whether or not the media item is hidden
        /// </summary>
        private Boolean isHidden;

        /// <summary>
        /// Gets or sets the date and time the media item was created
        /// </summary>
        private DateTime dateCreated;

        /// <summary>
        /// Gets or sets the date and time the media item was last modified
        /// </summary>
        private DateTime dateModified;

        /// <summary>
        /// Gets or sets the play history for the media item
        /// </summary>
        private DateTime[] playHistory;

        /// <summary>
        /// Gets or sets the parts of the media item
        /// </summary>
        private MediaItemPartCollection parts;

        /// <summary>
        /// Gets or sets the tags assigned to the media item
        /// </summary>
        private ObservableCollection<IntelligentString> tags;

        /// <summary>
        /// Gets or sets the unique identifier of the user who created the media item
        /// </summary>
        private IntelligentString userName;
                
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the media item
        /// </summary>
        public Int64 Id
        {
            get { return id; }
            protected set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Gets or sets the name of the media item
        /// </summary>
        public IntelligentString Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets the genre of the video
        /// </summary>
        public IntelligentString Genre
        {
            get { return genre; }
            set
            {
                genre = value;
                OnPropertyChanged("Genre");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the media item is hidden
        /// </summary>
        public Boolean IsHidden
        {
            get { return isHidden; }
            set
            {
                isHidden = value;
                OnPropertyChanged("IsHidden");
            }
        }

        /// <summary>
        /// Gets or sets the date and time the media item was created
        /// </summary>
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set
            {
                dateCreated = value;
                OnPropertyChanged("DateCreated");
                OnPropertyChanged("DateCreatedString");
            }
        }

        /// <summary>
        /// Gets the date and time the media item was created expressed as a string
        /// </summary>
        public String DateCreatedString
        {
            get
            {
                if (DateCreated == DefaultDate)
                    return "";

                return DateCreated.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        /// <summary>
        /// Gets or sets the date and time the media item was last modified
        /// </summary>
        public DateTime DateModified
        {
            get { return dateModified; }
            protected set
            {
                dateModified = value;
                OnPropertyChanged("DateModified");
                OnPropertyChanged("DateModifiedString");
            }
        }

        /// <summary>
        /// Gets the date and time the media item was last modified expressed as a string
        /// </summary>
        public String DateModifiedString
        {
            get
            {
                if (DateModified == DefaultDate)
                    return "";

                return DateModified.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        /// <summary>
        /// Gets or sets the play history for the media item
        /// </summary>
        public DateTime[] PlayHistory
        {
            get { return playHistory; }
            protected set
            {
                playHistory = value;
                OnPropertyChanged("PlayHistory");
                OnPropertyChanged("PlayCount");
                OnPropertyChanged("PlayCountString");
                OnPropertyChanged("DateLastPlayed");
                OnPropertyChanged("DateLastPlayedString");
            }
        }

        /// <summary>
        /// Gets the number of times the media item has been played to the end
        /// </summary>
        public int PlayCount
        {
            get { return PlayHistory.Length; }
        }

        /// <summary>
        /// Gets the number of times the media item has been played to the end expressed as a string
        /// </summary>
        public IntelligentString PlayCountString
        {
            get
            {
                if (PlayCount == 0)
                    return IntelligentString.Empty;

                return PlayCount.ToString();
            }
        }
        
        /// <summary>
        /// Gets the last date and time the media item was played to the end
        /// </summary>
        public DateTime DateLastPlayed
        {
            get
            {
                if (PlayHistory.Length == 0)
                    return DefaultDate;

                return PlayHistory[PlayHistory.Length - 1];
            }
        }

        /// <summary>
        /// Gets the last date and time the media item was played to the end expressed as a string
        /// </summary>
        public String DateLastPlayedString
        {
            get
            {
                if (DateLastPlayed == DefaultDate)
                    return "";

                return DateLastPlayed.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        /// <summary>
        /// Gets or sets the parts of the media item
        /// </summary>
        public MediaItemPartCollection Parts
        {
            get { return parts; }
            set
            {
                parts = value;
                UpdatedParts();
            }
        }

        /// <summary>
        /// Gets or sets the tags assigned to the media item
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

        /// <summary>
        /// Gets the description of the media item
        /// </summary>
        public IntelligentString Description
        {
            get { return GetDescription(" - "); }
        }

        /// <summary>
        /// Gets or sets the tool tip for the media item
        /// </summary>
        public IntelligentString ToolTip
        {
            get { return GetDescription(Environment.NewLine); }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the user who created the media item
        /// </summary>
        public IntelligentString UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
               
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MediaItem class
        /// </summary>
        protected MediaItem()
            : base()
        {
            Id = -1;
            Name = IntelligentString.Empty;
            Genre = IntelligentString.Empty;
            IsHidden = false;
            DateCreated = DateTime.Now;
            DateModified = DefaultDate;
            PlayHistory = new DateTime[0];
            Parts = new MediaItemPartCollection();
            Tags = new ObservableCollection<IntelligentString>();
            UserName = Environment.UserDomainName + "\\" + Environment.UserName;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Copies the data from the clone to this media item
        /// </summary>
        /// <param name="clone">Media item being cloned</param>
        public virtual void CopyFromClone(MediaItem clone)
        {
            Id = clone.Id;
            Name = clone.Name;
            IsHidden = clone.IsHidden;
            DateCreated = clone.DateCreated;
            DateModified = clone.DateModified;
            PlayHistory = clone.PlayHistory;
            Genre = clone.Genre;
            Tags = clone.Tags;
            UserName = clone.UserName;
            IsInDatabase = clone.IsInDatabase;
        }

        /// <summary>
        /// Updates the parts in the media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="clone">This media item as it is currently saved in the database</param>
        private void UpdateParts(SqlConnection conn, MediaItem clone)
        {
            List<MediaItemPart> expected = new List<MediaItemPart>(Parts);
            List<MediaItemPart> toDelete = new List<MediaItemPart>(clone.Parts);
            List<MediaItemPart> toAdd = new List<MediaItemPart>();

            while (expected.Count > 0)
            {
                if (toDelete.Any(p =>
                    (p.Location == expected[0].Location) &&
                    (p.Duration == expected[0].Duration) &&
                    (p.Index == expected[0].Index) &&
                    (p.Size == expected[0].Size)))
                    toDelete.Remove(toDelete.Single(p =>
                        (p.Location == expected[0].Location) &&
                        (p.Duration == expected[0].Duration) &&
                        (p.Index == expected[0].Index) &&
                        (p.Size == expected[0].Size)));
                else
                    toAdd.Add(expected[0]);

                expected.Remove(expected[0]);
            }

            foreach (MediaItemPart part in toDelete)
                Data.MediaItemPart.DeleteMediaItemPart(conn, Id, Type, part.Location.Value);

            foreach (MediaItemPart part in toAdd)
                Data.MediaItemPart.AddMediaItemPart(conn, Id, Type, part);
        }

        /// <summary>
        /// Updates the tags in the media item
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="clone">This media item as it is currently saved in the database</param>
        private void UpdateTags(SqlConnection conn, MediaItem clone)
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
                Data.MediaItem.DeleteMediaItemTag(conn, Id, Type, tag.Value);

            foreach (IntelligentString tag in toAdd)
                Data.MediaItem.AddMediaItemTag(conn, Id, Type, tag.Value);
        }

        /// <summary>
        /// Notifies that the Parts collection has changed
        /// </summary>
        internal void UpdatedParts()
        {
            OnPropertyChanged("Parts");
        }

        /// <summary>
        /// Converts each property in the media item that is to be saved to the database to a parameter to pass to a stored procedure
        /// </summary>
        /// <param name="includeId">Value determining whether or not to include the media item's unique identifier in the returned array</param>
        /// <returns>Array containing a stored procedure parameter for each property in the media item that is to be saved to the database</returns>
        internal sp.Parameter[] GetMediaItemParametersForStoredProcedure(Boolean includeId)
        {
            return GetParametersForStoredProcedure(includeId);
        }

        /// <summary>
        /// Determines whether or not the media item contains the specified search in any of it's properties
        /// </summary>
        /// <param name="searchText">Search text to look for</param>
        /// <returns>True if the media item contains the specified search in any of it's properties, false if not</returns>
        public Boolean ContainsSearchText(String searchText)
        {
            if (String.IsNullOrEmpty(searchText))
                return true;

            searchText = searchText.ToLower();

            if (Name.ToLower().Contains(searchText))
                return true;

            if (Genre.ToLower().Contains(searchText))
                return true;

            if (MediaItemContainsSearchText(searchText))
                return true;

            if (Parts.Any(p => p.Location.ToLower().Contains(searchText)))
                return true;

            if (Tags.Any(p => p.ToLower().Contains(searchText)))
                return true;

            return false;
        }

        /// <summary>
        /// Adds a date and time to the media item's play history
        /// </summary>
        public void Played()
        {
            Played(DateTime.Now);
        }

        /// <summary>
        /// Adds a date and time to the media item's play history
        /// </summary>
        /// <param name="datePlayed">Date and time the media item was played to the end</param>
        public void Played(DateTime datePlayed)
        {
            if (IsInDatabase)
            {
                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        conn.Open();
                        Data.MediaItem.AddMediaItemPlayHistory(conn, Id, Type, datePlayed);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            List<DateTime> playHistory = new List<DateTime>(PlayHistory);
            playHistory.Add(datePlayed);
            playHistory.Sort();

            PlayHistory = playHistory.ToArray();
        }

        /// <summary>
        /// Sets the duration of a part in the media item
        /// </summary>
        /// <param name="index">Index of the part in the media item</param>
        /// <param name="duration">New duration of the part</param>
        public void SetPartDuration(Int32 index, TimeSpan duration)
        {
            Parts[index].Duration = duration;
            OnPropertyChanged("Parts");
        }
        
        #endregion

        #region Abstract Members

        /// <summary>
        /// Gets the type of media item the current object is
        /// </summary>
        public abstract MediaItemTypeEnum Type { get; }

        /// <summary>
        /// Gets the names of the properties that can be modified
        /// </summary>
        public abstract IntelligentString[] ModifyProperties { get; }

        /// <summary>
        /// Gets or sets the organised path of the media item
        /// </summary>
        public abstract IntelligentString OrganisedPath { get; }

        /// <summary>
        /// Adds the media item to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void AddMediaItemToDatabase(SqlConnection conn);

        /// <summary>
        /// Updates the media item in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        protected abstract void UpdateMediaItemInDatabase(SqlConnection conn);

        /// <summary>
        /// Converts each property in the media item that is to be saved to the database to a parameter to pass to a stored procedure
        /// </summary>
        /// <returns>Array containing a stored procedure parameter for each property in the media item that is to be saved to the database</returns>
        protected abstract sp.Parameter[] GetMediaItemParametersForStoredProcedure();

        /// <summary>
        /// Determines whether or not the media item contains the specified search in any of it's properties
        /// </summary>
        /// <param name="searchText">Search text to look for</param>
        /// <returns>True if the media item contains the specified search in any of it's properties, false if not</returns>
        protected abstract Boolean MediaItemContainsSearchText(String searchText);

        /// <summary>
        /// Clones the media item
        /// </summary>
        /// <returns>Cloned media item</returns>
        protected abstract MediaItem CloneMediaItem();

        /// <summary>
        /// Parses a file name into relevant properties for the media item
        /// </summary>
        /// <param name="filename">Filename to parse</param>
        public abstract void ParseFilename(String filename);

        /// <summary>
        /// Compares the media item with the specified media item
        /// </summary>
        /// <param name="mediaItem">A media item to compare with this instance</param>
        /// <returns>A value that indicates the relative order of the media items being compared</returns>
        protected abstract int CompareMediaItems(MediaItem mediaItem);

        /// <summary>
        /// Gets the description of the media item
        /// </summary>
        /// <param name="seperator">Separator used to separate the various elements of the description</param>
        /// <returns>Description of the media item</returns>
        protected abstract IntelligentString GetDescription(IntelligentString seperator);

        /// <summary>
        /// Gets the search string for the media item
        /// </summary>
        /// <returns>Search string for the media item</returns>
        public abstract IntelligentString GetSearchString();

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new MediaItem object using the data in a data row
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="row">DataRow containing the data for the media item</param>
        /// <returns>MediaItem object with properties loaded from the database</returns>
        internal static MediaItem FromDataRow(SqlConnection conn, sp.DataRow row)
        {
            MediaItemTypeEnum type = (MediaItemTypeEnum)Convert.ToInt16(row["Type"]);
            MediaItem mediaItem;

            switch (type)
            {
                case MediaItemTypeEnum.Video:
                    mediaItem = Video.FromDataRow(row);
                    break;

                case MediaItemTypeEnum.Song:
                    mediaItem = Song.FromDataRow(row);
                    break;

                default:
                    throw new UnknownEnumValueException(type);
            }

            mediaItem.id = (Int64)row["Id"];
            mediaItem.Genre = (String)row["Genre"];
            mediaItem.name = (String)row["Name"];
            mediaItem.isHidden = (Boolean)row["IsHidden"];
            mediaItem.dateCreated = (DateTime)row["DateCreated"];
            mediaItem.dateModified = (DateTime)row["DateModified"];
            mediaItem.UserName = (String)row["UserName"];

            mediaItem.playHistory = Data.MediaItem.GetMediaItemPlayHistoryById(conn, mediaItem.id, mediaItem.Type);
            mediaItem.parts = MediaItemPart.GetMediaItemPartsById(conn, mediaItem.Id, mediaItem.Type);
            mediaItem.tags = new ObservableCollection<IntelligentString>(Data.MediaItem.GetMediaItemTagsById(conn, mediaItem.id, mediaItem.Type));

            mediaItem.IsInDatabase = true;
            
            return mediaItem;
        }

        /// <summary>
        /// Gets all media items in the system
        /// </summary>
        /// <returns>All media items in the system</returns>
        public static MediaItem[] GetMediaItems()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return Data.MediaItem.GetMediaItems(conn);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Gets the duration in milliseconds of a media file
        /// </summary>
        /// <param name="location">Location of the media file</param>
        /// <returns>Duration in milliseconds of the media file</returns>
        public static TimeSpan GetDuration(IntelligentString location)
        {
            try
            {
                if (File.Exists(location.Value))
                {
                    ShellFile sf = ShellFile.FromFilePath(location.Value);
                    ShellProperty<ulong?> duration = sf.Properties.System.Media.Duration;

                    if (duration.Value.HasValue)
                        return TimeSpan.FromMilliseconds(duration.Value.Value / TimeSpan.TicksPerMillisecond);
                }
            }
            catch (EntryPointNotFoundException) { }

            return TimeSpan.FromMilliseconds(0);
        }

        /// <summary>
        /// Determines whether or a part already exists in the database with the specified location
        /// </summary>
        /// <param name="location">Location to the check for</param>
        /// <returns>True if a part already exists in the database with the specified location, false if not</returns>
        public static Boolean MediaItemPartLocationExists(IntelligentString location)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return Data.MediaItemPart.MediaItemPartLocationExists(conn, location.Value);
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
            if (IntelligentString.IsNullOrEmpty(Name))
            {
                exception = new PropertyNotSetException(typeof(MediaItem), "Name");
                return false;
            }

            exception = null;
            return true;
        }

        protected override void AddToSqlDatabase(SqlConnection conn)
        {            
            AddMediaItemToDatabase(conn);

            foreach (DateTime datePlayed in PlayHistory)
                Data.MediaItem.AddMediaItemPlayHistory(conn, Id, Type, datePlayed);

            foreach (MediaItemPart part in Parts)
                Data.MediaItemPart.AddMediaItemPart(conn, Id, Type, part);

            foreach (IntelligentString tag in Tags)
                Data.MediaItem.AddMediaItemTag(conn, Id, Type, tag.Value);
        }

        protected override void UpdateInSqlDatabase(SqlConnection conn)
        {            
            DateModified = DateTime.Now;
            UpdateMediaItemInDatabase(conn);

            MediaItem clone;

            switch (Type)
            {
                case MediaItemTypeEnum.Song:
                    clone = new Song(Id);
                    break;

                case MediaItemTypeEnum.Video:
                    clone = new Video(Id);
                    break;

                default:
                    throw new UnknownEnumValueException(Type);
            }

            UpdateParts(conn, clone);
            UpdateTags(conn, clone);
        }

        protected override void DeleteFromSqlDatabase(SqlConnection conn)
        {            
            Data.MediaItem.DeleteMediaItem(conn, Id, Type);
        }

        protected override void ResetProperties()
        {
            id = -1;
            playHistory = new DateTime[0];
            parts = new MediaItemPartCollection();
            tags = new ObservableCollection<IntelligentString>();
        }

        public override sp.ParameterCollection GetParametersForStoredProcedure(bool includeId)
        {
            sp.ParameterCollection parameters = new sp.ParameterCollection();

            if (includeId)
                parameters.AddWithValue("MediaItemId", DbType.Int64, Id);

            parameters.AddWithValue("Type", DbType.Int16, Convert.ToInt16(Type));
            parameters.AddWithValue("Name", DbType.String, Name.Value);
            parameters.AddWithValue("Genre", DbType.String, Genre.Value);
            parameters.AddWithValue("IsHidden", DbType.Boolean, IsHidden);
            parameters.AddWithValue("DateCreated", DbType.DateTime, DateCreated);
            parameters.AddWithValue("DateModified", DbType.DateTime, DateModified);
            parameters.AddWithValue("UserName", DbType.String, UserName.Value);

            foreach (sp.Parameter parameter in GetMediaItemParametersForStoredProcedure())
                parameters.Add(parameter);

            return parameters;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            MediaItem mediaItem = obj as MediaItem;

            if (mediaItem == null)
                return 1;

            int typeCompare = Type.CompareTo(mediaItem.Type);

            if (typeCompare == 0)
                return CompareMediaItems(mediaItem);

            return typeCompare;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return CloneMediaItem();
        }

        #endregion
    }
}

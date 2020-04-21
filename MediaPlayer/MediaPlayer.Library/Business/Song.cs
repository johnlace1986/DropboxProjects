using System;
using System.Collections.Generic;
using System.Linq;
using sp = Utilities.Data.StoredProcedure;
using System.Data;
using Utilities.Business;
using System.Data.SqlClient;
using System.IO;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public class Song : MediaItem
    {
        #region Fields

        /// <summary>
        /// Gets or sets the artist the song belongs to
        /// </summary>
        private IntelligentString artist;

        /// <summary>
        /// Gets or sets the album the song belongs to
        /// </summary>
        private IntelligentString album;

        /// <summary>
        /// Gets or sets the disk number in the album the song belongs to
        /// </summary>
        private Int16 diskNumber;

        /// <summary>
        /// Gets or sets the number of disks there are in the album the song belongs to
        /// </summary>
        private Int16 numberOfDisks;

        /// <summary>
        /// Gets or sets the track number in the album the song belongs to
        /// </summary>
        private Int16 trackNumber;

        /// <summary>
        /// Gets or sets the number of tracks there are in the album the song belongs to
        /// </summary>
        private Int16 numberOfTracks;

        /// <summary>
        /// Gets or sets the year the song was released
        /// </summary>
        private Int16 year;

        /// <summary>
        /// Gets or sets the unique identifier of the song in the iTunes library that corresponds to this song
        /// </summary>
        private Int16 itunesId;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the artist the song belongs to
        /// </summary>
        public IntelligentString Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                OnPropertyChanged("Artist");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets the album the song belongs to
        /// </summary>
        public IntelligentString Album
        {
            get { return album; }
            set
            {
                album = value;
                OnPropertyChanged("Album");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets the disk number in the album the song belongs to
        /// </summary>
        public Int16 DiskNumber
        {
            get { return diskNumber; }
            set
            {
                diskNumber = value;
                OnPropertyChanged("DiskNumber");
                OnPropertyChanged("DiskNumberOfString");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets the number of disks there are in the album the song belongs to
        /// </summary>
        public Int16 NumberOfDisks
        {
            get { return numberOfDisks; }
            set
            {
                numberOfDisks = value;
                OnPropertyChanged("NumberOfDisks");
                OnPropertyChanged("DiskNumberOfString");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets the disk number and the number of disks of the song expressed as a string
        /// </summary>
        public IntelligentString DiskNumberOfString
        {
            get
            {
                IntelligentString episode = IntelligentString.Empty;

                if (DiskNumber != 0)
                {
                    episode = DiskNumber.ToString();

                    if (NumberOfDisks != 0)
                    {
                        if (DiskNumber > NumberOfDisks)
                            episode = IntelligentString.Empty;
                        else
                            episode += " of " + NumberOfDisks.ToString();
                    }
                }

                return episode;
            }
        }

        /// <summary>
        /// Gets or sets the track number in the album the song belongs to
        /// </summary>
        public Int16 TrackNumber
        {
            get { return trackNumber; }
            set
            {
                trackNumber = value;
                OnPropertyChanged("TrackNumber");
                OnPropertyChanged("TrackNumberOfString");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets the number of tracks there are in the album the song belongs to
        /// </summary>
        public Int16 NumberOfTracks
        {
            get { return numberOfTracks; }
            set
            {
                numberOfTracks = value;
                OnPropertyChanged("NumberOfTracks");
                OnPropertyChanged("TrackNumberOfString");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets the track number and the number of tracks of the video expressed as a string
        /// </summary>
        public IntelligentString TrackNumberOfString
        {
            get
            {
                IntelligentString episode = IntelligentString.Empty;

                if (TrackNumber != 0)
                {
                    episode = TrackNumber.ToString();

                    if (NumberOfTracks != 0)
                    {
                        if (TrackNumber > NumberOfTracks)
                            episode = IntelligentString.Empty;
                        else
                            episode += " of " + NumberOfTracks.ToString();
                    }
                }

                return episode;
            }
        }

        /// <summary>
        /// Gets or sets the year the song was released
        /// </summary>
        public Int16 Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
                OnPropertyChanged("YearString");
            }
        }

        /// <summary>
        /// Gets or sets the year the song was released, expressed as a String
        /// </summary>
        public IntelligentString YearString
        {
            get
            {
                if (Year == 0)
                    return IntelligentString.Empty;

                return Year.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the song in the iTunes library that corresponds to this song
        /// </summary>
        public Int16 iTunesId
        {
            get { return itunesId; }
            set
            {
                itunesId = value;
                OnPropertyChanged("iTunesId");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Song class
        /// </summary>
        public Song()
            : base()
        {
            Artist = IntelligentString.Empty;
            Album = IntelligentString.Empty;
            DiskNumber = 0;
            NumberOfDisks = 0;
            TrackNumber = 0;
            NumberOfTracks = 0;
            Year = 0;
            iTunesId = 0;
        }

        /// <summary>
        /// Initialises a new instance of the Song class with the properties loaded from the database
        /// </summary>
        /// <param name="songId">Unique identifier of the song</param>
        public Song(Int64 songId)
            : this()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    Song clone = Data.Song.GetSongById(conn, songId);
                    CopyFromClone(clone);

                    foreach (MediaItemPart part in clone.Parts)
                        Parts.Add(part.Location, part.Size, part.Duration);

                    IsInDatabase = clone.IsInDatabase;
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
        /// Creates a new Video object using the data in a data row
        /// </summary>
        /// <param name="row">DataRow containing the data for the video</param>
        /// <returns>Video object with properties loaded from the database</returns>
        internal new static Song FromDataRow(sp.DataRow dr)
        {
            Song song = new Song();

            song.Artist = (String)dr["Artist"];
            song.Album = (String)dr["Album"];
            song.DiskNumber = (Int16)dr["DiskNumber"];
            song.NumberOfDisks = (Int16)dr["NumberOfDisks"];
            song.TrackNumber = (Int16)dr["TrackNumber"];
            song.NumberOfTracks = (Int16)dr["NumberOfTracks"];
            song.Year = (Int16)dr["Year"];
            song.iTunesId = (Int16)dr["iTunesId"];

            return song;
        }

        #endregion

        #region MediaItem Members

        public override void CopyFromClone(MediaItem clone)
        {
            if (clone.Type != MediaItemTypeEnum.Song)
                throw new ArgumentException("Clone is not a song");

            base.CopyFromClone(clone);

            Song song = clone as Song;

            Artist = song.Artist;
            Album = song.Album;
            DiskNumber = song.DiskNumber;
            NumberOfDisks = song.NumberOfDisks;
            TrackNumber = song.TrackNumber;
            NumberOfTracks = song.NumberOfTracks;
            Year = song.Year;
            iTunesId = song.iTunesId;
        }

        public override MediaItemTypeEnum Type
        {
            get { return MediaItemTypeEnum.Song; }
        }

        public override IntelligentString[] ModifyProperties
        {
            get
            {
                List<IntelligentString> modifyProperties = new List<IntelligentString>();
                modifyProperties.Add("Name");
                modifyProperties.Add("Artist");
                modifyProperties.Add("Album");
                modifyProperties.Add("Genre");
                modifyProperties.Add("Year");

                return modifyProperties.ToArray();
            }
        }

        public override IntelligentString OrganisedPath
        {
            get
            {
                IntelligentString organisePath = IntelligentString.Empty;

                if (!IntelligentString.IsNullOrEmpty(Artist.ValidPathValue))
                    organisePath += Artist.ValidPathValue + "\\";

                if (!IntelligentString.IsNullOrEmpty(Album.ValidPathValue))
                    organisePath += Album.ValidPathValue + "\\";

                if (DiskNumber != 0)
                    organisePath += "Disk " + DiskNumber.ToString("00") + "\\";

                if (TrackNumber != 0)
                {
                    String format = "0";

                    if (NumberOfTracks != 0)
                    {
                        for (int i = 1; i < NumberOfTracks.ToString().Length; i++)
                            format += "0";

                        if (format.Length == 1)
                            format += "0";

                        organisePath += TrackNumber.ToString(format) + " ";
                    }
                }

                organisePath += Name.ValidPathValue;

                return organisePath;
            }
        }

        protected override void AddMediaItemToDatabase(SqlConnection conn)
        {
            Id = Data.Song.AddSong(conn, this);
        }

        protected override void UpdateMediaItemInDatabase(SqlConnection conn)
        {
            Data.Song.UpdateSong(conn, this);
        }

        protected override sp.Parameter[] GetMediaItemParametersForStoredProcedure()
        {
            List<sp.Parameter> parameters = new List<sp.Parameter>();
            parameters.Add(new sp.Parameter("Artist", DbType.String, Artist.Value));
            parameters.Add(new sp.Parameter("Album", DbType.String, Album.Value));
            parameters.Add(new sp.Parameter("DiskNumber", DbType.Int16, DiskNumber));
            parameters.Add(new sp.Parameter("NumberOfDisks", DbType.Int16, NumberOfDisks));
            parameters.Add(new sp.Parameter("TrackNumber", DbType.Int16, TrackNumber));
            parameters.Add(new sp.Parameter("NumberOfTracks", DbType.Int16, NumberOfTracks));
            parameters.Add(new sp.Parameter("Year", DbType.Int16, Year));
            parameters.Add(new sp.Parameter("iTunesId", DbType.Int16, iTunesId));

            return parameters.ToArray();
        }

        protected override bool MediaItemContainsSearchText(string searchText)
        {
            if (Artist.ToLower().Contains(searchText))
                return true;

            if (Album.ToLower().Contains(searchText))
                return true;

            if (Year.ToString().Contains(searchText))
                return true;

            return false;
        }

        protected override MediaItem CloneMediaItem()
        {
            return GeneralMethods.Clone<Song>(this);
        }

        public override void ParseFilename(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            Name = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            IsHidden = ((fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);

            DirectoryInfo di = fi.Directory;

            DiskNumber = 0;
            if (di.Name.StartsWith("Disk "))
            {
                if (Int16.TryParse(di.Name.Substring("Disk ".Length), out diskNumber))
                    di = di.Parent;
            }

            List<IntelligentString> parts = new List<IntelligentString>();
            for (int i = 0; i < 2; i++)
            {
                if (di == null)
                    break;

                parts.Add(di.Name);
                di = di.Parent;
            }

            while (parts.Count < 2)
                parts.Insert(0, IntelligentString.Empty);

            Album = parts[0];
            Artist = parts[1];
        }

        protected override int CompareMediaItems(MediaItem mediaItem)
        {
            if (Type == mediaItem.Type)
            {
                Song song = mediaItem as Song;

                if (Genre == song.Genre)
                {
                    if (Artist == song.Artist)
                    {
                        if (Album == song.Album)
                        {
                            if (DiskNumber == song.DiskNumber)
                            {
                                if (TrackNumber == song.TrackNumber)
                                {
                                    return Name.CompareTo(song.Name);
                                }
                                else
                                    return TrackNumber.CompareTo(song.TrackNumber);
                            }
                            else
                                return DiskNumber.CompareTo(song.DiskNumber);
                        }
                        else
                            return Album.CompareTo(song.Album);
                    }
                    else
                        return Artist.CompareTo(song.Artist);
                }
                else
                    return Genre.CompareTo(song.Genre);
            }
            else
                return Type.CompareTo(mediaItem.Type);
        }

        protected override IntelligentString GetDescription(IntelligentString seperator)
        {
            IntelligentString description = IntelligentString.Empty;

            if (!IntelligentString.IsNullOrEmpty(Genre))
                description += Genre.Trim() + seperator;

            if (!IntelligentString.IsNullOrEmpty(Artist))
                description += Artist.Trim() + seperator;

            if (!IntelligentString.IsNullOrEmpty(Album))
                description += Album.Trim() + seperator;

            if (!IntelligentString.IsNullOrEmpty(DiskNumberOfString))
                description += "Disk " + DiskNumberOfString + seperator;

            if (!IntelligentString.IsNullOrEmpty(TrackNumberOfString))
                description += "Track " + TrackNumberOfString + seperator;

            if (description.EndsWith(seperator))
                description = description.Substring(0, description.Length - seperator.Length);

            return description.Trim();
        }

        public override IntelligentString GetSearchString()
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

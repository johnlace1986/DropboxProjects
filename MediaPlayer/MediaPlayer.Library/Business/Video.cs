using System;
using System.Collections.Generic;
using System.Linq;
using sp = Utilities.Data.StoredProcedure;
using System.Data;
using System.IO;
using Utilities.Business;
using System.Data.SqlClient;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public class Video : MediaItem
    {
        #region Fields

        /// <summary>
        /// Gets or sets the program the video belongs to
        /// </summary>
        private IntelligentString program;

        /// <summary>
        /// Gets or sets the series the video belongs to
        /// </summary>
        private Int16 series;

        /// <summary>
        /// Gets or sets the episode number of the video
        /// </summary>
        private Int16 episode;

        /// <summary>
        /// Gets or sets the number of episodes there are in the series the video belongs to
        /// </summary>
        private Int16 numberOfEpisodes;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the program the video belongs to
        /// </summary>
        public IntelligentString Program
        {
            get { return program; }
            set
            {
                program = value;
                OnPropertyChanged("Program");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets the series the video belongs to
        /// </summary>
        public Int16 Series
        {
            get { return series; }
            set
            {
                series = value;
                OnPropertyChanged("Series");
                OnPropertyChanged("SeriesString");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets the series the video belongs to expressed as a string
        /// </summary>
        public IntelligentString SeriesString
        {
            get
            {
                if (Series == 0)
                    return IntelligentString.Empty;

                return Series.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the episode number of the video
        /// </summary>
        public Int16 Episode
        {
            get { return episode; }
            set
            {
                episode = value;
                OnPropertyChanged("Episode");
                OnPropertyChanged("EpisodeString");
                OnPropertyChanged("EpisodeOfString");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets the episode number of the video expressed as a string
        /// </summary>
        public IntelligentString EpisodeString
        {
            get
            {
                if (Episode == 0)
                    return IntelligentString.Empty;

                return Episode.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the number of episodes there are in the series the video belongs to
        /// </summary>
        public Int16 NumberOfEpisodes
        {
            get { return numberOfEpisodes; }
            set
            {
                numberOfEpisodes = value;
                OnPropertyChanged("NumberOfEpisodes");
                OnPropertyChanged("NumberOfEpisodesString");
                OnPropertyChanged("EpisodeOfString");
                OnPropertyChanged("Description");
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets the number of episodes there are in the series the video belongs to expressed as a string
        /// </summary>
        public IntelligentString NumberOfEpisodesString
        {
            get
            {
                if (NumberOfEpisodes == 0)
                    return IntelligentString.Empty;

                return NumberOfEpisodes.ToString();
            }
        }

        /// <summary>
        /// Gets the episode number and the number of episodes of the video expressed as a string
        /// </summary>
        public IntelligentString EpisodeOfString
        {
            get
            {
                IntelligentString episode = IntelligentString.Empty;

                if (Episode != 0)
                {
                    episode = Episode.ToString();

                    if (NumberOfEpisodes != 0)
                    {
                        if (Episode > NumberOfEpisodes)
                            episode = IntelligentString.Empty;
                        else
                            episode += " of " + NumberOfEpisodes.ToString();
                    }
                }

                return episode;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Video class
        /// </summary>
        public Video()
            : base()
        {
            Program = IntelligentString.Empty;
            Series = 0;
            Episode = 0;
            NumberOfEpisodes = 0;
        }

        /// <summary>
        /// Initialises a new instance of the Video class with the properties loaded from the database
        /// </summary>
        /// <param name="videoId">Unique identifier of the video</param>
        public Video(Int64 videoId)
            : this()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    Video clone = Data.Video.GetVideoById(conn, videoId);
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

        #region Instance Methods

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new Video object using the data in a data row
        /// </summary>
        /// <param name="row">DataRow containing the data for the video</param>
        /// <returns>Video object with properties loaded from the database</returns>
        internal new static Video FromDataRow(sp.DataRow dr)
        {
            Video video = new Video();

            video.Program = (String)dr["Program"];
            video.Series = (Int16)dr["Series"];
            video.Episode = (Int16)dr["Episode"];
            video.NumberOfEpisodes = (Int16)dr["NumberOfEpisodes"];

            return video;
        }

        #endregion

        #region MediaItem Members

        public override void CopyFromClone(MediaItem clone)
        {
            if (clone.Type != MediaItemTypeEnum.Video)
                throw new ArgumentException("Clone is not a video");

            base.CopyFromClone(clone);

            Video video = clone as Video;

            Program = video.Program;
            Series = video.Series;
            Episode = video.Episode;
            NumberOfEpisodes = video.NumberOfEpisodes;
        }

        public override MediaItemTypeEnum Type
        {
            get { return MediaItemTypeEnum.Video; }
        }

        public override IntelligentString[] ModifyProperties
        {
            get
            {
                List<IntelligentString> modifyProperties = new List<IntelligentString>();
                modifyProperties.Add("Name");
                modifyProperties.Add("Program");
                modifyProperties.Add("Genre");
                modifyProperties.Add("Episode");
                
                return modifyProperties.ToArray();
            }
        }

        public override IntelligentString OrganisedPath
        {
            get
            {
                IntelligentString organisePath = IntelligentString.Empty;

                if (!IntelligentString.IsNullOrEmpty(Genre.ValidPathValue))
                    organisePath += Genre.ValidPathValue + "\\";

                if (!IntelligentString.IsNullOrEmpty(Program.ValidPathValue))
                    organisePath += Program.ValidPathValue + "\\";

                if (Series != 0)
                    organisePath += "Series " + Series.ToString("00") + "\\";

                if (Episode != 0)
                {
                    String format = "0";

                    if (NumberOfEpisodes != 0)
                    {
                        for (int i = 1; i < NumberOfEpisodes.ToString().Length; i++)
                            format += "0";

                        if (format.Length == 1)
                            format += "0";

                        organisePath += Episode.ToString(format) + " ";
                    }
                }

                organisePath += Name.ValidPathValue;

                return organisePath;
            }
        }

        protected override void AddMediaItemToDatabase(SqlConnection conn)
        {
            Id = Data.Video.AddVideo(conn, this);
        }

        protected override void UpdateMediaItemInDatabase(SqlConnection conn)
        {
            Data.Video.UpdateVideo(conn, this);
        }

        protected override sp.Parameter[] GetMediaItemParametersForStoredProcedure()
        {
            List<sp.Parameter> parameters = new List<sp.Parameter>();
            parameters.Add(new sp.Parameter("Program", DbType.String, Program.Value));
            parameters.Add(new sp.Parameter("Series", DbType.Int16, Series));
            parameters.Add(new sp.Parameter("Episode", DbType.Int16, Episode));
            parameters.Add(new sp.Parameter("NumberOfEpisodes", DbType.Int16, NumberOfEpisodes));

            return parameters.ToArray();
        }

        protected override bool MediaItemContainsSearchText(String searchText)
        {
            if (Program.ToLower().Contains(searchText))
                return true;

            return false;
        }

        protected override MediaItem CloneMediaItem()
        {
            return GeneralMethods.Clone<Video>(this);
        }

        public override void ParseFilename(String filename)
        {
            FileInfo fi = new FileInfo(filename);
            Name = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            IsHidden = ((fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);

            DirectoryInfo di = fi.Directory;

            Series = 0;
            if (di.Name.StartsWith("Series "))
            {
                if (Int16.TryParse(di.Name.Substring("Series ".Length), out series))
                    di = di.Parent;
            }

            if (di != null)
            {
                if (di.Parent == null)
                    Genre = di.Name;
                else
                {
                    Program = di.Name;
                    Genre = di.Parent.Name;
                }
            }
        }

        protected override int CompareMediaItems(MediaItem mediaItem)
        {
            if (Type == mediaItem.Type)
            {
                Video video = mediaItem as Video;

                if (Genre == video.Genre)
                {
                    if (Program == video.Program)
                    {
                        if (Series == video.Series)
                        {
                            if (Episode == video.Episode)
                            {
                                return Name.CompareTo(video.Name);
                            }
                            else
                                return Episode.CompareTo(video.Episode);
                        }
                        else
                            return Series.CompareTo(video.Series);
                    }
                    else
                        return Program.CompareTo(video.Program);
                }
                else
                    return Genre.CompareTo(video.Genre);
            }
            else
                return Type.CompareTo(mediaItem.Type);
        }

        protected override IntelligentString GetDescription(IntelligentString seperator)
        {
            IntelligentString description = IntelligentString.Empty;

            if (!IntelligentString.IsNullOrEmpty(Genre))
                description += Genre.Trim() + seperator;

            if (!IntelligentString.IsNullOrEmpty(Program))
                description += Program.Trim() + seperator;

            if (Series != 0)
                description += "Series " + Series.ToString() + seperator;

            if (!IntelligentString.IsNullOrEmpty(EpisodeOfString))
                description += "Episode " + EpisodeOfString + seperator;

            if (description.EndsWith(seperator))
                description = description.Substring(0, description.Length - seperator.Length);

            return description.Trim(); 
        }

        public override IntelligentString GetSearchString()
        {
            IntelligentString searchString = IntelligentString.Empty;

            if ((!IntelligentString.IsNullOrEmpty(Program)) &&
                (Series != 0) &&
                (Episode != 0))
            {
                searchString += Program + " S" + Series.ToString("00") + "E" + Episode.ToString("00");
            }
            else
                searchString = Name;

            return searchString;
        }

        #endregion
    }
}

using System;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using MediaPlayer.Library.Business;
using System.IO;
using Utilities.Business;

namespace MediaPlayer.ImportFromVideoPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["VideoPlayer"].ConnectionString))
            {
                try
                {
                    conn.Open();

                    int videoCount;

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Video", conn))
                    {
                        videoCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    Console.WriteLine(videoCount.ToString() + " video(s)...");
                    int ptr = 0;

                    String format = "";

                    for (int i = 0; i < videoCount.ToString().Length; i++)
                        format += "0";

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Video", conn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ptr++;
                                Video video = new Video();

                                if (dr["Episode"] != DBNull.Value)
                                    video.Episode = Convert.ToInt16(dr["Episode"]);

                                if (dr["Category"] != DBNull.Value)
                                    video.Genre = (String)dr["Category"];

                                if (dr["IsHidden"] != DBNull.Value)
                                    video.IsHidden = Convert.ToBoolean(dr["IsHidden"]);

                                if (dr["Title"] != DBNull.Value)
                                    video.Name = (String)dr["Title"];

                                if (dr["NumberOfEpisodes"] != DBNull.Value)
                                    video.NumberOfEpisodes = Convert.ToInt16(dr["NumberOfEpisodes"]);

                                if (dr["Program"] != DBNull.Value)
                                    video.Program = (String)dr["Program"];

                                if (dr["Series"] != DBNull.Value)
                                    video.Series = Convert.ToInt16(dr["Series"]);

                                DateTime dateCreated = (DateTime)dr["DateAdded"];
                                video.DateCreated = dateCreated;

                                IntelligentString location = (String)dr["Location"];
                                int hours = (int)dr["Hours"];
                                int minutes = (int)dr["Minutes"];
                                int seconds = (int)dr["Seconds"];
                                int miliseconds = (int)dr["Miliseconds"];
                                TimeSpan duration = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds) + TimeSpan.FromMilliseconds(miliseconds);

                                Int64 size = 0;

                                FileInfo fi = new FileInfo(location.Value);

                                if (fi.Exists)
                                    size = fi.Length;

                                video.Parts.Add(location, size, duration);

                                if (dr["PlayCount"] != DBNull.Value)
                                {
                                    int playCount = (int)dr["PlayCount"];

                                    for (int i = 0; i < playCount; i++)
                                        video.Played(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(duration.TotalMilliseconds * i)));
                                }

                                video.Save(conn);
                                Console.Write(ptr.ToString(format) + " / " + videoCount.ToString());

                                if (!video.IsHidden)
                                    Console.Write(": " + location);

                                Console.WriteLine();
                            }
                        }
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

            Console.Write("Finished.");
            Console.Read();
        }
    }
}
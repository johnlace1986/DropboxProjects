using System;
using System.Linq;

namespace MediaPlayer.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //#region QI

            //foreach (Video video in MediaItem.GetMediaItems().Where(p => p.Type == MediaItemTypeEnum.Video && ((Video)p).Program == "QI"))
            //{
            //    int bracketPos = video.Name.IndexOf("(");

            //    if (bracketPos != -1)
            //    {
            //        IntelligentString str = video.Name.Substring(bracketPos + 1, video.Name.Length - bracketPos - 2);
            //        video.Name = video.Name.Substring(0, bracketPos).Trim();

            //        IntelligentString[] panelists = str.Replace(" and ", ",").Split(",");

            //        video.Tags.Clear();

            //        foreach (IntelligentString panelist in panelists)
            //            video.Tags.Add(panelist.Trim());

            //        video.Save();
            //        Console.WriteLine(str);
            //    }
            //}

            //Console.Write("Finished QI.");
            //Console.Read();

            //#endregion

            //#region Would I Lie to You

            //foreach (Video video in MediaItem.GetMediaItems().Where(p => p.Type == MediaItemTypeEnum.Video && ((Video)p).Program.ToLower() == "Would I Lie to You?".ToLower()))
            //{
            //    IntelligentString[] panelists = video.Name.Replace(" and ", ",").Split(",");

            //    video.Tags.Clear();

            //    foreach (IntelligentString panelist in panelists)
            //        video.Tags.Add(panelist.Trim());

            //    video.Name = "Episode " + video.Episode.ToString();
            //    video.Save();
            //}

            //Console.Write("Finished Would I Lie to You.");
            //Console.Read();

            //#endregion
        }
    }
}

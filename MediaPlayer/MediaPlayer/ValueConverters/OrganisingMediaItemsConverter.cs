using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Library.Business;
using MediaPlayer.Business;
using Utilities.Business;
using System.IO;

namespace MediaPlayer.ValueConverters
{
    public class OrganisingMediaItemsConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Gets the media items that need to be organised from the specified collection
        /// </summary>
        /// <param name="mediaItems">Collection of media items being checked</param>
        /// <param name="rootFolders">Collection of root folders the media items could be organised into</param>
        /// <param name="organiseHiddenMediaItems">Value determining whether or not hidden media items should be organised</param>
        /// <param name="organiseMissingMediaItems">Value determining whether or not media items with missing parts should be organised</param>
        /// <returns>Media items that need to be organised from the specified collection</returns>
        private static OrganisingMediaItemPart[] GetTypedUnorganisedMediaItems(MediaItem[] mediaItems, RootFolder[] rootFolders, Boolean? organiseHiddenMediaItems, Boolean organiseMissingMediaItems)
        {
            if (rootFolders.Length == 0)
                return new OrganisingMediaItemPart[0];

            List<OrganisingMediaItemPart> unorganised = new List<OrganisingMediaItemPart>();

            foreach (MediaItem mediaItem in mediaItems)
            {
                if (organiseHiddenMediaItems.HasValue)
                    if (mediaItem.IsHidden != organiseHiddenMediaItems.Value)
                        continue;

                if (!organiseMissingMediaItems)
                    if (!mediaItem.Parts.PartsExist)
                        continue;

                foreach (MediaItemPart part in mediaItem.Parts)
                {
                    Boolean isPartOrgnised = false;
                    Boolean requiresMove = true;

                    IntelligentString organisedPath = mediaItem.Parts.GetPartOrganisedPath(mediaItem.OrganisedPath, part.Index);

                    foreach (RootFolder rootFolder in rootFolders)
                    {
                        IntelligentString rootFolderPath = rootFolder.Path;

                        if (!rootFolderPath.EndsWith("\\"))
                            rootFolderPath += "\\";

                        IntelligentString fullOrganisedPath = rootFolderPath + organisedPath;

                        if (part.Location == fullOrganisedPath)
                        {
                            isPartOrgnised = true;
                            requiresMove = false;
                            break;
                        }
                    }

                    FileInfo fi = new FileInfo(part.Location.Value);
                    Boolean isFileHidden = ((fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);

                    if (isFileHidden != mediaItem.IsHidden)
                        isPartOrgnised = false;

                    if (!isPartOrgnised)
                        unorganised.Add(new OrganisingMediaItemPart(mediaItem, part.Index, organisedPath, requiresMove));
                }
            }

            return unorganised.ToArray();
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<OrganisingMediaItemPart> unorganised = new List<OrganisingMediaItemPart>();

            if (values.Length == 6)
            {
                IEnumerable<Video> videos = values[0] as IEnumerable<Video>;

                if (videos != null)
                {
                    IEnumerable<RootFolder> videoRootFolders = values[1] as IEnumerable<RootFolder>;

                    if (videoRootFolders != null)
                    {
                        IEnumerable<Song> songs = values[2] as IEnumerable<Song>;

                        if (songs != null)
                        {
                            IEnumerable<RootFolder> songRootFolders = values[3] as IEnumerable<RootFolder>;

                            if (songRootFolders != null)
                            {
                                Boolean? organiseHiddenMediaItems = (Boolean?)values[4];
                                Boolean organiseMissingMediaItems = (Boolean)values[5];

                                unorganised.AddRange(GetTypedUnorganisedMediaItems(videos.ToArray(), videoRootFolders.ToArray(), organiseHiddenMediaItems, organiseMissingMediaItems));
                                unorganised.AddRange(GetTypedUnorganisedMediaItems(songs.ToArray(), songRootFolders.ToArray(), organiseHiddenMediaItems, organiseMissingMediaItems));
                            }
                        }
                    }
                }
            }

            return unorganised.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

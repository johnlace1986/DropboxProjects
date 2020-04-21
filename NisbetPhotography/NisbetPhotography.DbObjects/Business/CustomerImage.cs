using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NisbetPhotography.DbObjects.Business
{
    public struct CustomerImage
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the image
        /// </summary>
        public Int16 Id { get; internal set; }

        /// <summary>
        /// URL of the image
        /// </summary>
        public String ImageUrl { get; internal set; }

        /// <summary>
        /// Determines whether the image is used as the thumbnail for it's parent album
        /// </summary>
        public Boolean Thumbnail { get; internal set; }

        /// <summary>
        /// Determines the name of the image
        /// </summary>
        public String Name
        {
            get
            {
                string filename = ImageUrl.Substring(ImageUrl.LastIndexOf("/") + 1);
                string extension = ImageUrl.Substring(ImageUrl.LastIndexOf("."));
                return filename.Substring(0, filename.Length - extension.Length);
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NisbetPhotography.DbObjects.Business
{
    public struct PublicImage
    {
        /// <summary>
        /// Unique identifier for the image
        /// </summary>
        public Int16 Id { get; internal set; }

        /// <summary>
        /// URL of the physical image file
        /// </summary>
        public String ImageUrl { get; internal set; }

        /// <summary>
        /// Caption to display with the image
        /// </summary>
        public String Caption { get; internal set; }
        
        /// <summary>
        /// Determines whether the image is used as the thumbnail for it's parent album
        /// </summary>
        public Boolean Thumbnail { get; internal set; }
    }
}

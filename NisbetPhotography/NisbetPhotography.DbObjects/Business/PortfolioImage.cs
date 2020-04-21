using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NisbetPhotography.DbObjects.Business
{
    public struct PortfolioImage
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
        /// Determines whether the image is used as the thumbnail for it's parent category
        /// </summary>
        public Boolean Thumbnail { get; internal set; }
        
        #endregion
    }
}

using System;
using System.Linq;
using System.Windows;
using MediaPlayer.Library.Business;

namespace MediaPlayer.Business
{
    public interface IMediaItemView
    {
        #region Properties

        /// <summary>
        /// Gets or sets the currently selected element
        /// </summary>
        UIElement SelectedElement { get; set; }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Copies the properties from the source media item to the target media item
        /// </summary>
        /// <param name="target">Target media item who is copying the properties from the source media item</param>
        void CopyPropertiesFromClone(MediaItem target);

        /// <summary>
        /// Parses the specified filename
        /// </summary>
        /// <param name="filename">Filename to parse</param>
        void ParseFilename(String filename);

        #endregion
    }
}

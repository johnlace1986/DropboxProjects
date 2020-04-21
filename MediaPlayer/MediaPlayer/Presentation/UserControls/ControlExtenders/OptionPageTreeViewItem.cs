using System;
using System.Linq;
using System.Windows.Controls;
using MediaPlayer.Business;

namespace MediaPlayer.Presentation.UserControls.ControlExtenders
{
    public class OptionPageTreeViewItem : TreeViewItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets type of the page
        /// </summary>
        public OptionsPageTypeEnum PageType { get; set; }

        #endregion
    }
}

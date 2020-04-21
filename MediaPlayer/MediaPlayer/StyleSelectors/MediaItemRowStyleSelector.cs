using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Library.Business;

namespace MediaPlayer.StyleSelectors
{
    public class MediaItemRowStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            DataGridRow dgr = container as DataGridRow;
            MediaItem mediaItem = item as MediaItem;

            if (mediaItem != null)
            {
                if (mediaItem.IsHidden)
                    return (Style)dgr.FindResource("hiddenMediaItemRowStyle");
            }

            return base.SelectStyle(item, container);
        }
    }
}

using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using MediaPlayer.Library.Business;
using Utilities.Business;
using MediaPlayer.Presentation.UserControls.MediaItemViews;

namespace MediaPlayer.TemplateSelectors
{
    public class MediaItemPartsRowDetailSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ContentPresenter cp = container as ContentPresenter;
            Boolean allowMergeExtract = true;

            MediaItemsView miv = VisualTreeHelpers.FindAncestor<MediaItemsView>(container);

            if (miv != null)
                allowMergeExtract = miv.AllowMergeExtract;

            if (allowMergeExtract)
            {
                MediaItem mediaItem = item as MediaItem;

                if (mediaItem.Parts.Count > 1)
                    return (DataTemplate)cp.FindResource("mediaItemPartsRowDetailsTemplate");
            }

            return (DataTemplate)cp.FindResource("emptyDataTemplate");
        }
    }
}

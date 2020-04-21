using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business;

namespace MediaPlayer.TemplateSelectors
{
    public class OrganisingProgressCellTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            OrganisingMediaItemPart part = item as OrganisingMediaItemPart;

            if (part != null)
            {
                ContentPresenter cp = container as ContentPresenter;

                if (cp != null)
                {
                    switch (part.Status)
                    {
                        case OrganisingMediaItemPartStatus.Waiting:
                            return (DataTemplate)cp.FindResource("organisingProgressBarTemplate");
                        case OrganisingMediaItemPartStatus.Organising:
                            return (DataTemplate)cp.FindResource("organisingProgressBarTemplate");
                        case OrganisingMediaItemPartStatus.Organised:
                            return (DataTemplate)cp.FindResource("organisingSuccessBarTemplate");
                        case OrganisingMediaItemPartStatus.Error:
                            return (DataTemplate)cp.FindResource("organisingErrorBarTemplate");
                    }
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}

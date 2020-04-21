using System;
using System.Linq;
using System.Windows.Data;
using MediaPlayer.Business.ModificationRules;
using MediaPlayer.Presentation.UserControls.ModificationRules;

namespace MediaPlayer.ValueConverters
{
    public class ModificationRuleContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TrimRule)
            {
                TrimRuleView trv = new TrimRuleView();
                trv.Rule = value as TrimRule;

                return trv;
            }

            if (value is ReplaceRule)
            {
                ReplaceRuleView rrv = new ReplaceRuleView();
                rrv.Rule = value as ReplaceRule;

                return rrv;
            }

            if (value is PrefixRule)
            {
                PrefixRuleView prv = new PrefixRuleView();
                prv.Rule = value as PrefixRule;

                return prv;
            }

            if (value is SubtractRule)
            {
                SubtractRuleView srv = new SubtractRuleView();
                srv.Rule = value as SubtractRule;

                return srv;
            }

            if (value == null)
                return null;

            return "Not implemented.";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

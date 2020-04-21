using System;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using MediaPlayer.Business.ModificationRules;
using Utilities.Business;

namespace MediaPlayer.ValueConverters
{
    public class ModificationRulesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return GeneralMethods.LoadModules<IRule>(Assembly.GetExecutingAssembly());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

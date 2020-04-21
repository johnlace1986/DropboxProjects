using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Utilities.Business
{
    public class EnumerationExtension : MarkupExtension
    {
        #region Fields

        private Type enumType;

        #endregion

        #region Properties

        public Type EnumType
        {
            get { return enumType; }
            private set
            {
                if (EnumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                this.enumType = value;
            }
        }

        #endregion

        #region Constructors

        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentException("enumType");

            EnumType = enumType;
        }

        #endregion

        #region MarkupExtension Members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }

        #endregion
    }
}

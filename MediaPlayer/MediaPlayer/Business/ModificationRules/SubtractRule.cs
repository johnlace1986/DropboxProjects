using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Business;

namespace MediaPlayer.Business.ModificationRules
{
    public class SubtractRule : NotifyPropertyChangedObject, IRule
    {
        #region Fields

        /// <summary>
        /// Gets or sets the value that is to be subtracted
        /// </summary>
        private Int64 value;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value that is to be subtracted
        /// </summary>
        public Int64 Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SubtractRule class
        /// </summary>
        public SubtractRule()
            : base()
        {
            Value = 0;
        }

        #endregion

        #region IRule Members

        public IntelligentString DisplayName
        {
            get { return "Subtract"; }
        }

        public bool Validate(Type propertyType, out String errorMessage)
        {
            if ((propertyType != typeof(Int16)) &&
                (propertyType != typeof(Int32)) &&
                (propertyType != typeof(Int64)))
            {
                errorMessage = "Cannot subtract from a value of type " + propertyType.Name;
                return false;
            }

            errorMessage = null;
            return true;
        }

        public object Apply(Type propertyType, object value)
        {
            Int64 originalValue = Convert.ToInt64(value);
            Int64 result = originalValue - Value;

            switch (propertyType.Name.ToLower())
            {
                case "int16":
                    return Convert.ToInt16(result);

                case "int32":
                    return Convert.ToInt32(result);

                case "int64":
                    return result;

                default:
                    throw new ArgumentException("Unknown property type: " + propertyType.Name.ToLower());
            }
        }

        public int CompareTo(object obj)
        {
            IRule rule = obj as IRule;

            return DisplayName.CompareTo(rule.DisplayName);
        }

        #endregion
    }
}

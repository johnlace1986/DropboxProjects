using System;
using System.Linq;
using Utilities.Business;

namespace MediaPlayer.Business.ModificationRules
{
    public class PrefixRule : NotifyPropertyChangedObject, IRule
    {
        #region Fields

        /// <summary>
        /// Gets or sets the value to prefix
        /// </summary>
        private IntelligentString prefix;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to prefix
        /// </summary>
        public IntelligentString Prefix
        {
            get { return prefix; }
            set
            {
                prefix = value;
                OnPropertyChanged("Prefix");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the PrefixRule class
        /// </summary>
        public PrefixRule()
            : base()
        {
            Prefix = IntelligentString.Empty;
        }

        #endregion

        #region IRule Members

        public IntelligentString DisplayName
        {
            get { return "Prefix"; }
        }

        public Boolean Validate(Type propertyType, out String errorMessage)
        {
            if (propertyType != typeof(IntelligentString))
            {
                errorMessage = "Cannot apply a prefix to a value of type " + propertyType.Name;
                return false;
            }

            if (IntelligentString.IsNullOrEmpty(Prefix))
            {
                errorMessage = "Prefix cannot be empty";
                return false;
            }

            errorMessage = null;
            return true;
        }

        public object Apply(Type propertyType, object value)
        {
            return Prefix + value.ToString();
        }

        public int CompareTo(object obj)
        {
            IRule rule = obj as IRule;

            return DisplayName.CompareTo(rule.DisplayName);
        }

        #endregion
    }
}

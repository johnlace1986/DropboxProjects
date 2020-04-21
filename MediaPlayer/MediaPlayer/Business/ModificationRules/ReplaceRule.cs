using System;
using System.Linq;
using Utilities.Business;

namespace MediaPlayer.Business.ModificationRules
{
    [Serializable]
    public class ReplaceRule : NotifyPropertyChangedObject, IRule
    {
        #region Fields

        /// <summary>
        /// Gets or sets the value to replace
        /// </summary>
        private IntelligentString oldValue;

        /// <summary>
        /// Gets or sets the value to replace the old value with
        /// </summary>
        private IntelligentString newValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to replace
        /// </summary>
        public IntelligentString OldValue
        {
            get { return oldValue; }
            set
            {
                oldValue = value;
                OnPropertyChanged("OldValue");
            }
        }

        /// <summary>
        /// Gets or sets the value to replace the old value with
        /// </summary>
        public IntelligentString NewValue
        {
            get { return newValue; }
            set
            {
                newValue = value;
                OnPropertyChanged("NewValue");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Intialises a new instance of the ReplaceRule class
        /// </summary>
        public ReplaceRule()
            : base()
        {
            OldValue = IntelligentString.Empty;
            NewValue = IntelligentString.Empty;
        }

        #endregion

        #region IRule Members

        public IntelligentString DisplayName
        {
            get { return "Replace"; }
        }

        public Boolean Validate(Type propertyType, out String errorMessage)
        {
            if (propertyType != typeof(IntelligentString))
            {
                errorMessage = "Cannot do a find and replace to a value of type " + propertyType.Name;
                return false;
            }
            
            if (IntelligentString.IsNullOrEmpty(OldValue))
            {
                errorMessage = "Text to replace cannot be empty";
                return false;
            }

            if (IntelligentString.IsNullOrEmpty(NewValue))
            {
                errorMessage = "Text to replace old text with cannot be empty";
                return false;
            }

            errorMessage = null;
            return true;
        }

        public object Apply(Type propertyType, object value)
        {
            return ((IntelligentString)value).Replace(OldValue, NewValue);
        }

        public int CompareTo(object obj)
        {
            IRule rule = obj as IRule;

            return DisplayName.CompareTo(rule.DisplayName);
        }

        #endregion
    }
}

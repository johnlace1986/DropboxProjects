using System;
using System.Linq;
using Utilities.Business;
using Utilities.Exception;

namespace MediaPlayer.Business.ModificationRules
{
    [Serializable]
    public class TrimRule : NotifyPropertyChangedObject, IRule
    {
        #region Fields

        /// <summary>
        /// Gets or sets the number of characters that are to be trimmed
        /// </summary>
        private Int32 charactersToTrim;

        /// <summary>
        /// Gets or sets the position to trim the characters from
        /// </summary>
        private TrimRulePositionEnum position;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the number of characters that are to be trimmed
        /// </summary>
        public Int32 CharactersToTrim
        {
            get { return charactersToTrim; }
            set
            {
                charactersToTrim = value;
                OnPropertyChanged("CharactersToTrim");
            }
        }

        /// <summary>
        /// Gets or sets the position to trim the characters from
        /// </summary>
        public TrimRulePositionEnum Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Intialises a new instance of the TrimRule class
        /// </summary>
        public TrimRule()
            : base()
        {
            CharactersToTrim = 0;
            Position = TrimRulePositionEnum.Start;
        }

        #endregion

        #region IRule Members
        
        public IntelligentString DisplayName
        {
            get { return "Trim"; }
        }

        public Boolean Validate(Type propertyType, out String errorMessage)
        {
            if (propertyType != typeof(IntelligentString))
            {
                errorMessage = "Cannot trim a value of type " + propertyType.Name;
                return false;
            }

            if (CharactersToTrim == 0)
            {
                errorMessage = "Cannot trim 0 characters";
                return false;
            }

            errorMessage = null;
            return true;
        }

        public object Apply(Type propertyType, object value)
        {
            IntelligentString originalValue = value.ToString();

            switch (Position)
            {
                case TrimRulePositionEnum.Start:
                    return originalValue.Substring(CharactersToTrim);
                case TrimRulePositionEnum.End:
                    return originalValue.Substring(0, originalValue.Length - CharactersToTrim);
                default:
                    throw new UnknownEnumValueException(Position);
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

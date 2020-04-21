using MediaPlayer.Library.Business;
using System;
using System.Linq;
using System.Reflection;
using Utilities.Business;

namespace MediaPlayer.Business.ModificationRules
{
    public class RuleHeader : NotifyPropertyChangedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the index of the rule header
        /// </summary>
        private Int32 index;

        /// <summary>
        /// Gets or sets the rule
        /// </summary>
        private IRule rule;

        /// <summary>
        /// Gets or sets the name of the property the rule will be applied to
        /// </summary>
        private IntelligentString propertyName;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index of the rule header
        /// </summary>
        public Int32 Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged("Index");
            }
        }

        /// <summary>
        /// Gets or sets the rule
        /// </summary>
        public IRule Rule
        {
            get { return rule; }
            set
            {
                rule = value;
                OnPropertyChanged("Rule");
            }
        }

        /// <summary>
        /// Gets or sets the name of the property the rule will be applied to
        /// </summary>
        public IntelligentString PropertyName
        {
            get { return propertyName; }
            set
            {
                propertyName = value;
                OnPropertyChanged("PropertyName");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Intialises a new instance of the RuleHeader class
        /// </summary>
        public RuleHeader()
            : base()
        {
            Index = 0;
            Rule = null;
            PropertyName = null;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Gets the type of the property that the rule will be applied to
        /// </summary>
        /// <param name="mediaItem">Media item the rule will be applied to</param>
        /// <returns>Type of the property that the rule will be applied to</returns>
        public Type GetPropertyType(MediaItem mediaItem)
        {
            if (IntelligentString.IsNullOrEmpty(PropertyName))
                return null;
                        
            PropertyInfo pi = mediaItem.GetType().GetProperty(PropertyName.Value);
            return pi.PropertyType;
        }

        #endregion
    }
}

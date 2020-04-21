using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Utilities.EventArgs
{
    public class PersistsPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        #region Fields

        /// <summary>
        /// Gets or sets the value of the property before it was changed
        /// </summary>
        private object previousValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value of the property before it was changed
        /// </summary>
        public object PreviousValue
        {
            get { return previousValue; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the PersistsPropertyChangedEventArgs class
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        /// <param name="previousValue">Value of the property before it was changed</param>
        public PersistsPropertyChangedEventArgs(String propertyName, object previousValue)
            : base(propertyName)
        {
            this.previousValue = previousValue;
        }

        #endregion
    }
}

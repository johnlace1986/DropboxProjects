using System;
using System.Linq;
using Utilities.Business;

namespace MediaPlayer.Business.ModificationRules
{
    public interface IRule : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets the display name of the rule
        /// </summary>
        IntelligentString DisplayName { get; }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Validates the rule
        /// </summary>
        /// <param name="propertyType">Type of the property the rule is being applied to</param>
        /// <param name="errorMessage">Description of the error that caused validation of the rule to fail</param>
        /// <returns>True if the rule is valid, false if not</returns>
        Boolean Validate(Type propertyType, out String errorMessage);

        /// <summary>
        /// Applies the rule to the value and returns the result
        /// </summary>
        /// <param name="propertyType">Type of the property the rule is being applied to</param>
        /// <param name="value">Value the rule is being applied to</param>
        /// <returns>Value after the rule has been applied</returns>
        object Apply(Type propertyType, object value);

        #endregion
    }
}

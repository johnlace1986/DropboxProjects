using System;
using System.Linq;

namespace MediaPlayer.Business
{
    public interface IOptionsPage
    {
        #region Events

        /// <summary>
        /// Fires when the user submits the values of the control
        /// </summary>
        event EventHandler Submitted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets type of the page
        /// </summary>
        OptionsPageTypeEnum PageType { get; }

        /// <summary>
        /// Gets or sets the options currently displayed in the control's window
        /// </summary>
        Options Options { get; set; }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the Submitted event
        /// </summary>
        void OnSubmitted();

        /// <summary>
        /// Determines whether or not the values in the control are valid
        /// </summary>
        /// <param name="errorMessage">Description of the reason the values in the control are not valid</param>
        /// <returns>True if the values in the control are valid, false if not</returns>
        Boolean Validate(out String errorMessage);

        #endregion
    }
}

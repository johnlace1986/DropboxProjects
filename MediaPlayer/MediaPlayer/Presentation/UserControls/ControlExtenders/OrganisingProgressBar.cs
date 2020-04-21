using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using MediaPlayer.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.UserControls.ControlExtenders
{
    public class OrganisingProgressBar : ProgressBar
    {
        #region Dependency Properties

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(OrganisingMediaItemPartStatus), typeof(OrganisingProgressBar));

        public static readonly DependencyProperty ErrorsProperty = DependencyProperty.Register("Errors", typeof(Dictionary<IntelligentString, System.Exception>), typeof(OrganisingProgressBar));

        public static readonly DependencyProperty ErrorCountProperty = DependencyProperty.Register("ErrorCount", typeof(Int32), typeof(OrganisingProgressBar));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the status of the progress bar
        /// </summary>
        public OrganisingMediaItemPartStatus Status
        {
            get { return (OrganisingMediaItemPartStatus)GetValue(OrganisingProgressBar.StatusProperty); }
            set { SetValue(OrganisingProgressBar.StatusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the errors that occurred while attempting to organise the part
        /// </summary>
        public Dictionary<IntelligentString, System.Exception> Errors
        {
            get { return (Dictionary<IntelligentString, System.Exception>)GetValue(OrganisingProgressBar.ErrorsProperty); }
            set { SetValue(OrganisingProgressBar.ErrorsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of errors that occurred during organisation of the part
        /// </summary>
        public Int32 ErrorCount
        {
            get { return (Int32)GetValue(OrganisingProgressBar.ErrorCountProperty); }
            set { SetValue(OrganisingProgressBar.ErrorCountProperty, value); }
        }

        #endregion
    }
}

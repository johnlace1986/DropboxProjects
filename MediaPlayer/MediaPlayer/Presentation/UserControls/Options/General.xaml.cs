using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaPlayer.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.UserControls.Options
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : UserControl, IOptionsPage
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(General));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the General class
        /// </summary>
        public General()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OnSubmitted();
        }

        #endregion

        #region IOptionsPage Members

        /// <summary>
        /// Fires when the user submits the values of the control
        /// </summary>
        public event EventHandler Submitted;

        /// <summary>
        /// Gets or sets type of the page
        /// </summary>
        public OptionsPageTypeEnum PageType
        {
            get { return OptionsPageTypeEnum.General; }
        }

        /// <summary>
        /// Gets or sets the options currently displayed in the control's window
        /// </summary>
        public Business.Options Options
        {
            get { return GetValue(OptionsProperty) as Business.Options; }
            set { SetValue(OptionsProperty, value); }
        }

        /// <summary>
        /// Fires the Submitted event
        /// </summary>
        public void OnSubmitted()
        {
            if (Submitted != null)
                Submitted(this, new System.EventArgs());
        }

        /// <summary>
        /// Determines whether or not the values in the control are valid
        /// </summary>
        /// <param name="errorMessage">Description of the reason the values in the control are not valid</param>
        /// <returns>True if the values in the control are valid, false if not</returns>
        public Boolean Validate(out String errorMessage)
        {
            if (IntelligentString.IsNullOrEmpty(Options.ViewAllText))
            {
                errorMessage = "Please set 'View All' text";
                return false;
            }

            errorMessage = null;
            return true;
        }

        #endregion
    }
}

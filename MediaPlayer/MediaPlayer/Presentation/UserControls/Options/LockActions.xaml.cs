using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaPlayer.Business;

namespace MediaPlayer.Presentation.UserControls.Options
{
    /// <summary>
    /// Interaction logic for LockActions.xaml
    /// </summary>
    public partial class LockActions : UserControl, IOptionsPage
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(LockActions));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the LockActions class
        /// </summary>
        public LockActions()
        {
            InitializeComponent();
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
            get { return OptionsPageTypeEnum.LockActions; }
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
            errorMessage = null;
            return true;
        }

        #endregion
    }
}

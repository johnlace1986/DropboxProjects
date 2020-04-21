using System;
using System.Linq;
using System.Windows;
using MediaPlayer.Business;
using MediaPlayer.Presentation.UserControls.ControlExtenders;
using Utilities.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for OptionsDialog.xaml
    /// </summary>
    public partial class OptionsDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Options), typeof(OptionsDialog));

        public static readonly DependencyProperty SelectedOptionPageTreeViewItemProperty = DependencyProperty.Register("SelectedOptionPageTreeViewItem", typeof(OptionPageTreeViewItem), typeof(OptionsDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the options currently displayed in the window
        /// </summary>
        public Options Options
        {
            get { return GetValue(OptionsDialog.OptionsProperty) as Options; }
            set { SetValue(OptionsDialog.OptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected option page tree view item
        /// </summary>
        public OptionPageTreeViewItem SelectedOptionPageTreeViewItem
        {
            get{return GetValue(OptionsDialog.SelectedOptionPageTreeViewItemProperty) as OptionPageTreeViewItem;}
            set{SetValue(OptionsDialog.SelectedOptionPageTreeViewItemProperty, value);}
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OptionsDialog class
        /// </summary>
        public OptionsDialog(Options options)
        {
            InitializeComponent();

            Options = options;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Validates the values entered in the window and closes it
        /// </summary>
        private void Submit()
        {
            foreach (IOptionsPage page in grdPages.Children)
            {
                String errorMessage;

                if (!page.Validate(out errorMessage))
                {
                    GeneralMethods.MessageBoxApplicationError(errorMessage);
                    return;
                }
            }

            DialogResult = true;
        }

        #endregion

        #region Event Handlers

        private void trvNavigation_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedOptionPageTreeViewItem = trvNavigation.SelectedItem as OptionPageTreeViewItem;
        }

        private void optionPage_Submitted(object sender, System.EventArgs e)
        {
            Submit();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Submit();
        }

        #endregion
    }
}

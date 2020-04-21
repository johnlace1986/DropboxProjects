using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaPlayer.Business;

namespace MediaPlayer.Presentation.UserControls.Options
{
    /// <summary>
    /// Interaction logic for RootFolderFileExceptions.xaml
    /// </summary>
    public partial class RootFolderFileExceptions : UserControl, IOptionsPage
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(RootFolderFileExceptions));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OptionsDialog class
        /// </summary>
        public RootFolderFileExceptions()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a file exception to the collection
        /// </summary>
        /// <param name="fileException">File exception being added</param>
        private void AddFileException(String fileException)
        {
            if (!String.IsNullOrEmpty(fileException))
            {
                if (!Options.RootFolderFileExceptions.Contains(fileException))
                {
                    Options.RootFolderFileExceptions.Add(fileException);
                    txtNewFileException.Text = String.Empty;
                    FocusManager.SetFocusedElement(this, txtNewFileException);
                }
            }
        }

        /// <summary>
        /// Deletes the selected file exceptions from the collection
        /// </summary>
        private void DeleteSelectedFileExceptions()
        {
            List<String> selectedFileExceptions = new List<String>(lstFileExceptions.SelectedItems.Cast<String>());

            foreach (String fileException in selectedFileExceptions)
                Options.RootFolderFileExceptions.Remove(fileException);
        }

        #endregion

        #region Event Handlers

        private void lstFileExceptions_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteSelectedFileExceptions();
        }

        private void txtNewFileException_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddFileException(txtNewFileException.Text);
        }

        private void btnAddFileException_Click(object sender, RoutedEventArgs e)
        {
            AddFileException(txtNewFileException.Text);
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
            get { return OptionsPageTypeEnum.RootFolderFileExceptions; }
        }

        /// <summary>
        /// Gets or sets the options currently displayed in the control's window
        /// </summary>
        public Business.Options Options
        {
            get { return GetValue(RootFolderFileExceptions.OptionsProperty) as Business.Options; }
            set { SetValue(RootFolderFileExceptions.OptionsProperty, value); }
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

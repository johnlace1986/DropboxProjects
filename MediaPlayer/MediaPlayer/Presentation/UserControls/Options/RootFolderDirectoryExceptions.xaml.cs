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
    /// Interaction logic for RootFolderDirectoryExceptions.xaml
    /// </summary>
    public partial class RootFolderDirectoryExceptions : UserControl, IOptionsPage
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(RootFolderDirectoryExceptions));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RootFolderDirectoryExceptions class
        /// </summary>
        public RootFolderDirectoryExceptions()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a file exception to the collection
        /// </summary>
        /// <param name="fileException">Directory exception being added</param>
        private void AddDirectoryException(String fileException)
        {
            if (!String.IsNullOrEmpty(fileException))
            {
                if (!Options.RootFolderDirectoryExceptions.Contains(fileException))
                {
                    Options.RootFolderDirectoryExceptions.Add(fileException);
                    txtNewDirectoryException.Text = String.Empty;
                    FocusManager.SetFocusedElement(this, txtNewDirectoryException);
                }
            }
        }

        /// <summary>
        /// Deletes the selected file exceptions from the collection
        /// </summary>
        private void DeleteSelectedDirectoryExceptions()
        {
            List<String> selectedDirectoryExceptions = new List<String>(lstDirectoryExceptions.SelectedItems.Cast<String>());

            foreach (String fileException in selectedDirectoryExceptions)
                Options.RootFolderDirectoryExceptions.Remove(fileException);
        }

        #endregion

        #region Event Handlers

        private void lstDirectoryExceptions_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteSelectedDirectoryExceptions();
        }

        private void txtNewDirectoryException_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddDirectoryException(txtNewDirectoryException.Text);
        }

        private void btnAddDirectoryException_Click(object sender, RoutedEventArgs e)
        {
            AddDirectoryException(txtNewDirectoryException.Text);
        }

        #endregion

        #region IOptionsPage Members

        public event EventHandler Submitted;

        public OptionsPageTypeEnum PageType
        {
            get { return OptionsPageTypeEnum.RootFolderDirectoryExceptions; }
        }

        public Business.Options Options
        {
            get { return GetValue(RootFolderDirectoryExceptions.OptionsProperty) as Business.Options; }
            set { SetValue(RootFolderDirectoryExceptions.OptionsProperty, value); }
        }

        public void OnSubmitted()
        {
            if (Submitted != null)
                Submitted(this, new System.EventArgs());
        }

        public Boolean Validate(out String errorMessage)
        {
            errorMessage = null;
            return true;
        }

        #endregion
    }
}

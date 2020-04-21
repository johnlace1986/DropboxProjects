using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using forms = System.Windows.Forms;

namespace Utilities.Presentation.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for FolderBrowserTextBox.xaml
    /// </summary>
    public partial class FolderBrowserTextBox : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FolderBrowserTextBox));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(String), typeof(FolderBrowserTextBox));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text displayed in the control
        /// </summary>
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the description to display in the FolderBrowserDialog window
        /// </summary>
        public String Description
        {
            get { return (String)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FolderBrowserTextBox class
        /// </summary>
        public FolderBrowserTextBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Browses for a folder path
        /// </summary>
        public void Browse()
        {
            using (forms.FolderBrowserDialog fbd = new forms.FolderBrowserDialog())
            {
                fbd.Description = Description;

                if (fbd.ShowDialog() == forms.DialogResult.OK)
                {
                    Text = fbd.SelectedPath;
                }
            }
        }

        #endregion

        #region Event Handlers

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Browse();
        }

        #endregion
    }
}

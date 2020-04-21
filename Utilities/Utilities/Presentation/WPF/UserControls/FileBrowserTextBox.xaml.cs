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
    /// Interaction logic for FileBrowserTextBox.xaml
    /// </summary>
    public partial class FileBrowserTextBox : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FileBrowserTextBox));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(String), typeof(FileBrowserTextBox));

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
        /// Gets or sets the filter used to determine which file types can be browsed for
        /// </summary>
        public String Filter
        {
            get { return (String)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the FileBrowserTextBox class
        /// </summary>
        public FileBrowserTextBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Browses for a filename
        /// </summary>
        public void Browse()
        {
            using (forms.OpenFileDialog ofd = new forms.OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = Filter;

                if (ofd.ShowDialog() == forms.DialogResult.OK)
                {
                    Text = ofd.FileName;
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

using MediaPlayer.Business;
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

namespace MediaPlayer.Presentation.UserControls.Options
{
    /// <summary>
    /// Interaction logic for ExportDirectories.xaml
    /// </summary>
    public partial class ExportDirectories : UserControl, IOptionsPage
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(ExportDirectories));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ExportDirectories class
        /// </summary>
        public ExportDirectories()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds an export directory to the collection
        /// </summary>
        /// <param name="exportDirectory">Export directory being added</param>
        private void AddExportDirectory(String exportDirectory)
        {
            if (!String.IsNullOrEmpty(exportDirectory))
            {
                if (!Options.ExportDirectories.Contains(exportDirectory))
                {
                    Options.ExportDirectories.Add(exportDirectory);
                    txtNewExportDirectory.Text = String.Empty;
                    FocusManager.SetFocusedElement(this, txtNewExportDirectory);
                }
            }
        }

        /// <summary>
        /// Deletes the selected export directory from the collection
        /// </summary>
        private void DeleteSelectedExportDirectories()
        {
            List<String> selectedExportDirectories = new List<String>(lstExportDirectories.SelectedItems.Cast<String>());

            foreach (String exportDirectory in selectedExportDirectories)
                Options.ExportDirectories.Remove(exportDirectory);
        }

        #endregion

        #region Event Handlers

        private void lstExportDirectories_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteSelectedExportDirectories();
        }

        private void txtNewExportDirectory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddExportDirectory(txtNewExportDirectory.Text);
        }

        private void btnAddExportDirectory_Click(object sender, RoutedEventArgs e)
        {
            AddExportDirectory(txtNewExportDirectory.Text);
        }

        #endregion

        #region IOptionsPage Members

        public event EventHandler Submitted;

        public OptionsPageTypeEnum PageType
        {
            get { return OptionsPageTypeEnum.ExportDirectories; }
        }

        public Business.Options Options
        {
            get { return GetValue(OptionsProperty) as Business.Options; }
            set { SetValue(OptionsProperty, value); }
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

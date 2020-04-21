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
    /// Interaction logic for TorrentSearch.xaml
    /// </summary>
    public partial class TorrentSearch : UserControl, IOptionsPage
    {
        #region Dependency Properties

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Business.Options), typeof(TorrentSearch));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the TorrentSearch class
        /// </summary>
        public TorrentSearch()
        {
            InitializeComponent();
        }

        #endregion

        #region IOptionsPage Members

        public event EventHandler Submitted;

        public OptionsPageTypeEnum PageType
        {
            get { return OptionsPageTypeEnum.TorrentSearch; }
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

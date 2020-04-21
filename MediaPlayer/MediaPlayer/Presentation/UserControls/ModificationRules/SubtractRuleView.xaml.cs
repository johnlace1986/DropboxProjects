using MediaPlayer.Business.ModificationRules;
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

namespace MediaPlayer.Presentation.UserControls.ModificationRules
{
    /// <summary>
    /// Interaction logic for SubtractRuleView.xaml
    /// </summary>
    public partial class SubtractRuleView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty RuleProperty = DependencyProperty.Register("Rule", typeof(SubtractRule), typeof(SubtractRuleView), new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the prefix rule displayed in the view
        /// </summary>
        public SubtractRule Rule
        {
            get { return GetValue(RuleProperty) as SubtractRule; }
            set { SetValue(RuleProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the SubtractRuleView class
        /// </summary>
        public SubtractRuleView()
        {
            InitializeComponent();
        }

        #endregion
    }
}

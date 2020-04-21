using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business.ModificationRules;

namespace MediaPlayer.Presentation.UserControls.ModificationRules
{
    /// <summary>
    /// Interaction logic for ReplaceRuleView.xaml
    /// </summary>
    public partial class ReplaceRuleView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty RuleProperty = DependencyProperty.Register("Rule", typeof(ReplaceRule), typeof(ReplaceRuleView), new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the replace rule displayed in the view
        /// </summary>
        public ReplaceRule Rule
        {
            get { return GetValue(RuleProperty) as ReplaceRule; }
            set { SetValue(RuleProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ReplaceRuleView class
        /// </summary>
        public ReplaceRuleView()
        {
            InitializeComponent();
        }

        #endregion
    }
}

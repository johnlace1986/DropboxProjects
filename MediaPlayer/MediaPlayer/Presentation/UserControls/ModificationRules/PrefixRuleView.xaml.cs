using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business.ModificationRules;

namespace MediaPlayer.Presentation.UserControls.ModificationRules
{
    /// <summary>
    /// Interaction logic for PrefixRuleView.xaml
    /// </summary>
    public partial class PrefixRuleView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty RuleProperty = DependencyProperty.Register("Rule", typeof(PrefixRule), typeof(PrefixRuleView), new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the prefix rule displayed in the view
        /// </summary>
        public PrefixRule Rule
        {
            get { return GetValue(RuleProperty) as PrefixRule; }
            set { SetValue(RuleProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the PrefixRuleView class
        /// </summary>
        public PrefixRuleView()
        {
            InitializeComponent();
        }

        #endregion
    }
}

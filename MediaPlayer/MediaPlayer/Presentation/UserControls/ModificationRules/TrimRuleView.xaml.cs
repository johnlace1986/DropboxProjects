using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business.ModificationRules;

namespace MediaPlayer.Presentation.UserControls.ModificationRules
{
    /// <summary>
    /// Interaction logic for TrimRuleView.xaml
    /// </summary>
    public partial class TrimRuleView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty RuleProperty = DependencyProperty.Register("Rule", typeof(TrimRule), typeof(TrimRuleView), new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the trim rule displayed in the view
        /// </summary>
        public TrimRule Rule
        {
            get { return GetValue(RuleProperty) as TrimRule; }
            set { SetValue(RuleProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the TrimRuleView class
        /// </summary>
        public TrimRuleView()
        {
            InitializeComponent();
        }

        #endregion
    }
}

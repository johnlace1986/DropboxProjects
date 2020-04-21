using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using MediaPlayer.Business.ModificationRules;
using MediaPlayer.Library.Business;
using Utilities.Business;

namespace MediaPlayer.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for ModifyMediaItemsDialog.xaml
    /// </summary>
    public partial class ModifyMediaItemsDialog : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty MediaItemsProperty = DependencyProperty.Register("MediaItems", typeof(MediaItem[]), typeof(ModifyMediaItemsDialog), new PropertyMetadata(new MediaItem[0]));

        public static readonly DependencyProperty RulesProperty = DependencyProperty.Register("Rules", typeof(ObservableCollection<RuleHeader>), typeof(ModifyMediaItemsDialog), new PropertyMetadata(new ObservableCollection<RuleHeader>()));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the media items being modified
        /// </summary>
        public MediaItem[] MediaItems
        {
            get { return GetValue(MediaItemsProperty) as MediaItem[]; }
            set { SetValue(MediaItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the modification rules that are to be applied to the media items
        /// </summary>
        public ObservableCollection<RuleHeader> Rules
        {
            get { return GetValue(RulesProperty) as ObservableCollection<RuleHeader>; }
            set { SetValue(RulesProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ModifyMediaItemsDialog class
        /// </summary>
        /// <param name="mediaItems">Media items being modified</param>
        public ModifyMediaItemsDialog(MediaItem[] mediaItems)
        {
            InitializeComponent();

            MediaItems = mediaItems;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Submits the rules
        /// </summary>
        private void SubmitRules()
        {
            if (Rules.Any(p => p.Rule == null))
            {
                MessageBox.Show("Rule has not been selected.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            String errorMessage = String.Empty;
            if (Rules.Any(p => !p.Rule.Validate(p.GetPropertyType(MediaItems[0]), out errorMessage)))
            {
                MessageBox.Show(errorMessage, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Rules.Any(p => IntelligentString.IsNullOrEmpty(p.PropertyName)))
            {
                MessageBox.Show("Property to modify has not been selected", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (MediaItem mediaItem in MediaItems)
            {
                foreach (RuleHeader rule in Rules)
                {
                    PropertyInfo pi = mediaItem.GetType().GetProperty(rule.PropertyName.Value);
                    object value = pi.GetValue(mediaItem);

                    value = rule.Rule.Apply(rule.GetPropertyType(mediaItem), value);
                    pi.SetValue(mediaItem, value);
                }
            }

            DialogResult = true;
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Rules.Clear();
            Rules.Add(new RuleHeader());
        }

        private void btnAddRule_Click(object sender, RoutedEventArgs e)
        {
            //get data context
            Button btnDeleteRule = sender as Button;
            RuleHeader rule = btnDeleteRule.DataContext as RuleHeader;

            //get index of rule that new rule is being added after
            Int32 index = Rules.IndexOf(rule) + 1;

            //update indices of rules that come after the one being inserted
            for (int i = index; i < Rules.Count; i++)
                Rules[i].Index++;

            //insert rule
            Rules.Insert(index, new RuleHeader() { Index = index });
        }

        private void btnDeleteRule_Click(object sender, RoutedEventArgs e)
        {
            //get data context
            Button btnDeleteRule = sender as Button;
            RuleHeader rule = btnDeleteRule.DataContext as RuleHeader;

            //get index of rule being removed
            Int32 index = Rules.IndexOf(rule);

            //remove rule
            Rules.Remove(rule);

            //update indices of remaing rules
            for (int i = index; i < Rules.Count; i++)
                Rules[i].Index--;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SubmitRules();
        }

        #endregion
    }
}

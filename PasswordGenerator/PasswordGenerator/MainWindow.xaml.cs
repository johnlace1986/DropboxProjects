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
using System.Collections.ObjectModel;

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty PasswordsProperty = DependencyProperty.Register("Password", typeof(ObservableCollection<String>), typeof(MainWindow));

        public static readonly DependencyProperty PasswordsToGenerateProperty = DependencyProperty.Register("PasswordsToGenerate", typeof(Int32), typeof(MainWindow));

        public static readonly DependencyProperty ClearBeforeGeneratingProperty = DependencyProperty.Register("ClearBeforeGenerating", typeof(Boolean), typeof(MainWindow));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the passwords that have been generated
        /// </summary>
        public ObservableCollection<String> Passwords
        {
            get { return GetValue(MainWindow.PasswordsProperty) as ObservableCollection<String>; }
            set { SetValue(MainWindow.PasswordsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the quantity of passwords to generate when the button is clicked
        /// </summary>
        public Int32 PasswordsToGenerate
        {
            get { return (Int32)GetValue(MainWindow.PasswordsToGenerateProperty); }
            set { SetValue(MainWindow.PasswordsToGenerateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether the generated passwords should be cleared before a new set of passwords are generated
        /// </summary>
        public Boolean ClearBeforeGenerating
        {
            get { return (Boolean)GetValue(MainWindow.ClearBeforeGeneratingProperty); }
            set { SetValue(MainWindow.ClearBeforeGeneratingProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Passwords = new ObservableCollection<String>();
            PasswordsToGenerate = 10;
            ClearBeforeGenerating = true;
        }

        #endregion

        #region Event Handlers

        private void btnGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            if (ClearBeforeGenerating)
                Passwords.Clear();

            Random rnd = new Random();

            for (int i = 0; i < PasswordsToGenerate; i++)
            {
                String password = String.Empty;

                for (int j = 0; j < 7; j++)
                    password += Convert.ToChar(rnd.Next(65, 90));

                password += rnd.Next(0, 9).ToString();

                Passwords.Insert(0, password);
            }
        }

        private void btnClearPasswords_Click(object sender, RoutedEventArgs e)
        {
            Passwords.Clear();
        }

        #endregion
    }
}

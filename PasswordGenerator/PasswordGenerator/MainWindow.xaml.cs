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

        public static readonly DependencyProperty PasswordsProperty = DependencyProperty.Register("Password", typeof(ObservableCollection<string>), typeof(MainWindow));

        public static readonly DependencyProperty PasswordsToGenerateProperty = DependencyProperty.Register("PasswordsToGenerate", typeof(int), typeof(MainWindow));

        public static readonly DependencyProperty ClearBeforeGeneratingProperty = DependencyProperty.Register("ClearBeforeGenerating", typeof(bool), typeof(MainWindow));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the passwords that have been generated
        /// </summary>
        public ObservableCollection<string> Passwords
        {
            get { return GetValue(MainWindow.PasswordsProperty) as ObservableCollection<string>; }
            set { SetValue(MainWindow.PasswordsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the quantity of passwords to generate when the button is clicked
        /// </summary>
        public int PasswordsToGenerate
        {
            get { return (int)GetValue(MainWindow.PasswordsToGenerateProperty); }
            set { SetValue(MainWindow.PasswordsToGenerateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether the generated passwords should be cleared before a new set of passwords are generated
        /// </summary>
        public bool ClearBeforeGenerating
        {
            get { return (bool)GetValue(MainWindow.ClearBeforeGeneratingProperty); }
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
            Passwords = new ObservableCollection<string>();
            PasswordsToGenerate = 10;
            ClearBeforeGenerating = true;
        }

        #endregion

        #region Event Handlers

        private void btnGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            if (ClearBeforeGenerating)
                Passwords.Clear();
            
            var rnd = new Random();

            for (var i = 0; i < PasswordsToGenerate; i++)
            {
                var password = string.Empty;

                for (var j = 0; j < 11; j++)
                    password += Convert.ToChar(rnd.Next(97, 122));

                password = password[0].ToString().ToUpper() + password.Substring(1);

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

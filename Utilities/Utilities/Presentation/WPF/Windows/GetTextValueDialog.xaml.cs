using System;
using System.Collections;
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
using Utilities.Business;
using Utilities.Exception;

namespace Utilities.Presentation.WPF.Windows
{
    /// <summary>
    /// Interaction logic for GetTextValueDialog.xaml
    /// </summary>
    public partial class GetTextValueDialog : Window
    {
        #region Depencency Properties

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(String), typeof(GetTextValueDialog));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(String), typeof(GetTextValueDialog));

        public static readonly DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(TextValueDialogInputType), typeof(GetTextValueDialog));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(GetTextValueDialog));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(GetTextValueDialog));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(GetTextValueDialog));

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(String), typeof(GetTextValueDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the title text to the left of the text box
        /// </summary>
        public String Header
        {
            get { return GetValue(GetTextValueDialog.HeaderProperty) as String; }
            set { SetValue(GetTextValueDialog.HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the text box
        /// </summary>
        public String Value
        {
            get
            {
                if (InputType == TextValueDialogInputType.PasswordBox)
                    return pwbValue.Password;

                return GetValue(GetTextValueDialog.ValueProperty) as String;
            }
            set
            {
                if (InputType == TextValueDialogInputType.PasswordBox)
                    pwbValue.Password = value;
                else
                    SetValue(GetTextValueDialog.ValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the type of input control that the user uses to enter the text value
        /// </summary>
        public TextValueDialogInputType InputType
        {
            get { return (TextValueDialogInputType)GetValue(GetTextValueDialog.InputTypeProperty); }
            private set { SetValue(GetTextValueDialog.InputTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection used to generate the content of the window
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return GetValue(GetTextValueDialog.ItemsSourceProperty) as IEnumerable; }
            set { SetValue(GetTextValueDialog.ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected item
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return (Int32)GetValue(GetTextValueDialog.SelectedIndexProperty); }
            set { SetValue(GetTextValueDialog.SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected item
        /// </summary>
        public object SelectedItem
        {
            get { return GetValue(GetTextValueDialog.SelectedItemProperty); }
            set { SetValue(GetTextValueDialog.SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path to a value on the source object to serve as the visual representation of the object
        /// </summary>
        public String DisplayMemberPath
        {
            get { return (String)GetValue(GetTextValueDialog.DisplayMemberPathProperty); }
            set { SetValue(GetTextValueDialog.DisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the user is allowed to accept an empty value
        /// </summary>
        public Boolean AllowEmptyValue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public GetTextValueDialog()
        {
            InitializeComponent();

            InputType = TextValueDialogInputType.TextBox;
            AllowEmptyValue = true;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="inputType">Type of input control that the user uses to enter the text value</param>
        public GetTextValueDialog(TextValueDialogInputType inputType)
            : this()
        {
            InputType = inputType;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Submits the entered value
        /// </summary>
        private void SubmitValue()
        {
            if (!AllowEmptyValue)
            {
                if (String.IsNullOrEmpty(Value))
                {
                    GeneralMethods.MessageBoxApplicationError("Please enter a value.");
                    return;
                }
            }

            this.DialogResult = true;
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (InputType)
            {
                case TextValueDialogInputType.TextBox:
                    FocusManager.SetFocusedElement(this, txtValue);
                    txtValue.CaretIndex = txtValue.Text.Length;
                    break;

                case TextValueDialogInputType.FileBrowserTextBox:
                    FocusManager.SetFocusedElement(this, txtFile);
                    txtFile.Browse();
                    break;

                case TextValueDialogInputType.FolderBrowserTextBox:
                    FocusManager.SetFocusedElement(this, txtFolder);
                    txtFolder.Browse();
                    break;

                case TextValueDialogInputType.ComboBox:
                    FocusManager.SetFocusedElement(this, cmbValue);
                    break;

                case TextValueDialogInputType.PasswordBox:
                    FocusManager.SetFocusedElement(this, pwbValue);
                    break;

                case TextValueDialogInputType.DatePicker:
                    FocusManager.SetFocusedElement(this, dpValue);
                    break;

                default:
                    throw new UnknownEnumValueException(InputType);
            }
        }

        private void ele_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    SubmitValue();
                    break;

                case Key.Escape:
                    this.DialogResult = false;
                    break;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SubmitValue();
        }

        #endregion
    }
}

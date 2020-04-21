using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Private.Customer
{
    public partial class PersonalDetails : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (!(IsPostBack))
                BindCustomer();
        }

        private void BindCustomer()
        {
            DbObjects.Business.User customer = CurrentUser;

            lblUserId.Text = customer.UserId.ToString();
            txtFirstName.Text = customer.FirstName;
            txtSurname.Text = customer.Surname;
            txtEmailAddress.Text = customer.EmailAddress;
            lblDateCreated.Text = customer.DateAdded.ToString("dddd d MMMM yyyy");
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {            
            DbObjects.Business.User customer = CurrentUser;

            if (String.IsNullOrEmpty(txtFirstName.Text))
            {
                lblError.Text = "Please enter your first name";
                mpeError.Show();
                return;
            }

            if (String.IsNullOrEmpty(txtSurname.Text))
            {
                lblError.Text = "Please enter your surname";
                mpeError.Show();
                return;
            }

            if (txtEmailAddress.Text != customer.EmailAddress)
            {
                if (String.IsNullOrEmpty(txtEmailAddress.Text))
                {
                    lblError.Text = "Please enter your email address";
                    mpeError.Show();
                    return;
                }

                if (!(IsEmailAddressValid(txtEmailAddress.Text)))
                {
                    lblError.Text = "Please enter a valid email address";
                    mpeError.Show();
                    return;
                }

                if (DbObjects.Business.User.EmailAddressExists(txtEmailAddress.Text))
                {
                    lblError.Text = "There is already another customer with that email address";
                    mpeError.Show();
                    return;
                }
            }

            customer.FirstName = txtFirstName.Text;
            customer.Surname = txtSurname.Text;
            customer.EmailAddress = txtEmailAddress.Text;
            customer.Save();

            BindCustomer();

            lblConfirmation.Text = "Successfully saved changes";
        }

        protected void btnChangePasswordOk_Click(object sender, EventArgs e)
        {
            DbObjects.Business.User customer = CurrentUser;
            lblChangePasswordError.Text = "";

            if (String.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                lblChangePasswordError.Text = "Please enter your current password";
                mpeChangePassword.Show();
                return;
            }

            if (txtCurrentPassword.Text != customer.Password)
            {
                lblChangePasswordError.Text = "That is not your current password";
                mpeChangePassword.Show();
                return;
            }

            if (String.IsNullOrEmpty(txtNewPassword.Text))
            {
                lblChangePasswordError.Text = "Please enter your new password";
                mpeChangePassword.Show();
                return;
            }

            if (txtConfirmPassword.Text != txtNewPassword.Text)
            {
                lblChangePasswordError.Text = "Passwords to not match";
                mpeChangePassword.Show();
                return;
            }

            customer.Password = txtNewPassword.Text;
            customer.Save();

            lblConfirmation.Text = "Successfully changed password";
        }
    }
}
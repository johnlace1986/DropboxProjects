using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class Customer : Business.BasePage
    {
        protected String CustomerImageFolder;

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (SelectedCustomer == null)
                SelectedCustomer = new DbObjects.Business.User();
            else
                if (SelectedCustomer.IsInDatabase)
                    SelectedCustomer = new DbObjects.Business.User(SelectedCustomer.UserId);
            
            CustomerImageFolder = "../Customer/Images/" + SelectedCustomer.UserId.ToString() + "/";

            if (!(IsPostBack))
                BindCustomer();
        }

        private void BindCustomer()
        {
            DbObjects.Business.User customer = SelectedCustomer;

            if (String.IsNullOrEmpty(customer.FullName))
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"Customers.aspx\">Customers</a> >> New Customer";
            else
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"Customers.aspx\">Customers</a> >> " + customer.FullName;
            
            rptAlbums.DataSource = customer.CustomerAlbums;
            rptAlbums.DataBind();
        }

        protected void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (SelectedCustomer.IsInDatabase)
            {
                SelectedCustomer.Delete();
                SelectedCustomer = null;
            }

            Response.Redirect("Customers.aspx");
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            DbObjects.Business.User customer = SelectedCustomer;
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtFirstName.Text))
            {
                lblError.Text = "Please give the customer a first name";
                mpeError.Show();
                return;
            }

            if (String.IsNullOrEmpty(txtSurname.Text))
            {
                lblError.Text = "Please give the customer a surname";
                mpeError.Show();
                return;
            }

            if (String.IsNullOrEmpty(txtEmailAddress.Text))
            {
                lblError.Text = "Please give the customer an email address";
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
            
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                lblError.Text = "Please give the new customer a password";
                mpeError.Show();
                return;
            }

            if (!(txtPassword.Text == txtConfirmPassword.Text))
            {
                lblError.Text = "Please make sure both passwords match";
                mpeError.Show();
                return;
            }
            
            customer.FirstName = txtFirstName.Text;
            customer.Surname = txtSurname.Text;
            customer.EmailAddress = txtEmailAddress.Text;
            customer.Password = txtPassword.Text;

            customer.Save();
            BindCustomer();
        }

        protected void btnAddAlbum_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (!(SelectedCustomer.IsInDatabase))
            {
                lblError.Text = "Please save the new user's details before adding any albums";
                return;
            }

            SelectedCustomerAlbum = new DbObjects.Business.CustomerAlbum();
            SelectedCustomerAlbum.Customer = SelectedCustomer;
            Response.Redirect("CustomerAlbum.aspx");
        }

        protected void rptAlbums_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem ri = e.Item;

            Image imgAlbum = (Image)ri.FindControl("imgAlbum");

            if (String.IsNullOrEmpty(imgAlbum.ImageUrl))
                imgAlbum.ImageUrl = "../../Images/no_thumbnail_image.png";

            int imageCount = SelectedCustomer.CustomerAlbums.Length;

            if (imageCount > 0)
            {
                int rowCount = Convert.ToInt32((Convert.ToDouble(imageCount) / ThumbnailImageColumnCount) + 0.49);
                pnlAlbums.Style["height"] = (rowCount * 238).ToString() + "px";
            }
        }

        protected void lnkAlbum_Click(object sender, EventArgs e)
        {
            LinkButton lnkAlbum = (LinkButton)sender;
            Int16 customerAlbumId = Convert.ToInt16(lnkAlbum.CommandArgument);

            SelectedCustomerAlbum = SelectedCustomer.GetCustomerAlbumById(customerAlbumId);
            Response.Redirect("CustomerAlbum.aspx");
        }
    }
}

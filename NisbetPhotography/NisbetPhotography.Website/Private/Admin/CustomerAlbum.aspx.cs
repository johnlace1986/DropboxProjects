using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class CustomerAlbum : Business.BasePage
    {
        protected String CustomerImageFolder;

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (SelectedCustomer == null)
                Response.Redirect("Customers.aspx");

            if (SelectedCustomerAlbum == null)
            {
                SelectedCustomerAlbum = new DbObjects.Business.CustomerAlbum();
                SelectedCustomerAlbum.Customer = SelectedCustomer;
            }

            CustomerImageFolder = "../Customer/Images/" + SelectedCustomerAlbum.Customer.UserId.ToString().Replace("-", "") + "/";

            String mappedCustomerImageFolder = Server.MapPath(CustomerImageFolder);
            if (!(Directory.Exists(mappedCustomerImageFolder)))
            {
                DbObjects.Business.CustomerAlbum selectedAlbum = SelectedCustomerAlbum;
                Directory.CreateDirectory(mappedCustomerImageFolder);
                SelectedCustomerAlbum = selectedAlbum;
            }

            SelectedCustomer = SelectedCustomerAlbum.Customer;

            if (!(IsPostBack))
                BindAlbum();

            LoadSilverlightControl(ltrInitParams, Business.AlbumTypeEnum.Customer, SelectedCustomerAlbum.Id, SelectedCustomer.UserId);
        }

        private void BindAlbum()
        {
            if (SelectedCustomerAlbum.IsInDatabase)
                SelectedCustomerAlbum = new DbObjects.Business.CustomerAlbum(SelectedCustomerAlbum.Id, SelectedCustomerAlbum.Customer.UserId);

            if (String.IsNullOrEmpty(SelectedCustomerAlbum.Name))
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"Customers.aspx\">Customers</a> >> <a href=\"Customer.aspx\">" + SelectedCustomerAlbum.Customer.FullName + "</a> >> New Album";
            else
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"Customers.aspx\">Customers</a> >> <a href=\"Customer.aspx\">" + SelectedCustomerAlbum.Customer.FullName + "</a> >> " + SelectedCustomerAlbum.Name;

            txtName.Text = SelectedCustomerAlbum.Name;
            txtDescription.Text = SelectedCustomerAlbum.Description;

            rptImages.DataSource = SelectedCustomerAlbum.Images;
            rptImages.DataBind();

            LoadSilverlightControl(ltrInitParams, Business.AlbumTypeEnum.Customer, SelectedCustomerAlbum.Id, SelectedCustomer.UserId);
        }

        protected void rptImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType != ListItemType.Item) && (e.Item.ItemType != ListItemType.AlternatingItem))
                return;

            RadioButton rdo = (RadioButton)e.Item.FindControl("rdoThumbnail");
            string script = "SetUniqueRadioButton('rptImages.*CustomerAlbum',this)";
            rdo.Attributes.Add("onclick", script);

            int imageCount = SelectedCustomerAlbum.Images.Length;

            if (imageCount > 0)
            {
                int rowCount = Convert.ToInt32((Convert.ToDouble(imageCount) / ThumbnailImageColumnCount) + 0.49);
                pnlImages.Style["height"] = (rowCount * 270).ToString() + "px";
            }
        }

        protected void btnDeleteAlbum_Click(object sender, EventArgs e)
        {
            if (SelectedCustomerAlbum.IsInDatabase)
            {
                foreach (DbObjects.Business.CustomerImage image in SelectedCustomerAlbum.Images)
                    DeleteCustomerImage(image);

                SelectedCustomerAlbum.Delete();
                SelectedCustomerAlbum = null;
            }

            Response.Redirect("Customer.aspx");
        }

        private void DeleteCustomerImage(DbObjects.Business.CustomerImage image)
        {
            string absolutePath = Server.MapPath(image.ImageUrl);

            //delete physical image file
            if (File.Exists(absolutePath))
                File.Delete(absolutePath);

            //remove image from database
            SelectedCustomerAlbum.RemoveImage(image.Id);
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtName.Text))
            {
                lblError.Text = "Please give the album a name";
                mpeError.Show();
                return;
            }

            DbObjects.Business.CustomerAlbum album = SelectedCustomerAlbum;

            album.Name = txtName.Text;
            album.Description = txtDescription.Text;
            album.Save();

            Int16 thumbnailId = album.Thumbnail.Id;

            for (int i = 0; i < rptImages.Items.Count; i++)
            {
                RadioButton rdoThumbnail = (RadioButton)rptImages.Items[i].FindControl("rdoThumbnail");
                Int16 imageId = Convert.ToInt16(rdoThumbnail.Attributes["customerImageId"]);

                if (rdoThumbnail.Checked)
                {
                    thumbnailId = imageId;
                    break;
                }
            }

            if (album.Thumbnail.Id != thumbnailId)
                album.SetThumbnailImage(thumbnailId);

            BindAlbum();
        }

        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {
            //get id number
            Button btnDeleteImage = (Button)sender;
            Int16 customerImageId = Convert.ToInt16(btnDeleteImage.CommandArgument);

            DeleteCustomerImage(SelectedCustomerAlbum.GetCustomerImageById(customerImageId));

            //reload
            BindAlbum();
        }
    }
}

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
    public partial class PublicAlbum : Business.BasePage
    {
        protected String PublicImageFolderUrl = "../../Public/";

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (SelectedPublicAlbum == null)
                SelectedPublicAlbum = new DbObjects.Business.PublicAlbum();

            if (!(IsPostBack))
                BindAlbum();

            LoadSilverlightControl(ltrInitParams, Business.AlbumTypeEnum.Public, SelectedPublicAlbum.Id, Guid.Empty);
        }

        private void BindAlbum()
        {
            if (SelectedPublicAlbum.IsInDatabase)
                SelectedPublicAlbum = new DbObjects.Business.PublicAlbum(SelectedPublicAlbum.Id);

            if (String.IsNullOrEmpty(SelectedPublicAlbum.Name))
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"PublicAlbums.aspx\">Public Albums</a> >> New Album";
            else
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"PublicAlbums.aspx\">Public Albums</a> >> " + SelectedPublicAlbum.Name;

            txtName.Text = SelectedPublicAlbum.Name;
            txtDescription.Text = SelectedPublicAlbum.Description;

            rptImages.DataSource = SelectedPublicAlbum.Images;
            rptImages.DataBind();

            LoadSilverlightControl(ltrInitParams, Business.AlbumTypeEnum.Public, SelectedPublicAlbum.Id, Guid.Empty);
        }

        protected void rptImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType != ListItemType.Item) && (e.Item.ItemType != ListItemType.AlternatingItem))
                return;

            RadioButton rdo = (RadioButton)e.Item.FindControl("rdoThumbnail");
            string script = "SetUniqueRadioButton('rptImages.*PublicAlbum',this)";
            rdo.Attributes.Add("onclick", script);

            int imageCount = SelectedPublicAlbum.Images.Length;

            if (imageCount > 0)
            {
                int rowCount = Convert.ToInt32((Convert.ToDouble(imageCount) / ThumbnailImageColumnCount) + 0.49);
                pnlImages.Style["height"] = (rowCount * 290).ToString() + "px";
            }
        }

        protected void btnDeleteAlbum_Click(object sender, EventArgs e)
        {
            if (SelectedPublicAlbum.IsInDatabase)
            {

                foreach (DbObjects.Business.PublicImage image in SelectedPublicAlbum.Images)
                    DeletePublicImage(image);

                SelectedPublicAlbum.Delete();
                SelectedPublicAlbum = null;
            }

            Response.Redirect("PublicAlbums.aspx");
        }

        private void DeletePublicImage(DbObjects.Business.PublicImage image)
        {
            string absolutePath = Server.MapPath(image.ImageUrl);

            //delete physical image file
            if (File.Exists(absolutePath))
                File.Delete(absolutePath);

            //remove image from database
            SelectedPublicAlbum.RemoveImage(image.Id);
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

            if (!(txtName.Text == SelectedPublicAlbum.Name))
            {
                if (DbObjects.Business.PublicAlbum.PublicAlbumNameExists(txtName.Text))
                {
                    lblError.Text = "There is already another album with that name";
                    mpeError.Show();
                    return;
                }
            }

            DbObjects.Business.PublicAlbum album = SelectedPublicAlbum;

            album.Name = txtName.Text;
            album.Description = txtDescription.Text;
            album.Save();

            Int16 thumbnailId = album.Thumbnail.Id;

            for (int i = 0; i < rptImages.Items.Count; i++)
            {
                RadioButton rdoThumbnail = (RadioButton)rptImages.Items[i].FindControl("rdoThumbnail");
                Int16 imageId = Convert.ToInt16(rdoThumbnail.Attributes["publicImageId"]);

                if (rdoThumbnail.Checked)
                    thumbnailId = imageId;

                TextBox txtCaption = (TextBox)rptImages.Items[i].FindControl("txtCaption");

                DbObjects.Business.PublicImage image = album.GetPublicImageById(imageId);

                if (image.Caption != txtCaption.Text)
                    album.ChangeImageCaption(imageId, txtCaption.Text);
            }

            if (album.Thumbnail.Id != thumbnailId)
                album.SetThumbnailImage(thumbnailId);

            BindAlbum();
        }

        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {
            //get id number
            Button btnDeleteImage = (Button)sender;
            Int16 publicImageId = Convert.ToInt16(btnDeleteImage.CommandArgument);

            DeletePublicImage(SelectedPublicAlbum.GetPublicImageById(publicImageId));

            //reload
            BindAlbum();
        }
    }
}

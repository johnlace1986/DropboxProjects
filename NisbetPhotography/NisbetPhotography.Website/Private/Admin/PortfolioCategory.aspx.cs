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
    public partial class PortfolioCategory : Business.BasePage
    {
        protected String PortfolioCategoryFolderUrl = "../../Images/ContentImages/Portfolio/";

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (SelectedPortfolioCategory == null)
                SelectedPortfolioCategory = new DbObjects.Business.PortfolioCategory();

            if (!(IsPostBack))
                BindCategory();

            LoadSilverlightControl(ltrInitParams, Business.AlbumTypeEnum.Portfolio, SelectedPortfolioCategory.Id, Guid.Empty);
        }

        private void BindCategory()
        {
            if (SelectedPortfolioCategory.IsInDatabase)
                SelectedPortfolioCategory = new DbObjects.Business.PortfolioCategory(SelectedPortfolioCategory.Id);

            if (String.IsNullOrEmpty(SelectedPortfolioCategory.Name))
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"Portfolio.aspx\">Portfolio</a> >> New Category";
            else
                Master.ThumbnailLabel.Text = "Admin >> <a href=\"Portfolio.aspx\">Portfolio</a> >> " + SelectedPortfolioCategory.Name;

            txtName.Text = SelectedPortfolioCategory.Name;

            rptImages.DataSource = SelectedPortfolioCategory.Images;
            rptImages.DataBind();

            LoadSilverlightControl(ltrInitParams, Business.AlbumTypeEnum.Portfolio, SelectedPortfolioCategory.Id, Guid.Empty);
        }

        protected void rptImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType != ListItemType.Item) && (e.Item.ItemType != ListItemType.AlternatingItem))
                return;

            RadioButton rdo = (RadioButton)e.Item.FindControl("rdoThumbnail");
            string script = "SetUniqueRadioButton('rptImages.*Category',this)";
            rdo.Attributes.Add("onclick", script);

            int imageCount = SelectedPortfolioCategory.Images.Length;

            if (imageCount > 0)
            {
                int rowCount = Convert.ToInt32((Convert.ToDouble(imageCount) / ThumbnailImageColumnCount) + 0.49);                
                pnlImages.Style["height"] = (rowCount * 248).ToString() + "px";
            }
        }

        protected void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (SelectedPortfolioCategory.IsInDatabase)
            {
                foreach (DbObjects.Business.PortfolioImage image in SelectedPortfolioCategory.Images)
                    DeletePortfolioImage(image);

                SelectedPortfolioCategory.Delete();
                SelectedPortfolioCategory = null;
            }

            Response.Redirect("Portfolio.aspx");
        }

        private void DeletePortfolioImage(DbObjects.Business.PortfolioImage image)
        {
            string absolutePath = Server.MapPath(image.ImageUrl);

            //delete physical image file
            if (File.Exists(absolutePath))
                File.Delete(absolutePath);

            //remove image from database
            SelectedPortfolioCategory.RemoveImage(image.Id);
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtName.Text))
            {
                lblError.Text = "Please give the category a name";
                mpeError.Show();
                return;
            }

            if (!(txtName.Text == SelectedPortfolioCategory.Name))
            {
                if (DbObjects.Business.PortfolioCategory.PortfolioCategoryNameExists(txtName.Text))
                {
                    lblError.Text = "There is already another category with that name";
                    mpeError.Show();
                    return;
                }
            }

            SelectedPortfolioCategory.Name = txtName.Text;
            SelectedPortfolioCategory.Save();

            for (int i = 0; i < rptImages.Items.Count; i++)
            {
                RadioButton rdoThumbnail = (RadioButton)rptImages.Items[i].FindControl("rdoThumbnail");

                if (rdoThumbnail.Checked)
                {
                    SelectedPortfolioCategory.SetThumbnailImage(Convert.ToInt16(rdoThumbnail.Attributes["portfolioImageId"]));
                    break;
                }
            }

            BindCategory();
        }

        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {
            //get id number
            Button btnDeleteImage = (Button)sender;
            Int16 portfolioImageId = Convert.ToInt16(btnDeleteImage.CommandArgument);

            DeletePortfolioImage(SelectedPortfolioCategory.GetPortfolioImageById(portfolioImageId));

            //reload
            BindCategory();
        }
    }
}

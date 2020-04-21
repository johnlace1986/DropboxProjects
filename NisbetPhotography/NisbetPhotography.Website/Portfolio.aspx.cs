using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace NisbetPhotography.Website
{
    public partial class Portfolio : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (!(IsPostBack))
                PortfolioCategories = DbObjects.Business.PortfolioCategory.GetPortfolioCategories();

            BindCategories();

            if (PortfolioCategories.Length > 0)
            {
                if (SelectedPortfolioCategory == null)
                    BindGallery(PortfolioCategories[0]);
                else
                    BindGallery(SelectedPortfolioCategory);


                pnlSelectedCategory.Visible = true;
            }
            else
            {
                pnlSelectedCategory.Visible = false;
            }
        }

        protected void lnkCategory_Click(object sender, EventArgs e)
        {
            LinkButton lnkCategory = (LinkButton)sender;
            BindGallery(new DbObjects.Business.PortfolioCategory(Convert.ToInt16(lnkCategory.CommandArgument)));
        }

        private void BindCategories()
        {
            rptCategories.DataSource = PortfolioCategories;
            rptCategories.DataBind();
        }

        protected void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem ri = e.Item;

            Image imgCategory = (Image)ri.FindControl("imgCategory");

            if (String.IsNullOrEmpty(imgCategory.ImageUrl))
                imgCategory.ImageUrl = "Images/no_thumbnail_image.png";
        }

        private void BindGallery(DbObjects.Business.PortfolioCategory category)
        {
            lblAlbumTitle.Text = category.Name;

            rptGallery.DataSource = category.Images;
            rptGallery.DataBind();
        }
    }
}

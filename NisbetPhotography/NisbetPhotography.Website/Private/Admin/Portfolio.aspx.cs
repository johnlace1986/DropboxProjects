using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class Portfolio : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            Master.ThumbnailLabel.Text = "Admin >> Portfolio";

            if (!(IsPostBack))
            {
                PortfolioCategories = DbObjects.Business.PortfolioCategory.GetPortfolioCategories();
                BindCategories();
            }
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
                imgCategory.ImageUrl = "../../Images/no_thumbnail_image.png";

            int imageCount = PortfolioCategories.Length;

            if (imageCount > 0)
            {
                int rowCount = Convert.ToInt32((Convert.ToDouble(imageCount) / ThumbnailImageColumnCount) + 0.49);
                pnlCategories.Style["height"] = (rowCount * 200).ToString() + "px";
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            SelectedPortfolioCategory = new DbObjects.Business.PortfolioCategory();
            Response.Redirect("PortfolioCategory.aspx");
        }
    }
}

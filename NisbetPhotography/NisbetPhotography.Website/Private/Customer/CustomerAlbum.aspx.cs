using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Private.Customer
{
    public partial class CustomerAlbum : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (SelectedCustomerAlbum == null)
                Response.Redirect("Default.aspx");

            if (!(IsPostBack))
                BindAlbum();
        }

        private void BindAlbum()
        {
            DbObjects.Business.CustomerAlbum album = SelectedCustomerAlbum;
            lblName.Text = album.Name;
            lblDateCreated.Text = album.DateCreated.ToString("dddd d MMMM yyyy");

            rptImages.DataSource = album.Images;
            rptImages.DataBind();
        }
    }
}
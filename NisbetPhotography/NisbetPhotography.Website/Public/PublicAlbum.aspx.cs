using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Public
{
    public partial class PublicAlbum : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "../ContentErrorPage.aspx";

            if (SelectedPublicAlbum == null)
                Response.Redirect("Default.aspx");

            if (!(IsPostBack))
                BindAlbum();
        }

        private void BindAlbum()
        {
            lblAlbumTitle.Text = SelectedPublicAlbum.Name;

            rptGallery.DataSource = SelectedPublicAlbum.Images;
            rptGallery.DataBind();
        }
    }
}

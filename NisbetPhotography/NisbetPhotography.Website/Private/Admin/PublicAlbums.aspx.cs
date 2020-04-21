using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class PublicAlbums : Business.BasePage
    {
        protected String PublicImageFolderUrl = "../../Public/";

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            Master.ThumbnailLabel.Text = "Admin >> Public Albums";

            if (!(IsPostBack))
            {
                PublicAlbums = DbObjects.Business.PublicAlbum.GetPublicAlbums();
                BindAlbums();
            }
        }

        private void BindAlbums()
        {
            rptPublicAlbums.DataSource = PublicAlbums;
            rptPublicAlbums.DataBind();
        }

        protected void rptPublicAlbums_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem ri = e.Item;

            Image imgPublicAlbum = (Image)ri.FindControl("imgPublicAlbum");

            if (imgPublicAlbum.ImageUrl == PublicImageFolderUrl)
                imgPublicAlbum.ImageUrl = "../../Images/no_thumbnail_image.png";

            int imageCount = PublicAlbums.Length;

            if (imageCount > 0)
            {
                int rowCount = Convert.ToInt32((Convert.ToDouble(imageCount) / ThumbnailImageColumnCount) + 0.49);
                pnlPublicAlbums.Style["height"] = (rowCount * 200).ToString() + "px";
            }
        }

        protected void lnkPublicAlbum_Click(object sender, EventArgs e)
        {
            LinkButton lnkPublicAlbum = (LinkButton)sender;
            Int16 publicAlbumId = Convert.ToInt16(lnkPublicAlbum.CommandArgument);

            SelectedPublicAlbum = new DbObjects.Business.PublicAlbum(publicAlbumId);
            Response.Redirect("PublicAlbum.aspx");
        }

        protected void btnAddAlbum_Click(object sender, EventArgs e)
        {
            SelectedPublicAlbum = new DbObjects.Business.PublicAlbum();
            Response.Redirect("PublicAlbum.aspx");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Public
{
    public partial class Default : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "../ContentErrorPage.aspx";

            if (!(IsPostBack))
            {
                PublicAlbums = DbObjects.Business.PublicAlbum.GetPublicAlbums();
                BindAlbums();
            }
        }

        private void BindAlbums()
        {
            dgPublicAlbums.DataSource = PublicAlbums;
            dgPublicAlbums.DataBind();
        }

        protected void dgPublicAlbums_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindAlbums();

            dgPublicAlbums.PageIndex = e.NewPageIndex;
            dgPublicAlbums.DataBind();
        }

        protected void dgPublicAlbums_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "dgPublicAlbums_RowClicked")
            {
                //get error index
                int offset = dgPublicAlbums.PageIndex * dgPublicAlbums.PageSize;
                int index = offset + Convert.ToInt32(e.CommandArgument);

                SelectedPublicAlbum = PublicAlbums[index];
                Response.Redirect("PublicAlbum.aspx");
            }
        }
    }
}

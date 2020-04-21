using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace NisbetPhotography.Website.Private.Customer
{
    public partial class Customer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbObjects.Business.User currentUser = Business.BasePage.CurrentUser;

            if (currentUser == null)
                Response.Redirect("../../");

            if (currentUser.Admin)
                Response.Redirect("../Admin");

            if (!(IsPostBack))
                BindAlbums();
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("../");
        }
        
        protected void lnkAlbum_Click(object sender, EventArgs e)
        {
            LinkButton lnkAlbum = (LinkButton)sender;
            Int16 customerAlbumId = Convert.ToInt16(lnkAlbum.CommandArgument);

            Business.BasePage.SelectedCustomerAlbum = Business.BasePage.CurrentUser.GetCustomerAlbumById(customerAlbumId);
            Response.Redirect("CustomerAlbum.aspx");
        }

        private void BindAlbums()
        {
            DbObjects.Business.User currentUser = Business.BasePage.CurrentUser;

            rptAlbums.DataSource = currentUser.CustomerAlbums;
            rptAlbums.DataBind();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        public Label ThumbnailLabel
        {
            get { return lblThumbnail; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DbObjects.Business.User currentUser = Business.BasePage.CurrentUser;

            if (currentUser == null)
                Response.Redirect("../../");

            if (!(currentUser.Admin))
                Response.Redirect("../Customer");
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("../");
        }
    }
}

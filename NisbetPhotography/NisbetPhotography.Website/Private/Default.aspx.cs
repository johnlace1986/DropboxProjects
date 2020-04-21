using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace NisbetPhotography.Website.Private
{
    public partial class Default : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "../ContentErrorPage.aspx";

            if (CurrentUser != null)
                Redirect(CurrentUser);
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (DbObjects.Business.User.EmailAddressExists(txtEmailAddress.Text))
            {
                DbObjects.Business.User user = new DbObjects.Business.User(txtEmailAddress.Text);

                if (user.Password == txtPassword.Text)
                {
                    CurrentUser = user;
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);

                    Redirect(user);
                }
                else
                {
                    lblError.Text = "Incorrect password";
                    return;
                }
            }
            else
            {
                lblError.Text = "No account exists for that email address";
                return;
            }
        }

        private void Redirect(DbObjects.Business.User user)
        {
            if (String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                if (user.Admin)
                    Response.Redirect("Admin");
                else
                    Response.Redirect("Customer");
            else
                Response.Redirect(Request.QueryString["ReturnUrl"]);
        }
    }
}

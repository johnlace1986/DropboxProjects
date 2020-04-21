using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class Error : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            if (SelectedError == null)
                Response.Redirect("Errors.aspx");

            Master.ThumbnailLabel.Text = "Admin >> <a href=\"Errors.aspx\">Errors</a> >> " + SelectedError.Name;

            if (!(IsPostBack))
                BindError();
        }

        private void BindError()
        {
            DbObjects.Business.Error error = SelectedError;

            lblName.Text = error.Name;
            lblMessage.Text = error.Message;
            lblStackTrace.Text = error.StrackTrace.Replace("<br />", "</li><li>");

            List<DbObjects.Business.Error> innerErrors = new List<DbObjects.Business.Error>();
            while (error.InnerError != null)
            {
                error = error.InnerError;
                innerErrors.Add(error);
            }

            rptInnerErrors.DataSource = innerErrors;
            rptInnerErrors.DataBind();
        }

        protected void btnDeleteError_Click(object sender, EventArgs e)
        {
            SelectedError.Delete();
            Response.Redirect("Errors.aspx");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class Errors : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            Master.ThumbnailLabel.Text = "Admin >> Errors";

            if (!(IsPostBack))
            {
                Errors = DbObjects.Business.Error.GetErrors();
                BindErrors();
            }
        }

        private void BindErrors()
        {
            dgError.DataSource = Errors;
            dgError.DataBind();
        }

        protected void dgError_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindErrors();

            dgError.PageIndex = e.NewPageIndex;
            dgError.DataBind();
        }

        protected void dgError_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "dgError_RowClicked")
            {
                //get error index
                int offset = dgError.PageIndex * dgError.PageSize;
                int index = offset + Convert.ToInt32(e.CommandArgument);

                SelectedError = Errors[index];
                Response.Redirect("Error.aspx");
            }
        }
    }
}

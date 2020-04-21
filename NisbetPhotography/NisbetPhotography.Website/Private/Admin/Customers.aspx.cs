using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website.Private.Admin
{
    public partial class Customers : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            Master.ThumbnailLabel.Text = "Admin >> Customers";

            if (!(IsPostBack))
            {
                Customers = DbObjects.Business.User.GetUsers().Where(p => p.Admin == false).ToArray<DbObjects.Business.User>();
                BindCustomers();
            }
        }

        private void BindCustomers()
        {
            dgCustomer.DataSource = Customers;
            dgCustomer.DataBind();
        }

        protected void dgCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindCustomers();

            dgCustomer.PageIndex = e.NewPageIndex;
            dgCustomer.DataBind();
        }

        protected void dgCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "dgCustomer_RowClicked")
            {
                //get error index
                int offset = dgCustomer.PageIndex * dgCustomer.PageSize;
                int index = offset + Convert.ToInt32(e.CommandArgument);

                SelectedCustomer = Customers[index];
                Response.Redirect("Customer.aspx");
            }
        }

        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            SelectedCustomer = new DbObjects.Business.User();
            Response.Redirect("Customer.aspx");
        }
    }
}

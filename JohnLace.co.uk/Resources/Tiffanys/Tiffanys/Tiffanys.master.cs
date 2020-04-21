using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tiffanys_Tiffanys : System.Web.UI.MasterPage
{
    public NavItem NavItem { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected string GetNavItemCssClass(NavItem navItem)
    {
        if (navItem == NavItem)
            return "nav_item_selected";
        else
            return "nav_item";
    }
}

public enum NavItem
{
    Home,
    GreetingsCards,
    ThorntonsChocolates,
    HeliumBalloons,
    FindUs
}

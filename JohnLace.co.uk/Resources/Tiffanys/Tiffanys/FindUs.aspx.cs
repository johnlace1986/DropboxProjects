﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tiffanys_FindUs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.NavItem = NavItem.FindUs;
    }
}

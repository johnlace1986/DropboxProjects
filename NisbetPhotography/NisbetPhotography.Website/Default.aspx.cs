using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website
{
    public partial class Default : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            List<String> images = new List<string>();

            images.Add("images/ContentImages/Default/001.JPG");
            images.Add("images/ContentImages/Default/002.JPG");
            images.Add("images/ContentImages/Default/003.JPG");
            images.Add("images/ContentImages/Default/004.JPG");
            images.Add("images/ContentImages/Default/005.JPG");
            images.Add("images/ContentImages/Default/006.JPG");

            Master.SliderImages = images.ToArray();
        }
    }
}

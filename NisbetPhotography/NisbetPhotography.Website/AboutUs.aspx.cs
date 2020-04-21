using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website
{
    public partial class AboutUs : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";

            List<String> images = new List<string>();

            images.Add("images/ContentImages/AboutUs/001.jpg");
            images.Add("images/ContentImages/AboutUs/002.jpg");
            images.Add("images/ContentImages/AboutUs/003.jpg");
            images.Add("images/ContentImages/AboutUs/004.jpg");
            images.Add("images/ContentImages/AboutUs/005.jpg");
            images.Add("images/ContentImages/AboutUs/006.jpg");

            Master.SliderImages = images.ToArray();
        }
    }
}

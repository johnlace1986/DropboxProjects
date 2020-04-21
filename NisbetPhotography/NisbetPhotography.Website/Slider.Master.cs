using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NisbetPhotography.Website
{
    public partial class Slider : System.Web.UI.MasterPage
    {
        public String[] SliderImages;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                rptSlider.DataSource = SliderImages;
                rptSlider.DataBind();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NisbetPhotography.DbObjects.Business
{
    public static class Global
    {
        /// <summary>
        /// Makes a string with HTML safe for outputing in a web page
        /// </summary>
        /// <param name="html">String containing html tags</param>
        /// <returns>String that can be displayed in a web page</returns>
        public static String RemoveHtmlTags(String html)
        {
            if (String.IsNullOrEmpty(html))
                return "";

            html = html.Replace("<", "&lt");
            html = html.Replace(">", "&gt");
            html = html.Replace("\r\n", "<br />");

            return html;
        }
    }
}

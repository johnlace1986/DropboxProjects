using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Threading;
using System.Drawing;

namespace NisbetPhotography.Website.Private.Admin
{
    /// <summary>
    /// Summary description for ImagesHandler
    /// </summary>
    public class ImagesHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //process request
            string filename = context.Request.QueryString["filename"].ToString();

            using (Stream s = context.Request.InputStream)
            {
                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);

                byte[] data = Convert.FromBase64String(Convert.ToBase64String(buffer));

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(data, 0, data.Length);

                    using (Image img = Image.FromStream(ms))
                    {
                        img.Save(context.Server.MapPath(filename), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
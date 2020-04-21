using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using Wedding.eVite.Web.Controllers.ActionResults.JsonConverters;

namespace Wedding.eVite.Web.Controllers.ActionConverters
{
    public class JsonDateTimeResult : JsonResult
    {
        #region JsonResult Members

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var jsonDateTimeConverter = new JsonDateTimeConverter();
                response.Write(JsonConvert.SerializeObject(Data, jsonDateTimeConverter));
            }
        }

        #endregion
    }
}
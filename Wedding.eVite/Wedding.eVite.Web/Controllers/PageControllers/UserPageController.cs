using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wedding.eVite.Web.Controllers.PageControllers
{
    public abstract class UserPageController : PageController
    {
        #region Static Properties

        /// <summary>
        /// Gets or sets a value determining whether or not the images in the shared layout have been loaded
        /// </summary>
        public static Boolean LayoutImagesLoaded
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["LayoutImagesLoaded"] == null)
                    System.Web.HttpContext.Current.Session["LayoutImagesLoaded"] = false;

                return (Boolean)System.Web.HttpContext.Current.Session["LayoutImagesLoaded"];
            }
            set
            {
                System.Web.HttpContext.Current.Session["LayoutImagesLoaded"] = value;
            }
        }

        #endregion

        #region PageController Members

        public override Boolean IsAdminPageController
        {
            get { return false; }
        }
        
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (LayoutImagesLoaded)
                ViewBag.ShouldLoadLayoutImages = false;
            else
            {
                ViewBag.ShouldLoadLayoutImages = true;
                LayoutImagesLoaded = true;
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion

        #region Actions

        public ActionResult ReturnAdminWebsite()
        {
            OverrideAdmin = false;
            LayoutImagesLoaded = false;

            return Redirect("~/Admin");
        }

        #endregion
    }
}
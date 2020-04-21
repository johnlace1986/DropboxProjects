using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Wedding.eVite.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new { controller = "Default", action = "Index" }
            );

            routes.MapRoute(
                "AdminInvite",
                "Admin/Invite/{inviteId}",
                new { controller = "Admin", action = "Invite" }
            );

            routes.MapRoute(
                "AdminGuests",
                "Admin/Guests/{rsvp}",
                new { controller = "Admin", action = "Guests" }
            );

            routes.MapRoute(
                "AdminMessages",
                "Admin/Messages/{inviteId}",
                new { controller = "Admin", action = "Messages" }
            );

            routes.MapRoute(
                "Unsubscribe",
                "Unsubscribe/Index/{inviteId}",
                new { controller = "Unsubscribe", action = "Index" }
            );
        }
    }
}
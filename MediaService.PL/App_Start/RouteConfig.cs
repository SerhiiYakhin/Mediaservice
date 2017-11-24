#region usings

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace MediaService.PL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Account", action = "Login", id = UrlParameter.Optional}
            );
            routes.MapRoute(
    "Home",
    "{controller}/{action}/{id}",
    new { controller = "Home", action = "Index", dirId = UrlParameter.Optional }
);
        }
    }
}
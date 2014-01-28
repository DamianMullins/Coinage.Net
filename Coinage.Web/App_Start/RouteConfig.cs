using System.Web.Mvc;
using System.Web.Routing;

namespace Coinage.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Products",
                url: "product/{id}",
                defaults: new { controller = "Product", action = "Index" },
                namespaces: new[] { "Coinage.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Coinage.Web.Controllers" }
            );
        }
    }
}

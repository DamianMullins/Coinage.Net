using System.Web.Mvc;

namespace Coinage.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Admin_default", 
                url: "admin/{controller}/{action}/{id}", 
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Coinage.Web.Areas.Admin.Controllers" }
            );
        }
    }
}
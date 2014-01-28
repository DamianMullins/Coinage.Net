using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

using Coinage.Web.Models.Product;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class ProductController : Controller
    {
        public ViewResult Index(int id)
        {
            var product = new Product { ProductId = 1, Name = "First Product" };
            return View(product);
        }
    }
}

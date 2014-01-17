using Coinage.Web.Models.Product;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index(int id)
        {
            var products = new List<Product> 
            {
                new Product { ProductId = 1, Name = "First Product" }, 
                new Product { ProductId = 2, Name = "Second Product" }
            };

            var product = products.FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                return View(product);
            }
            return HttpNotFound("Product was not found.");
        }
    }
}

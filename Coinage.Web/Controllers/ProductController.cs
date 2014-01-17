using Coinage.Web.Models.Product;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class ProductController : Controller
    {
        private List<Product> _products;

        public ProductController()
        {
            
        }

        public ProductController(List<Product> products)
        {
            _products = products;
        }

        public ActionResult Index(int id)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                return View(product);
            }
            return HttpNotFound("Product was not found.");
        }

        public ActionResult List()
        {
            return View(_products);
        }
    }
}

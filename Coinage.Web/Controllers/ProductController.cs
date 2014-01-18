using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index(int id)
        {
            Product product = _productService.GetProduct(id);

            if (product != null)
            {
                return View(product);
            }
            return HttpNotFound("Product was not found.");
        }

        public ActionResult List()
        {
            var products = _productService.GetProducts();
            return View(products);
        }

        [ChildActionOnly]
        public ActionResult FeaturedList()
        {
            var products = _productService.GetFeaturedProducts();
            return View(products);
        }
    }
}

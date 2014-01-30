using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Web.Models.Product;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public ProductController(IProductService productService, IBasketService basketService)
        {
            _productService = productService;
            _basketService = basketService;
        }

        public ActionResult Index(int id)
        {
            Product product = _productService.GetProductById(id);

            if (product != null)
            {
                var model = new ProductDetailsModel
                {
                    Name = product.Name, 
                    Description = product.Description,
                    ProductPrice = new ProductDetailsModel.ProductPriceModel
                    {
                        ProductId = product.Id, 
                        PriceValue = product.Price
                    },
                    AddToBasket = new ProductDetailsModel.AddToBasketModel
                    {
                        ProductId = product.Id
                    }
                };
                return View(model);
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

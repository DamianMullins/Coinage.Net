using System.Threading.Tasks;
using Coinage.Domain.Entites;
using Coinage.Domain.Services;
using Coinage.Web.Models.Products;
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

        public async Task<ActionResult> Index(int id)
        {
            Product product = await _productService.GetProductByIdAsync(id);

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

        public async Task<ViewResult> List()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        [ChildActionOnly]
        public ActionResult Featured()
        {
            var model = new FeaturedProductsModel { Products = _productService.GetFeaturedProducts() };
            return View(model);
        }

        [ChildActionOnly]
        public ViewResult Latest(int count)
        {
            var model = new LatestProductModel { Products = _productService.GetLatestProducts(count) };
            return View(model);
        }
    }
}

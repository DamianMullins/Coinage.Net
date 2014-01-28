using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete;
using Coinage.Domain.Concrete.Entities;
using Coinage.Web.Models.Basket;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public BasketController(IProductService productService, IBasketService basketService)
        {
            _productService = productService;
            _basketService = basketService;
        }

        public ActionResult Index()
        {
            var model = new BasketModel();
            Basket basket = _basketService.GetBasket(2);

            if (basket != null)
            {
                // TODO: Add basket model properties

                foreach (BasketItem item in basket.BasketItems)
                {
                    var basketItem = new BasketModel.BasketItemModel
                    {
                        Id = item.ProductId,
                        ProductId = item.Product.Id,
                        ItemName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = "", // String formatted individual price
                        SubTotal = "" // String formatted price * quantity
                    };
                    model.Items.Add(basketItem);
                }
            }
            return View(model);
        }

        public ActionResult AddToBasket(int productId, int quantity)
        {
            Basket basket = _basketService.GetBasket(2);
            Product product = _productService.GetProductById(productId);
            EntityActionResponse response = _basketService.AddToCart(basket, product, quantity);

            if (response.Successful)
            {
                SuccessAlert(string.Format("{0} was added to your basket", product.Name));
            }
            else
            {
                ErrorAlert("was added to your basket");
            }
            return RedirectToRoute("Basket");
        }
	}
}

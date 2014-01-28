using System.Data.Entity.Core.Mapping;
using System.Linq;
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
                model.Total = basket.TotalAmount.ToString("0.00");

                foreach (BasketItem item in basket.BasketItems)
                {
                    var basketItem = new BasketModel.BasketItemModel
                    {
                        Id = item.Id,
                        ProductId = item.Product.Id,
                        ItemName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price.ToString("0.00"), // String formatted individual price
                        SubTotal = item.SubTotal.ToString("0.00") // String formatted price * quantity
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
            EntityActionResponse response = _basketService.AddProductToBasket(basket, product, quantity);

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

        [HttpPost]
        public ActionResult UpdateBasketItems(BasketModel model)
        {
            foreach (var item in model.Items)
            {
                // TODO: handle response
                var response = _basketService.UpdateProductInBasket(item.Id, item.ProductId, item.Quantity);
            }
            return RedirectToAction("Index");
        }
    }
}

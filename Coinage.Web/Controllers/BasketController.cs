﻿using Coinage.Domain.Entites;
using Coinage.Domain.Models;
using Coinage.Domain.Services;
using Coinage.Web.Models.Baskets;
using Coinage.Web.Models.Products;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        readonly HttpContextBase _httpContext;

        public BasketController(
            IProductService productService,
            IBasketService basketService,
            HttpContextBase httpContext)
        {
            _productService = productService;
            _basketService = basketService;
            _httpContext = httpContext;
        }

        public async Task<ViewResult> Index()
        {
            var model = new BasketModel();
            Basket basket = await _basketService.GetCustomerBasketAsync();

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

        [HttpPost]
        public async Task<ActionResult> AddToBasket(ProductDetailsModel.AddToBasketModel model)
        {
            // TODO test speeds when using Task.WhenAll()
            Basket basket = await _basketService.GetCustomerBasketAsync();
            Product product = await _productService.GetProductByIdAsync(model.ProductId);
            EntityActionResponse response = await _basketService.AddProductToBasketAsync(basket, product, model.Quantity);

            if (response.Success)
            {
                SuccessAlert(string.Format("{0} was added to your basket", product.Name));
            }
            else
            {
                ErrorAlert("There was an error adding the item to your basket");
                if (_httpContext.Request.UrlReferrer != null)
                {
                    return new RedirectResult(_httpContext.Request.UrlReferrer.AbsoluteUri);
                }
            }
            return RedirectToRoute("Basket");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBasketItem(BasketModel.BasketItemModel model)
        {
            EntityActionResponse response = await _basketService.UpdateProductInBasketAsync(model.Id, model.ProductId, model.Quantity);

            if (response.Success)
            {
                SuccessAlert("Basket was updated");
            }
            else
            {
                ErrorAlert("There was an error updating your basket");
            }

            return RedirectToAction("Index");
        }
    }
}

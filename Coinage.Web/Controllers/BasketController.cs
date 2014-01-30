﻿using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete;
using Coinage.Domain.Concrete.Entities;
using Coinage.Web.Models;
using Coinage.Web.Models.Basket;
using System.Web;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        readonly HttpContextBase _httpContext;

        public BasketController(IProductService productService, IBasketService basketService, HttpContextBase httpContext)
        {
            _productService = productService;
            _basketService = basketService;
            _httpContext = httpContext;
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

        [HttpPost]
        public ActionResult AddToBasket(ProductDetailsModel.AddToBasketModel model)
        {
            Basket basket = _basketService.GetBasket(2);
            Product product = _productService.GetProductById(model.ProductId);
            EntityActionResponse response = _basketService.AddProductToBasket(basket, product, model.Quantity);

            if (response.Successful)
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
        public ActionResult UpdateBasketItems(BasketModel model)
        {
            foreach (var item in model.Items)
            {
                EntityActionResponse response = _basketService.UpdateProductInBasket(item.Id, item.ProductId, item.Quantity);

                if (response.Successful)
                {
                    SuccessAlert("Basket was updated");
                }
                else
                {
                    ErrorAlert("There was an error updating your basket");
                }
            }
            return RedirectToAction("Index");
        }
    }
}

﻿using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System.Web.Mvc;

namespace Coinage.Web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            Product product = _productService.GetProduct(id);

            if (product != null)
            {
                return View(product);
            }
            return HttpNotFound("Product was not found.");
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            _productService.Update(product);
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            _productService.Create(product);
            return RedirectToAction("Edit", new { id = product.ProductId });
        }
    }
}
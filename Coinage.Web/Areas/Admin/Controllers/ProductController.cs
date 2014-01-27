﻿using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete;
using Coinage.Domain.Concrete.Entities;
using System.Collections.Generic;
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
            List<Product> products = _productService.GetProducts();
            return View(products);
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
            if (ModelState.IsValid)
            {
                EntityActionResponse response = _productService.Update(product);

                if (response.Successful)
                {
                    TempData["alert-success"] = string.Format("{0} was updated successfully", product.Name);
                }

                TempData["alert-error"] = response.Exception.Message;
            }
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                EntityActionResponse response = _productService.Create(product);

                if (response.Successful)
                {
                    TempData["alert-success"] = string.Format("{0} was created successfully", product.Name);
                    return RedirectToAction("Edit", new { id = product.Id });
                }

                TempData["alert-error"] = response.Exception.Message;
            }
            return View(product);
        }
    }
}
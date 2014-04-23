using Coinage.Domain.Entites;
using Coinage.Domain.Models;
using Coinage.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Coinage.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ViewResult> Index()
        {
            IEnumerable<Product> products = await _productService.GetProductsAsync();
            return View(products);
        }

        public async Task<ActionResult> Edit(int id)
        {
            Product product = await _productService.GetProductByIdAsync(id);

            if (product != null)
            {
                return View(product);
            }
            return HttpNotFound("Product was not found.");
        }

        [HttpPost]
        public async Task<ViewResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                EntityActionResponse response = await _productService.UpdateAsync(product);

                if (response.Success)
                {
                    SuccessAlert(string.Format("{0} was updated successfully", product.Name));
                }
                else
                {
                    ErrorAlert(response.Exception.Message);
                }
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

                if (response.Success)
                {
                    SuccessAlert(string.Format("{0} was created successfully", product.Name));
                    return RedirectToAction("Edit", new { id = product.Id });
                }
                ErrorAlert(response.Exception.Message);
            }
            return View(product);
        }
    }
}
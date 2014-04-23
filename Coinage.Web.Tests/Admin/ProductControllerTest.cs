using Coinage.Domain.Entites;
using Coinage.Domain.Services;
using Coinage.Web.Areas.Admin.Controllers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Coinage.Web.Tests.Admin
{
    [TestFixture]
    public class ProductControllerTest
    {
        public class Index
        {
            
        }

        private class TestableProductController : ProductController
        {
            public readonly Mock<IProductService> ProductService;

            private TestableProductController(Mock<IProductService> productService)
                : base(productService.Object)
            {
                ProductService = productService;
            }

            public static TestableProductController Create()
            {
                return new TestableProductController(new Mock<IProductService>());
            }

            public void SetupProductServiceGetProducts(List<Product> products)
            {
                ProductService
                    .Setup(s => s.GetProducts())
                    .Returns(products);
            }

            public void SetupProductServiceGetProduct(Product product)
            {
                ProductService
                    .Setup(s => s.GetProductById(It.IsAny<int>()))
                    .Returns(product);
            }
        }
    }
}

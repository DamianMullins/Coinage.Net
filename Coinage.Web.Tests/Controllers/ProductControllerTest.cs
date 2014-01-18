using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Web.Controllers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Coinage.Web.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTest
    {
        public class Index
        {
            [Test]
            public void Index_RequestWithNonExistingProductId_ReturnsHttpNotFound()
            {
                // Arrange
                var controller = TestableProductController.Create();
                int nonExitingProductId = 3;

                // Act
                ActionResult result = controller.Index(nonExitingProductId);

                // Assert
                Assert.IsInstanceOf<HttpNotFoundResult>(result);
            }

            [Test]
            public void Index_RequestWithExistingProductId_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                var product = new Product { ProductId = 1, Name = "First Product" };
                controller.SetupProductServiceGetProduct(product);
                int existingProductId = 1;

                // Act
                var result = (ViewResult)controller.Index(existingProductId);

                // Assert
                var resultModel = ((Product)result.Model);
                Assert.IsInstanceOf<Product>(result.Model);
                Assert.AreEqual(existingProductId, resultModel.ProductId);
            }
        }

        public class List
        {
            [Test]
            public void List_RequestWithNoProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                controller.SetupProductServiceGetProducts(new List<Product>());

                // Act
                var result = (ViewResult)controller.List();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result.Model);
                Assert.IsTrue(((List<Product>)result.Model).Count == 0);
            }

            [Test]
            public void List_RequestWithProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                var products = new List<Product>
                {
                    new Product {ProductId = 1, Name = "First Product"},
                    new Product {ProductId = 2, Name = "Second Product"}
                };
                controller.SetupProductServiceGetProducts(products);

                // Act
                var result = (ViewResult)controller.List();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result.Model);
                Assert.IsTrue(((List<Product>)result.Model).Count == 2);
            }
        }

        public class FeaturedList
        {
            [Test]
            public void FeaturedList_RequestWithNoProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                controller.SetupProductServiceGetProducts(new List<Product>());

                // Act
                var result = (ViewResult)controller.List();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result.Model);
                Assert.IsTrue(((List<Product>)result.Model).Count == 0);
            }

            [Test]
            public void List_RequestWithProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                var products = new List<Product>
                {
                    new Product {ProductId = 1, Name = "First Product"},
                    new Product {ProductId = 2, Name = "Second Product"}
                };
                controller.SetupProductServiceGetProducts(products);

                // Act
                var result = (ViewResult)controller.List();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result.Model);
                Assert.IsTrue(((List<Product>)result.Model).Count == 2);
            }
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
                    .Setup(s => s.GetProduct(It.IsAny<int>()))
                    .Returns(product);
            }
        }
    }
}

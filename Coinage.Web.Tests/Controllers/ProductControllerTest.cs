using System;
using System.Linq;
using System.Threading.Tasks;
using Coinage.Domain.Entites;
using Coinage.Domain.Services;
using Coinage.Web.Controllers;
using Coinage.Web.Models.Products;
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
            public async Task Index_RequestWithNonExistingProductId_ReturnsHttpNotFound()
            {
                // Arrange
                var controller = TestableProductController.Create();
                int nonExitingProductId = 3;

                // Act
                var result = await controller.Index(nonExitingProductId);

                // Assert
                Assert.IsInstanceOf<HttpNotFoundResult>(result);
            }

            [Test]
            public async Task Index_RequestWithExistingProductId_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                var product = new Product { Id = 1, Name = "First Product" };
                controller.SetupProductServiceGetProductByIdAsync(product);
                int existingProductId = 1;

                // Act
                var result = await controller.Index(existingProductId) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ProductDetailsModel>(result.Model);
                var resultModel = ((ProductDetailsModel)result.Model);
                Assert.AreEqual("First Product", resultModel.Name);
            }
        }

        public class List
        {
            [Test]
            public async Task List_RequestWithNoProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                controller.SetupProductServiceGetProductsAsync(new List<Product>());

                // Act
                var result = await controller.List();

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<List<Product>>(result.Model);
                Assert.IsTrue(((List<Product>)result.Model).Count == 0);
            }

            [Test]
            public async Task List_RequestWithProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                var products = new List<Product>
                {
                    new Product {Id = 1, Name = "First Product"},
                    new Product {Id = 2, Name = "Second Product"}
                };
                controller.SetupProductServiceGetProductsAsync(products);

                // Act
                var result = await controller.List();

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<List<Product>>(result.Model);
                Assert.IsTrue(((List<Product>)result.Model).Count == 2);
            }
        }

        public class Featured
        {
            [Test]
            public void Featured_RequestWithNoProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                controller.SetupProductServiceGetFeaturedProducts(new List<Product>());

                // Act
                var result = controller.Featured() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<FeaturedProductsModel>(result.Model);
                Assert.IsTrue(((FeaturedProductsModel)result.Model).Products.ToList().Count == 0);
            }

            [Test]
            public void Featured_RequestWithProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                var products = new List<Product>
                {
                    new Product {Id = 1, Name = "First Product"},
                    new Product {Id = 2, Name = "Second Product"}
                };
                controller.SetupProductServiceGetFeaturedProducts(products);

                // Act
                var result = controller.Featured() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<FeaturedProductsModel>(result.Model);
                Assert.IsTrue(((FeaturedProductsModel)result.Model).Products.ToList().Count == 2);
                Assert.AreEqual(1, ((FeaturedProductsModel)result.Model).Products.ToList()[0].Id);
                Assert.AreEqual(2, ((FeaturedProductsModel)result.Model).Products.ToList()[1].Id);
            }
        }

        public class Latest
        {
            [Test]
            public void Latest_RequestWithNoProducts_ReturnsWithView()
            {
                // Arrange`
                int numberOfProducts = 1;
                var controller = TestableProductController.Create();
                controller.SetupProductServiceGetLatestProducts(new List<Product>());

                // Act
                var result = controller.Latest(numberOfProducts) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<LatestProductModel>(result.Model);
                Assert.IsTrue(((LatestProductModel)result.Model).Products.ToList().Count == 0);
            }

            [Test]
            public void Latest_RequestWithProducts_ReturnsWithView()
            {
                // Arrange
                int numberOfProducts = 1;
                var controller = TestableProductController.Create();
                var products = new List<Product>
                {
                    new Product {Id = 1, Name = "First Product"}
                };
                controller.SetupProductServiceGetLatestProducts(products);

                // Act
                var result = controller.Latest(numberOfProducts) as ViewResult;

                // Assert - can't test for correct number of products returned
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<LatestProductModel>(result.Model);
                Assert.IsTrue(((LatestProductModel)result.Model).Products.ToList().Count == 1);
                Assert.AreEqual(1, ((LatestProductModel)result.Model).Products.ToList()[0].Id);
            }
        }

        private class TestableProductController : ProductController
        {
            public readonly Mock<IProductService> ProductService;
            public readonly Mock<IBasketService> BasketService;

            private TestableProductController(Mock<IProductService> productService, Mock<IBasketService> basketService)
                : base(productService.Object, basketService.Object)
            {
                ProductService = productService;
                BasketService = basketService;
            }

            public static TestableProductController Create()
            {
                return new TestableProductController(new Mock<IProductService>(), new Mock<IBasketService>());
            }

            public void SetupProductServiceGetProductsAsync(IEnumerable<Product> products)
            {
                ProductService
                    .Setup(s => s.GetProductsAsync())
                    .Returns(Task.FromResult(products));
            }

            public void SetupProductServiceGetFeaturedProducts(IEnumerable<Product> products)
            {
                ProductService
                    .Setup(s => s.GetFeaturedProducts())
                    .Returns(products);
            }

            public void SetupProductServiceGetLatestProducts(IEnumerable<Product> products)
            {
                ProductService
                    .Setup(s => s.GetLatestProducts(It.IsAny<int>()))
                    .Returns(products);
            }

            public void SetupProductServiceGetProductByIdAsync(Product product)
            {
                ProductService
                    .Setup(s => s.GetProductByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult(product));
            }
        }
    }
}

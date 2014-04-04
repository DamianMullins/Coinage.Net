using System.Linq;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Web.Controllers;
using Coinage.Web.Models.Product;
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
                var product = new Product { Id = 1, Name = "First Product" };
                controller.SetupProductServiceGetProduct(product);
                int existingProductId = 1;

                // Act
                var result = controller.Index(existingProductId) as ViewResult;

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
            public void List_RequestWithNoProducts_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                controller.SetupProductServiceGetProducts(new List<Product>());

                // Act
                var result = controller.List() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
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
                    new Product {Id = 1, Name = "First Product"},
                    new Product {Id = 2, Name = "Second Product"}
                };
                controller.SetupProductServiceGetProducts(products);

                // Act
                ViewResult result = controller.List() as ViewResult;

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
                Assert.IsTrue(((FeaturedProductsModel)result.Model).Products.Count == 0);
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
                Assert.IsTrue(((FeaturedProductsModel)result.Model).Products.Count == 2);
                Assert.AreEqual(1, ((FeaturedProductsModel)result.Model).Products[0].Id);
                Assert.AreEqual(2, ((FeaturedProductsModel)result.Model).Products[1].Id);
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
                Assert.IsTrue(((LatestProductModel)result.Model).Products.Count == 0);
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
                Assert.IsTrue(((LatestProductModel)result.Model).Products.Count == 1);
                Assert.AreEqual(1, ((LatestProductModel)result.Model).Products[0].Id);
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

            public void SetupProductServiceGetProducts(List<Product> products)
            {
                ProductService
                    .Setup(s => s.GetProducts())
                    .Returns(products);
            }

            public void SetupProductServiceGetFeaturedProducts(List<Product> products)
            {
                ProductService
                    .Setup(s => s.GetFeaturedProducts())
                    .Returns(products);
            }

            public void SetupProductServiceGetLatestProducts(List<Product> products)
            {
                ProductService
                    .Setup(s => s.GetLatestProducts(It.IsAny<int>()))
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

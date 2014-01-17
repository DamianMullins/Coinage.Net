using Coinage.Web.Controllers;
using Coinage.Web.Models.Product;
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
                var controller = TestableProductController.Create(new List<Product>());

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

                // Act
                var result = (ViewResult)controller.List();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result.Model);
                Assert.IsTrue(((List<Product>)result.Model).Count == 2);
            }
        }

        private class TestableProductController : ProductController
        {
            public readonly List<Product> Products;

            private static readonly List<Product> _products = new List<Product>
            {
                new Product {ProductId = 1, Name = "First Product"},
                new Product {ProductId = 2, Name = "Second Product"}
            };

            private TestableProductController(List<Product> products)
                : base(products)
            {
                Products = products;
            }

            public static TestableProductController Create()
            {
                return new TestableProductController(_products);
            }

            public static TestableProductController Create(List<Product> products)
            {
                return new TestableProductController(products);
            }

            public void SetupClient()
            {
                //Client
                //    .Setup(c => c.Blah())
                //    .Returns(Blah);
            }
        }
    }
}

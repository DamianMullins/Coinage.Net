using System.Web.Mvc;
using Coinage.Web.Controllers;
using Coinage.Web.Models.Product;
using NUnit.Framework;

namespace Coinage.Web.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTest
    {
        public class Index
        {
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
        }

        private class TestableProductController : ProductController
        {
            //public readonly Mock<IClient> Client;

            private TestableProductController(/*Mock<IClient> client*/)
                : base(/*client.Object*/)
            {
                //Client = client;
            }

            public static TestableProductController Create()
            {
                return new TestableProductController(/*new Mock<IClient>()*/);
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

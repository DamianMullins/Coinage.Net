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
            public void Index_RequestWithId_ReturnsWithView()
            {
                // Arrange
                var controller = TestableProductController.Create();
                int productId = 1;

                // Act
                ViewResult result = controller.Index(productId);

                // Assert
                var resultModel = ((Product) result.Model);
                Assert.IsInstanceOf<Product>(result.Model);
                Assert.AreEqual(productId, resultModel.ProductId);
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

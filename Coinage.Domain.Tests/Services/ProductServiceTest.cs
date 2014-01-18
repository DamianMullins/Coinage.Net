using System.Runtime.InteropServices;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Services;
using Coinage.Domain.Data;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Coinage.Domain.Tests.Services
{
    [TestFixture]
    public class ProductServiceTest
    {
        public class GetProducts
        {
            [Test]
            public void GetProducts_NoProducts_ReturnsEmptyList()
            {
                // Arrange
                var service = TestableProductService.Create();

                // Act
                var result = service.GetProducts();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(0, result.Count);
            }

            [Test]
            public void GetProducts_ExistingProducts_ReturnsListOfProducts()
            {
                // Arrange
                var service = TestableProductService.Create();
                var products = new List<Product>
                {
                    new Product {ProductId = 1, Name = "First Product"},
                    new Product {ProductId = 2, Name = "Second Product"}
                };
                service.SetupRepoGetProducts(products);

                // Act
                var result = service.GetProducts();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(2, result.Count);
            }
        }
        public class GetFeaturedProducts
        {
            [Test]
            public void GetFeaturedProducts_NoProducts_ReturnsEmptyList()
            {
                // Arrange
                var service = TestableProductService.Create();

                // Act
                var result = service.GetFeaturedProducts();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(0, result.Count);
            }

            [Test]
            public void GetFeaturedProducts_ExistingProductsIncludingFeatured_ReturnsFilteredListOfProducts()
            {
                // Arrange
                var service = TestableProductService.Create();
                var products = new List<Product>
                {
                    new Product {ProductId = 1, Name = "First Product", IsFeatured = true},
                    new Product {ProductId = 2, Name = "Second Product", IsFeatured = false}
                };
                service.SetupRepoGetProducts(products);

                // Act
                var result = service.GetFeaturedProducts();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(1, result.Count);
            }

            [Test]
            public void GetFeaturedProducts_ExistingProductsAllFeatured_ReturnsListOfAllProducts()
            {
                // Arrange
                var service = TestableProductService.Create();
                var products = new List<Product>
                {
                    new Product {ProductId = 1, Name = "First Product", IsFeatured = true},
                    new Product {ProductId = 2, Name = "Second Product", IsFeatured = true}
                };
                service.SetupRepoGetProducts(products);

                // Act
                var result = service.GetFeaturedProducts();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(2, result.Count);
            }

            [Test]
            public void GetFeaturedProducts_ExistingProductsNoneFeatured_ReturnsEmptyList()
            {
                // Arrange
                var service = TestableProductService.Create();
                var products = new List<Product>
                {
                    new Product {ProductId = 1, Name = "First Product", IsFeatured = false},
                    new Product {ProductId = 2, Name = "Second Product", IsFeatured = false}
                };
                service.SetupRepoGetProducts(products);

                // Act
                var result = service.GetFeaturedProducts();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        public class GetProduct
        {
            [Test]
            public void GetProduct_NonExistentProductId_ReturnsNull()
            {
                // Arrange
                var service = TestableProductService.Create();
                int productId = 1;

                // Act
                var result = service.GetProduct(productId);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetProduct_ExistingProductId_ReturnsProduct()
            {
                // Arrange
                var service = TestableProductService.Create();
                var product = new Product { ProductId = 1 };
                service.SetupRepoGetById(product);

                // Act
                var result = service.GetProduct(product.ProductId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, product.ProductId);
            }
        }

        private class TestableProductService : ProductService
        {
            public readonly Mock<IRepository<Product>> ProductRepository;

            private TestableProductService(Mock<IRepository<Product>> productRepository)
                : base(productRepository.Object)
            {
                ProductRepository = productRepository;
            }

            public static TestableProductService Create()
            {
                return new TestableProductService(new Mock<IRepository<Product>>());
            }

            public void SetupRepoGetProducts(IEnumerable<Product> products)
            {
                ProductRepository
                    .Setup(s => s.Table)
                    .Returns(products.AsQueryable());
            }

            public void SetupRepoGetById(Product product)
            {
                ProductRepository
                    .Setup(s => s.GetById(It.IsAny<int>()))
                    .Returns(product);
            }
        }
    }
}

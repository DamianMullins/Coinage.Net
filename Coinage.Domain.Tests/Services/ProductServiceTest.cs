using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Concrete;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Services;
using Moq;
using NUnit.Framework;
using System;
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
                    new Product {Id = 1, Name = "First Product"},
                    new Product {Id = 2, Name = "Second Product"}
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
                    new Product {Id = 1, Name = "First Product", IsFeatured = true},
                    new Product {Id = 2, Name = "Second Product", IsFeatured = false}
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
                    new Product {Id = 1, Name = "First Product", IsFeatured = true},
                    new Product {Id = 2, Name = "Second Product", IsFeatured = true}
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
                    new Product {Id = 1, Name = "First Product", IsFeatured = false},
                    new Product {Id = 2, Name = "Second Product", IsFeatured = false}
                };
                service.SetupRepoGetProducts(products);

                // Act
                var result = service.GetFeaturedProducts();

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        public class GetLatestProducts
        {
            [Test]
            public void GetLatestProducts_NoProducts_ReturnsEmptyList()
            {
                // Arrange
                var service = TestableProductService.Create();
                int count = 100;

                // Act
                var result = service.GetLatestProducts(count);

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(0, result.Count);
            }

            [Test]
            public void GetLatestProducts_NumberOfProductsLessThanCount_ReturnsAllProductsInCorrectOrder()
            {
                // Arrange
                var service = TestableProductService.Create();
                var products = new List<Product>
                {
                    new Product {Id = 1, Name = "First Product", CreatedOn = DateTime.Now.AddHours(-1)},
                    new Product {Id = 2, Name = "Second Product", CreatedOn = DateTime.Now}
                };
                service.SetupRepoGetProducts(products);
                int count = 100;

                // Act
                var result = service.GetLatestProducts(count);

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(2, result[0].Id);
                Assert.AreEqual(1, result[1].Id);
            }

            [Test]
            public void GetLatestProducts_NumberOfProductsGreaterThanCount_ReturnsCorrectNumberOfProductsInCorrectOrder()
            {
                // Arrange
                var service = TestableProductService.Create();
                var products = new List<Product>
                {
                    new Product {Id = 1, Name = "First Product", CreatedOn = DateTime.Now.AddHours(-3)},
                    new Product {Id = 2, Name = "Second Product", CreatedOn = DateTime.Now.AddHours(-2)},
                    new Product {Id = 3, Name = "Third Product", CreatedOn = DateTime.Now.AddHours(-1)},
                    new Product {Id = 4, Name = "Fourth Product", CreatedOn = DateTime.Now}
                };
                service.SetupRepoGetProducts(products);
                int count = 2;

                // Act
                var result = service.GetLatestProducts(count);

                // Assert
                Assert.IsInstanceOf<List<Product>>(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(4, result[0].Id);
                Assert.AreEqual(3, result[1].Id);
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
                var product = new Product { Id = 1 };
                service.SetupRepoGetById(product);

                // Act
                var result = service.GetProduct(product.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, product.Id);
            }
        }

        public class Update
        {
            [Test]
            public void Update_NullProduct_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableProductService.Create();
                Product product = null;

                // Act
                EntityActionResponse result = service.Update(product);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Successful);
            }

            [Test]
            public void Update_ProductRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableProductService.Create();
                var exception = new Exception("Error");
                service.ProductRepository.Setup(r => r.Update(It.IsAny<Product>())).Throws(exception);
                var product = new Product();

                // Act
                EntityActionResponse result = service.Update(product);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Successful);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public void Update_ProductRepoUpdatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableProductService.Create();
                var product = new Product();

                // Act
                EntityActionResponse result = service.Update(product);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Successful);
                Assert.IsNull(result.Exception);
            }
        }

        public class Create
        {
            [Test]
            public void Create_NullProduct_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableProductService.Create();
                Product product = null;

                // Act
                EntityActionResponse result = service.Update(product);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Successful);
            }

            [Test]
            public void Create_ProductRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableProductService.Create();
                var exception = new Exception("Error");
                service.ProductRepository.Setup(r => r.Insert(It.IsAny<Product>())).Throws(exception);
                var product = new Product();

                // Act
                EntityActionResponse result = service.Create(product);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Successful);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public void Create_ProductRepoCreatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableProductService.Create();
                var product = new Product();

                // Act
                EntityActionResponse result = service.Create(product);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Successful);
                Assert.IsNull(result.Exception);
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

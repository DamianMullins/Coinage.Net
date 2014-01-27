﻿using Coinage.Domain.Abstract.Data;
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
    public class BasketServiceTest
    {
        public class GetBasket
        {
            [Test]
            public void GetBasket_NonExistentBasketId_ReturnsNull()
            {
                // Arrange
                var service = TestableBasketService.Create();
                int basketId = 1;

                // Act
                var result = service.GetBasket(basketId);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetBasket_ExistingBasketId_ReturnsBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket { Id = 1 };
                service.SetupRepoGetById(basket);

                // Act
                var result = service.GetBasket(basket.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, basket.Id);
            }
        }

        public class GetCustomerBasket
        {
            [Test]
            public void GetCustomerBasket_CustomerIdWithNoBasket_ReturnsNull()
            {
                // Arrange
                var service = TestableBasketService.Create();
                int customerId = 1;
                var basket = new Basket { Id = 1, CustomerId = 2 };
                service.SetupRepoTable(new List<Basket> { basket });

                // Act
                var result = service.GetCustomerBasket(customerId);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetCustomerBasket_CustomerIdWithBasket_ReturnsBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                int customerId = 1;
                var basket = new Basket { Id = 1, CustomerId = 1 };
                service.SetupRepoTable(new List<Basket> { basket });

                // Act
                var result = service.GetCustomerBasket(customerId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, basket.Id);
                Assert.AreEqual(customerId, basket.CustomerId);
            }
        }

        public class AddToCart
        {
            [Test]
            public void AddToCart_NullBasket_ThrowsArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket nullBasket = null;
                var product = new Product();
                int quantity = 1;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => service.AddToCart(nullBasket, product, quantity));
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddToCart_NullProduct_ThrowsArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product nullProduct = null;
                int quantity = 1;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => service.AddToCart(basket, nullProduct, quantity));

                // Assert
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddToCart_ZeroQuantity_ReturnsWithoutUpdatingBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product();
                int zeroQuantity = 0;

                // Act
                service.AddToCart(basket, product, zeroQuantity);

                // Assert
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddToCart_NegativeQuantity_ReturnsWithoutUpdatingBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product();
                int negativeQuantity = -1;

                // Act
                service.AddToCart(basket, product, negativeQuantity);

                // Assert
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddToCart_ProductNotAlreadyAdded_UpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                int quantity = 1;

                // Act
                service.AddToCart(basket, product, quantity);

                // Assert
                Assert.AreEqual(1, basket.BasketItems.Count);
                Assert.AreEqual(1, basket.TotalItems); // Bonus assertion!
                Assert.AreEqual(1, basket.BasketItems.First().Quantity);
                Assert.IsNotNull(basket.BasketItems.First().CreatedOn);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public void AddToCart_ProductNotAlreadyAddedOtherProductsAlreadyInBasket_UpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = 2, Quantity = 1 });
                int quantity = 1;

                // Act
                service.AddToCart(basket, product, quantity);

                // Assert
                Assert.AreEqual(2, basket.BasketItems.Count);
                Assert.AreEqual(2, basket.TotalItems); // Bonus assertion!
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public void AddToCart_ProductAlreadyAdded_UpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = product.Id, Quantity = 1 });
                int quantity = 1;

                // Act
                service.AddToCart(basket, product, quantity);

                // Assert
                Assert.AreEqual(1, basket.BasketItems.Count);
                Assert.AreEqual(2, basket.TotalItems);
                Assert.AreEqual(2, basket.BasketItems.First().Quantity);
                Assert.IsNotNull(basket.BasketItems.First().CreatedOn);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public void AddToCart_ProductAlreadyAddedOtherProductsAlreadyInBasket_UpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = 2, Quantity = 1 });
                basket.AddBasketItem(new BasketItem { ProductId = product.Id, Quantity = 1 });
                int quantity = 1;

                // Act
                service.AddToCart(basket, product, quantity);

                // Assert
                Assert.AreEqual(2, basket.BasketItems.Count);
                Assert.AreEqual(3, basket.TotalItems);
                Assert.AreEqual(2, basket.BasketItems.First(b => b.ProductId == product.Id).Quantity);
                Assert.IsNotNull(basket.BasketItems.First(b => b.ProductId == product.Id).CreatedOn);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }
        }

        public class Update
        {
            [Test]
            public void Update_NullBasket_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket basket = null;

                // Act
                EntityActionResponse result = service.Update(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Successful);
            }

            [Test]
            public void Update_BasketRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var exception = new Exception("Error");
                service.BasketRepository.Setup(r => r.Update(It.IsAny<Basket>())).Throws(exception);
                var basket = new Basket();

                // Act
                EntityActionResponse result = service.Update(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Successful);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public void Update_BasketRepoUpdatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();

                // Act
                EntityActionResponse result = service.Update(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Successful);
                Assert.IsNull(result.Exception);
            }
        }

        public class Create
        {
            [Test]
            public void Create_NullBasket_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket basket = null;

                // Act
                EntityActionResponse result = service.Update(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Successful);
            }

            [Test]
            public void Create_BasketRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var exception = new Exception("Error");
                service.BasketRepository.Setup(r => r.Insert(It.IsAny<Basket>())).Throws(exception);
                var basket = new Basket();

                // Act
                EntityActionResponse result = service.Create(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Successful);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public void Create_BasketRepoCreatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();

                // Act
                EntityActionResponse result = service.Create(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Successful);
                Assert.IsNull(result.Exception);
            }
        }

        private class TestableBasketService : BasketService
        {
            public readonly Mock<IRepository<Basket>> BasketRepository;

            private TestableBasketService(Mock<IRepository<Basket>> basketRepository)
                : base(basketRepository.Object)
            {
                BasketRepository = basketRepository;
            }

            public static TestableBasketService Create()
            {
                return new TestableBasketService(new Mock<IRepository<Basket>>());
            }

            public void SetupRepoTable(IEnumerable<Basket> baskets)
            {
                BasketRepository
                    .Setup(s => s.Table)
                    .Returns(baskets.AsQueryable());
            }

            public void SetupRepoGetById(Basket basket)
            {
                BasketRepository
                    .Setup(s => s.GetById(It.IsAny<int>()))
                    .Returns(basket);
            }
        }
    }
}
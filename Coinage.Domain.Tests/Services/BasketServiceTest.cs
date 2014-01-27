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

        public class Update
        {
            [Test]
            public void Update_NullBasket_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket basket = null;

                // Act
                EntityActionResponse result = service.Update(basket);

                // Assert
                Assert.IsNotNull(result);
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
            public void Create_NullBasket_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket basket = null;

                // Act
                EntityActionResponse result = service.Update(basket);

                // Assert
                Assert.IsNotNull(result);
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

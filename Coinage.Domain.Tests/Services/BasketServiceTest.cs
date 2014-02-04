using System.Collections.ObjectModel;
using Coinage.Domain.Abstract;
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
            public void GetCustomerBasket_CustomerIdWithNoBasketRepoThrowsError_ReturnsUnsuccessfulWithException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var customer = new Customer { Id = 1 };
                var basket = new Basket { Id = 1, CustomerId = 2 };
                service.SetupRepoTable(new List<Basket> { basket });
                service.SetupWorkContextCurrentCustomer(customer);
                service.BasketRepository.Setup(b => b.Insert(It.IsAny<Basket>())).Throws<Exception>();

                // Act & Assert
                Assert.Throws<Exception>(() => service.GetCustomerBasket());
            }

            [Test]
            public void GetCustomerBasket_CustomerIdWithNoBasket_CreatesAndReturnsNewBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var customer = new Customer { Id = 1 };
                var basket = new Basket { Id = 1, CustomerId = 2 };
                service.SetupRepoTable(new List<Basket> { basket });
                service.SetupWorkContextCurrentCustomer(customer);

                // Act
                Basket result = service.GetCustomerBasket();

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<DateTime>(result.CreatedOn);
                Assert.AreEqual(customer.Id, result.CustomerId);
            }

            [Test]
            public void GetCustomerBasket_CustomerIdWithBasket_ReturnsBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var customer = new Customer { Id = 1 };
                var basket = new Basket { Id = 1, CustomerId = 1 };
                service.SetupRepoTable(new List<Basket> { basket });
                service.SetupWorkContextCurrentCustomer(customer);

                // TODO
                // Act
                var result = service.GetCustomerBasket();

                //// Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(customer.Id, result.CustomerId);
            }
        }

        public class AddProductToBasket
        {
            [Test]
            public void AddProductToBasket_NullBasket_ReturnsUnsuccessfulResponseWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket nullBasket = null;
                var product = new Product();
                int quantity = 1;

                // Act
                EntityActionResponse response = service.AddProductToBasket(nullBasket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Successful);
                Assert.IsInstanceOf<ArgumentNullException>(response.Exception);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddProductToBasket_NullProduct_ReturnsUnsuccessfulResponseWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product nullProduct = null;
                int quantity = 1;

                // Act
                EntityActionResponse response = service.AddProductToBasket(basket, nullProduct, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Successful);
                Assert.IsInstanceOf<ArgumentNullException>(response.Exception);
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddProductToBasket_ZeroQuantity_ReturnsUnsuccessfulResponseWithoutUpdatingBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product();
                int zeroQuantity = 0;

                // Act
                EntityActionResponse response = service.AddProductToBasket(basket, product, zeroQuantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Successful);
                Assert.IsInstanceOf<Exception>(response.Exception);
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddProductToBasket_NegativeQuantity_ReturnsUnsuccessfulResponseWithoutUpdatingBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product();
                int negativeQuantity = -1;

                // Act
                EntityActionResponse response = service.AddProductToBasket(basket, product, negativeQuantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Successful);
                Assert.IsInstanceOf<Exception>(response.Exception);
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public void AddProductToBasket_ProductNotAlreadyAdded_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                int quantity = 1;

                // Act
                EntityActionResponse response = service.AddProductToBasket(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Successful);
                Assert.AreEqual(1, basket.BasketItems.Count);
                Assert.AreEqual(1, basket.TotalItems); // Bonus assertion!
                Assert.AreEqual(1, basket.BasketItems.First().Quantity);
                Assert.IsNotNull(basket.BasketItems.First().CreatedOn);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public void AddProductToBasket_ProductNotAlreadyAddedOtherProductsAlreadyInBasket_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = 2, Quantity = 1 });
                int quantity = 1;

                // Act
                EntityActionResponse response = service.AddProductToBasket(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Successful);
                Assert.AreEqual(2, basket.BasketItems.Count);
                Assert.AreEqual(2, basket.TotalItems); // Bonus assertion!
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public void AddProductToBasket_ProductAlreadyAdded_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = product.Id, Quantity = 1 });
                int quantity = 1;

                // Act
                EntityActionResponse response = service.AddProductToBasket(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Successful);
                Assert.AreEqual(1, basket.BasketItems.Count);
                Assert.AreEqual(2, basket.TotalItems);
                Assert.AreEqual(2, basket.BasketItems.First().Quantity);
                Assert.IsNotNull(basket.BasketItems.First().CreatedOn);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public void AddProductToBasket_ProductAlreadyAddedOtherProductsAlreadyInBasket_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = 2, Quantity = 1 });
                basket.AddBasketItem(new BasketItem { ProductId = product.Id, Quantity = 1 });
                int quantity = 1;

                // Act
                EntityActionResponse response = service.AddProductToBasket(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Successful);
                Assert.AreEqual(2, basket.BasketItems.Count);
                Assert.AreEqual(3, basket.TotalItems);
                Assert.AreEqual(2, basket.BasketItems.First(b => b.ProductId == product.Id).Quantity);
                Assert.IsNotNull(basket.BasketItems.First(b => b.ProductId == product.Id).CreatedOn);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Once);
            }
        }

        public class UpdateProductInBasket
        {
            [Test]
            public void UpdateProductInBasket_BasketItemNotFound_ReturnsUnsuccessfulResponse()
            {
                // Arrange
                var service = TestableBasketService.Create();
                service.SetupBasketItemRepositoryGetById(null);
                int basketItemId = 1;
                int productId = 1;
                int quantity = 1;

                // Act
                EntityActionResponse response = service.UpdateProductInBasket(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Successful);
                Assert.IsInstanceOf<NullReferenceException>(response.Exception);
            }

            [Test]
            public void UpdateProductInBasket_RepoThrowsError_ReturnsUnsuccessfulResponse()
            {
                // Arrange
                var service = TestableBasketService.Create();
                service.BasketItemRepository.Setup(bi => bi.GetById(It.IsAny<int>())).Throws<Exception>();
                int basketItemId = 1;
                int productId = 1;
                int quantity = 1;

                // Act
                EntityActionResponse response = service.UpdateProductInBasket(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Successful);
            }

            [Test]
            public void UpdateProductInBasket_BasketItemNotInBasket_ReturnsUnsuccessfulResponse()
            {
                // Arrange
                var service = TestableBasketService.Create();
                service.SetupWorkContextCurrentCustomer(new Customer());
                service.SetupRepoTable(new List<Basket> { new Basket() });
                service.SetupBasketItemRepositoryGetById(new BasketItem());
                int basketItemId = 1;
                int productId = 1;
                int quantity = 1;

                // Act
                EntityActionResponse response = service.UpdateProductInBasket(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Successful);
            }

            [Test]
            public void UpdateProductInBasket_BasketItemInBasketQuantityLessThanOne_DeletesBasketItemAndReturnsSuccessfulResponse()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                var basketItem = new BasketItem { Id = 1 };
                basket.AddBasketItem(basketItem);
                service.SetupWorkContextCurrentCustomer(new Customer());
                service.SetupRepoTable(new List<Basket> { basket });
                service.SetupBasketItemRepositoryGetById(basketItem);
                int basketItemId = 1;
                int productId = 1;
                int quantity = 0;

                // Act
                EntityActionResponse response = service.UpdateProductInBasket(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Successful);
                service.BasketItemRepository.Verify(bi => bi.Delete(It.IsAny<BasketItem>()), Times.Once);
            }

            [Test]
            public void UpdateProductInBasket_BasketItemInBasketQuantityGreaterThanZero_UpdatesBasketItemAndReturnsSuccessfulResponse()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                var basketItem = new BasketItem { Id = 1, Quantity = 1 };
                basket.AddBasketItem(basketItem);
                service.SetupWorkContextCurrentCustomer(new Customer());
                service.SetupRepoTable(new List<Basket> { basket });
                service.SetupBasketItemRepositoryGetById(basketItem);
                int basketItemId = 1;
                int productId = 1;
                int quantity = 2;

                // Act
                EntityActionResponse response = service.UpdateProductInBasket(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Successful);
                Assert.AreEqual(quantity, basketItem.Quantity);
                service.BasketItemRepository.Verify(bi => bi.Update(It.IsAny<BasketItem>()), Times.Once);
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
            public readonly Mock<IWorkContext> WorkContext;
            public readonly Mock<IRepository<Basket>> BasketRepository;
            public readonly Mock<IRepository<BasketItem>> BasketItemRepository;

            private TestableBasketService(Mock<IWorkContext> workContext, Mock<IRepository<Basket>> basketRepository, Mock<IRepository<BasketItem>> basketItemRepository)
                : base(workContext.Object, basketRepository.Object, basketItemRepository.Object)
            {
                WorkContext = workContext;
                BasketRepository = basketRepository;
                BasketItemRepository = basketItemRepository;
            }

            public static TestableBasketService Create()
            {
                return new TestableBasketService(new Mock<IWorkContext>(), new Mock<IRepository<Basket>>(), new Mock<IRepository<BasketItem>>());
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

            public void SetupWorkContextCurrentCustomer(Customer customer)
            {
                WorkContext
                    .Setup(s => s.CurrentCustomer)
                    .Returns(customer);
            }

            public void SetupBasketItemRepositoryGetById(BasketItem basketItem)
            {
                BasketItemRepository
                    .Setup(s => s.GetById(It.IsAny<int>()))
                    .Returns(basketItem);
            }
        }
    }
}

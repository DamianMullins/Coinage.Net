using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coinage.Domain.Authentication;
using Coinage.Domain.Data;
using Coinage.Domain.Entites;
using Coinage.Domain.Models;
using Coinage.Domain.Services;
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
        public class GetBasketAsync
        {
            [Test]
            public async Task GetBasketAsync_NonExistentBasketId_ReturnsNull()
            {
                // Arrange
                var service = TestableBasketService.Create();
                int basketId = 1;

                // Act
                var result = await service.GetBasketAsync(basketId);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public async Task GetBasketAsync_ExistingBasketId_ReturnsBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket { Id = 1 };
                service.SetupBasketRepoFindAsync(basket);

                // Act
                var result = await service.GetBasketAsync(basket.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, basket.Id);
            }
        }

        public class GetCustomerBasketAsync
        {
            [Test]
            public async Task GetCustomerBasketAsync_CustomerIdWithNoBasketRepoThrowsError_ReturnsUnsuccessfulWithException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var customer = new Customer { Id = 1 };
                service.SetupWorkContextCurrentCustomer(customer);
                service.BasketRepository.Setup(b => b.InsertAsync(It.IsAny<Basket>())).Throws<Exception>();

                // Act & Assert
                Assert.Throws<Exception>(async () => await service.GetCustomerBasketAsync());
            }

            [Test]
            public async Task GetCustomerBasketAsync_CustomerIdWithNoBasket_CreatesAndReturnsNewBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var customer = new Customer { Id = 1 };
                service.SetupWorkContextCurrentCustomer(customer);

                // Act
                Basket result = await service.GetCustomerBasketAsync();

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<DateTime>(result.CreatedOn);
                Assert.AreEqual(customer.Id, result.CustomerId);
                service.BasketRepository.Verify(b => b.InsertAsync(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public async Task GetCustomerBasketAsync_CustomerIdWithBasket_ReturnsBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var customer = new Customer { Id = 1 };
                var basket = new Basket { Id = 1, CustomerId = 1 };
                service.SetupBasketRepoFindAsync(basket);
                service.SetupWorkContextCurrentCustomer(customer);

                // Act
                Basket result = await service.GetCustomerBasketAsync();

                //// Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(customer.Id, result.CustomerId);
                service.BasketRepository.Verify(b => b.InsertAsync(It.IsAny<Basket>()), Times.Never);
            }
        }

        public class AddProductToBasketAsync
        {
            [Test]
            public async Task AddProductToBasketAsync_NullBasket_ReturnsUnsuccessfulResponseWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket nullBasket = null;
                var product = new Product();
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(nullBasket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
                Assert.IsInstanceOf<ArgumentNullException>(response.Exception);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public async Task AddProductToBasketAsync_NullProduct_ReturnsUnsuccessfulResponseWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product nullProduct = null;
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(basket, nullProduct, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
                Assert.IsInstanceOf<ArgumentNullException>(response.Exception);
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public async Task AddProductToBasketAsync_ZeroQuantity_ReturnsUnsuccessfulResponseWithoutUpdatingBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product();
                int zeroQuantity = 0;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(basket, product, zeroQuantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
                Assert.IsInstanceOf<Exception>(response.Exception);
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public async Task AddProductToBasketAsync_NegativeQuantity_ReturnsUnsuccessfulResponseWithoutUpdatingBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product();
                int negativeQuantity = -1;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(basket, product, negativeQuantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
                Assert.IsInstanceOf<Exception>(response.Exception);
                Assert.AreEqual(0, basket.BasketItems.Count);
                service.BasketRepository.Verify(b => b.Update(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public async Task AddProductToBasketAsync_ProductNotAlreadyAdded_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(1, basket.BasketItems.Count);
                Assert.AreEqual(1, basket.TotalItems); // Bonus assertion!
                Assert.AreEqual(1, basket.BasketItems.First().Quantity);
                Assert.IsNotNull(basket.BasketItems.First().CreatedOn);
                service.BasketRepository.Verify(b => b.UpdateAsync(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public async Task AddProductToBasketAsync_ProductNotAlreadyAddedOtherProductsAlreadyInBasket_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = 2, Quantity = 1 });
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(2, basket.BasketItems.Count);
                Assert.AreEqual(2, basket.TotalItems); // Bonus assertion!
                service.BasketRepository.Verify(b => b.UpdateAsync(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public async Task AddProductToBasketAsync_ProductAlreadyAdded_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = product.Id, Quantity = 1 });
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(1, basket.BasketItems.Count);
                Assert.AreEqual(2, basket.TotalItems);
                Assert.AreEqual(2, basket.BasketItems.First().Quantity);
                Assert.IsNotNull(basket.BasketItems.First().CreatedOn);
                service.BasketRepository.Verify(b => b.UpdateAsync(It.IsAny<Basket>()), Times.Once);
            }

            [Test]
            public async Task AddProductToBasketAsync_ProductAlreadyAddedOtherProductsAlreadyInBasket_ReturnsSuccessfulAndUpdatesBasket()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();
                Product product = new Product { Id = 1 };
                basket.AddBasketItem(new BasketItem { ProductId = 2, Quantity = 1 });
                basket.AddBasketItem(new BasketItem { ProductId = product.Id, Quantity = 1 });
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.AddProductToBasketAsync(basket, product, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(2, basket.BasketItems.Count);
                Assert.AreEqual(3, basket.TotalItems);
                Assert.AreEqual(2, basket.BasketItems.First(b => b.ProductId == product.Id).Quantity);
                Assert.IsNotNull(basket.BasketItems.First(b => b.ProductId == product.Id).CreatedOn);
                service.BasketRepository.Verify(b => b.UpdateAsync(It.IsAny<Basket>()), Times.Once);
            }
        }

        public class UpdateProductInBasketAsync
        {
            [Test]
            public async Task UpdateProductInBasketAsync_BasketItemNotFound_ReturnsUnsuccessfulResponse()
            {
                // Arrange
                var service = TestableBasketService.Create();
                int basketItemId = 1;
                int productId = 1;
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.UpdateProductInBasketAsync(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
                Assert.IsInstanceOf<NullReferenceException>(response.Exception);
            }

            [Test]
            public async Task UpdateProductInBasketAsync_RepoThrowsError_ReturnsUnsuccessfulResponse()
            {
                // Arrange
                int basketItemId = 1;
                int productId = 1;
                int quantity = 1;
                var basket = new Basket();
                basket.AddBasketItem(new BasketItem { Id = basketItemId });
                var service = TestableBasketService.Create();
                service.SetupBasketRepoFindAsync(basket);
                service.BasketItemRepository.Setup(bi => bi.UpdateAsync(It.IsAny<BasketItem>())).Throws<Exception>();

                // Act
                EntityActionResponse response = await service.UpdateProductInBasketAsync(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
            }

            [Test]
            public async Task UpdateProductInBasketAsync_BasketItemNotInBasket_ReturnsUnsuccessfulResponse()
            {
                // Arrange
                var service = TestableBasketService.Create();
                service.SetupWorkContextCurrentCustomer(new Customer());
                int basketItemId = 1;
                int productId = 1;
                int quantity = 1;

                // Act
                EntityActionResponse response = await service.UpdateProductInBasketAsync(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
            }

            [Test]
            public async Task UpdateProductInBasketAsync_BasketItemInBasketQuantityLessThanOne_DeletesBasketItemAndReturnsSuccessfulResponse()
            {
                // Arrange
                int customerId = 1;
                var service = TestableBasketService.Create();
                var basket = new Basket { CustomerId = customerId };
                var basketItem = new BasketItem { Id = 1 };
                basket.AddBasketItem(basketItem);
                service.SetupWorkContextCurrentCustomer(new Customer { Id = customerId });
                service.SetupBasketRepoFindAsync(basket);
                service.SetupBasketItemRepoFindAsync(basketItem);
                int basketItemId = 1;
                int productId = 1;
                int quantity = 0;

                // Act
                EntityActionResponse response = await service.UpdateProductInBasketAsync(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                service.BasketItemRepository.Verify(bi => bi.Delete(It.IsAny<BasketItem>()), Times.Once);
            }

            [Test]
            public async Task UpdateProductInBasketAsync_BasketItemInBasketQuantityGreaterThanZero_UpdatesBasketItemAndReturnsSuccessfulResponse()
            {
                // Arrange
                int customerId = 1;
                var service = TestableBasketService.Create();
                var basket = new Basket { CustomerId = customerId };
                var basketItem = new BasketItem { Id = 1, Quantity = 1 };
                basket.AddBasketItem(basketItem);
                service.SetupWorkContextCurrentCustomer(new Customer { Id = customerId });
                service.SetupBasketRepoFindAsync(basket);
                service.SetupBasketItemRepoFindAsync(basketItem);
                int basketItemId = 1;
                int productId = 1;
                int quantity = 2;

                // Act
                EntityActionResponse response = await service.UpdateProductInBasketAsync(basketItemId, productId, quantity);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(quantity, basketItem.Quantity);
                service.BasketItemRepository.Verify(bi => bi.UpdateAsync(It.IsAny<BasketItem>()), Times.Once);
            }
        }

        public class UpdateAsync
        {
            [Test]
            public async Task UpdateAsync_NullBasket_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket basket = null;

                // Act
                EntityActionResponse result = await service.UpdateAsync(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Success);
                service.BasketRepository.Verify(b => b.UpdateAsync(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public async Task UpdateAsync_BasketRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var exception = new Exception("Error");
                service.BasketRepository.Setup(r => r.UpdateAsync(It.IsAny<Basket>())).Throws(exception);
                var basket = new Basket();

                // Act
                EntityActionResponse result = await service.UpdateAsync(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Success);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public async Task UpdateAsync_BasketRepoUpdatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();

                // Act
                EntityActionResponse result = await service.UpdateAsync(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success);
                Assert.IsNull(result.Exception);
                service.BasketRepository.Verify(b => b.UpdateAsync(It.IsAny<Basket>()), Times.Once);
            }
        }

        public class CreateAsync
        {
            [Test]
            public async Task CreateAsync_NullBasket_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableBasketService.Create();
                Basket basket = null;

                // Act
                EntityActionResponse result = await service.CreateAsync(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Success);
                service.BasketRepository.Verify(b => b.InsertAsync(It.IsAny<Basket>()), Times.Never);
            }

            [Test]
            public async Task CreateAsync_BasketRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var exception = new Exception("Error");
                service.BasketRepository.Setup(r => r.InsertAsync(It.IsAny<Basket>())).Throws(exception);
                var basket = new Basket();

                // Act
                EntityActionResponse result = await service.CreateAsync(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Success);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public async Task CreateAsync_BasketRepoCreatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableBasketService.Create();
                var basket = new Basket();

                // Act
                EntityActionResponse result = await service.CreateAsync(basket);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success);
                Assert.IsNull(result.Exception);
                service.BasketRepository.Verify(b => b.InsertAsync(It.IsAny<Basket>()), Times.Once);
            }
        }

        private class TestableBasketService : BasketService
        {
            public readonly Mock<IAuthenticationService> AuthenticationService;
            public readonly Mock<IRepositoryAsync<Basket>> BasketRepository;
            public readonly Mock<IRepositoryAsync<BasketItem>> BasketItemRepository;

            private TestableBasketService(Mock<IAuthenticationService> authenticationService, Mock<IRepositoryAsync<Basket>> basketRepository, Mock<IRepositoryAsync<BasketItem>> basketItemRepository)
                : base(authenticationService.Object, basketRepository.Object, basketItemRepository.Object)
            {
                AuthenticationService = authenticationService;
                BasketRepository = basketRepository;
                BasketItemRepository = basketItemRepository;
            }

            public static TestableBasketService Create()
            {
                return new TestableBasketService(new Mock<IAuthenticationService>(), new Mock<IRepositoryAsync<Basket>>(), new Mock<IRepositoryAsync<BasketItem>>());
            }

            public void SetupWorkContextCurrentCustomer(Customer customer)
            {
                AuthenticationService
                    .Setup(s => s.CurrentCustomer)
                    .Returns(customer);
            }

            public void SetupBasketRepoFindAsync(Basket basket)
            {
                BasketRepository
                    .Setup(s => s.FindAsync(It.IsAny<Expression<Func<Basket, bool>>>()))
                    .Returns(Task.FromResult(basket));
            }

            public void SetupBasketItemRepoFindAsync(BasketItem basketItem)
            {
                BasketItemRepository
                    .Setup(s => s.FindAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                    .Returns(Task.FromResult(basketItem));
            }
        }
    }
}

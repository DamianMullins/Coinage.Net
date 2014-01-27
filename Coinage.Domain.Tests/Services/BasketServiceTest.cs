using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Services;
using Moq;
using NUnit.Framework;

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

            public void SetupRepoGetById(Basket basket)
            {
                BasketRepository
                    .Setup(s => s.GetById(It.IsAny<int>()))
                    .Returns(basket);
            }
        }
    }
}

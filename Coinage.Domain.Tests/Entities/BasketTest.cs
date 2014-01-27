using System.Collections.Generic;
using Coinage.Domain.Concrete.Entities;
using NUnit.Framework;

namespace Coinage.Domain.Tests.Entities
{
    public class BasketTest
    {
        [Test]
        public void TotalItems_NoBasketItems_ReturnsZero()
        {
            // Arrange
            var basket = new Basket();

            // Assert
            Assert.AreEqual(0, basket.TotalItems);
        }

        [Test]
        public void TotalItems_MultipleBasketItemsWithQuantityOfOne_ReturnsCount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddBasketItem(new BasketItem { Quantity = 1 });
            basket.AddBasketItem(new BasketItem { Quantity = 1 });

            // Assert
            Assert.AreEqual(2, basket.TotalItems);
        }

        [Test]
        public void TotalItems_MultipleBasketItemsWithQuantityGreaterThanOne_ReturnsCount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddBasketItem(new BasketItem {Quantity = 5});
            basket.AddBasketItem(new BasketItem {Quantity = 10});

            // Assert
            Assert.AreEqual(15, basket.TotalItems);
        }
    }
}

using Coinage.Domain.Entites;
using NUnit.Framework;

namespace Coinage.Domain.Tests.Entities
{
    public class BasketTest
    {
        /* Total Items */
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


        /* Total Amount */
        [Test]
        public void TotalAmount_NoBasketItems_ReturnsZero()
        {
            // Arrange
            var basket = new Basket();

            // Assert
            Assert.AreEqual(0, basket.TotalAmount);
        }

        [Test]
        public void TotalAmount_SingleBasketItemWithQuantityOfOne_ReturnsAmount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddBasketItem(new BasketItem { Quantity = 1, Product = new Product { Price = 10 } });

            // Assert
            Assert.AreEqual(10, basket.TotalAmount);
        }

        [Test]
        public void TotalAmount_SingleBasketItemWithQuantityGreaterThanOne_ReturnsAmount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddBasketItem(new BasketItem { Quantity = 2, Product = new Product { Price = 10 } });

            // Assert
            Assert.AreEqual(20, basket.TotalAmount);
        }

        [Test]
        public void TotalAmount_MultipleBasketItemsWithQuantityOfOne_ReturnsAmount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddBasketItem(new BasketItem { Quantity = 1, Product = new Product { Price = 10 } });
            basket.AddBasketItem(new BasketItem { Quantity = 1, Product = new Product { Price = 10 } });

            // Assert
            Assert.AreEqual(20, basket.TotalAmount);
        }

        [Test]
        public void TotalAmount_MultipleBasketItemsWithQuantityGreaterThanOne_ReturnsAmount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddBasketItem(new BasketItem { Quantity = 5, Product = new Product { Price = 10 } });
            basket.AddBasketItem(new BasketItem { Quantity = 10, Product = new Product { Price = 10 } });

            // Assert
            Assert.AreEqual(150, basket.TotalAmount);
        }
    }
}

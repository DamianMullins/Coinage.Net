﻿using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Abstract.Services
{
    public interface IBasketService : IEditableService<Basket>
    {
        /// <summary>
        /// Get a Basket by its primary key.
        /// </summary>
        /// <param name="id">Primary key of the Basket.</param>
        /// <returns>A Basket object.</returns>
        Basket GetBasket(int id);

        /// <summary>
        /// Get a Basket by its owning customer primary key.
        /// </summary>
        /// <param name="customerId">Primary key of the owning customer.</param>
        /// <returns>A Basket object.</returns>
        Basket GetCustomerBasket(int customerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        void AddToCart(Basket basket, Product product, int quantity);
    }
}

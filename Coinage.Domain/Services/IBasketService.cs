using Coinage.Domain.Entites;
using Coinage.Domain.Models;

namespace Coinage.Domain.Services
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
        /// <returns>A Basket object.</returns>
        Basket GetCustomerBasket();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns>EntityActionResponse object.</returns>
        EntityActionResponse AddProductToBasket(Basket basket, Product product, int quantity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basketItemId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        EntityActionResponse UpdateProductInBasket(int basketItemId, int productId, int quantity);
    }
}

using System.Threading.Tasks;
using Coinage.Domain.Entites;
using Coinage.Domain.Models;

namespace Coinage.Domain.Services
{
    public interface IBasketService : IEditableServiceAsync<Basket>
    {
        /// <summary>
        /// Asynchronously get a Basket by its primary key.
        /// </summary>
        /// <param name="id">Primary key of the Basket.</param>
        /// <returns>A Basket object.</returns>
        Task<Basket> GetBasketAsync(int id);

        /// <summary>
        /// Asynchronously get a Basket by its owning customer primary key.
        /// </summary>
        /// <returns>A Basket object.</returns>
        Task<Basket> GetCustomerBasketAsync();

        /// <summary>
        /// Asynchronously add a product to a basket.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns>EntityActionResponse object.</returns>
        Task<EntityActionResponse> AddProductToBasketAsync(Basket basket, Product product, int quantity);

        /// <summary>
        /// Asynchronously update a product in a basket.
        /// </summary>
        /// <param name="basketItemId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<EntityActionResponse> UpdateProductInBasketAsync(int basketItemId, int productId, int quantity);
    }
}

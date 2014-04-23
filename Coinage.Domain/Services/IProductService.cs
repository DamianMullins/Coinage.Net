using Coinage.Domain.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coinage.Domain.Services
{
    public interface IProductService : IEditableServiceAsync<Product>
    {
        /// <summary>
        /// Get a list of all Products.
        /// </summary>
        /// <returns>List of Product objects.</returns>
        IEnumerable<Product> GetProducts();

        /// <summary>
        /// Asynchronously get a list of all Products.
        /// </summary>
        /// <returns>List of Product objects.</returns>
        Task<IEnumerable<Product>> GetProductsAsync();

        /// <summary>
        /// Get a list of featured Products, order by last modified.
        /// </summary>
        /// <returns>List of Product objects.</returns>
        IEnumerable<Product> GetFeaturedProducts();

        /// <summary>
        /// Get a list of the latest products created.
        /// </summary>
        /// <param name="count">Maximum number of records to return.</param>
        /// <returns>List of Product objects.</returns>
        IEnumerable<Product> GetLatestProducts(int count);

        /// <summary>
        /// Get a Product by its primary key.
        /// </summary>
        /// <param name="id">Primary key of the Product.</param>
        /// <returns>A Product object.</returns>
        Product GetProductById(int id);

        /// <summary>
        /// Get a Product by its primary key.
        /// </summary>
        /// <param name="id">Primary key of the Product.</param>
        /// <returns>A Product object.</returns>
        Task<Product> GetProductByIdAsync(int id);
    }
}

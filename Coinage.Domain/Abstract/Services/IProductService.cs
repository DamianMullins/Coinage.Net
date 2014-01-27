using Coinage.Domain.Concrete;
using Coinage.Domain.Concrete.Entities;
using System.Collections.Generic;

namespace Coinage.Domain.Abstract.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get a list of all Products.
        /// </summary>
        /// <returns>List of Product objects.</returns>
        List<Product> GetProducts();

        /// <summary>
        /// Get a list of featured Products, order by last modified.
        /// </summary>
        /// <returns>List of Product objects.</returns>
        List<Product> GetFeaturedProducts();

        /// <summary>
        /// Get a list of the latest products created.
        /// </summary>
        /// <param name="count">Maximum number of records to return.</param>
        /// <returns>List of Product objects.</returns>
        List<Product> GetLatestProducts(int count);

        /// <summary>
        /// Get a single Product by its primary key.
        /// </summary>
        /// <param name="id">Primary key of the Product.</param>
        /// <returns>A Product object.</returns>
        Product GetProduct(int id);

        /// <summary>
        /// Update a Product entity.
        /// </summary>
        /// <param name="product">Product object to update.</param>
        /// <returns>EntityActionResponse object.</returns>
        EntityActionResponse Update(Product product);

        /// <summary>
        /// Create a product entity.
        /// </summary>
        /// <param name="product">Product object to create.</param>
        /// <returns>EntityActionResponse object.</returns>
        EntityActionResponse Create(Product product);
    }
}

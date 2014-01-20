using Coinage.Domain.Concrete.Entities;
using System.Collections.Generic;

namespace Coinage.Domain.Abstract.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        List<Product> GetFeaturedProducts();
        List<Product> GetLatestProducts(int count);

        Product GetProduct(int id);

        void Update(Product product);
        void Create(Product product);
    }
}

using Coinage.Domain.Concrete.Entities;
using System.Collections.Generic;

namespace Coinage.Domain.Abstract.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product GetProduct(int id);
    }
}

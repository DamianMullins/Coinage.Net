using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Data;
using System.Collections.Generic;
using System.Linq;

namespace Coinage.Domain.Concrete.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetProducts()
        {
            return _productRepository.Table.ToList();
        }

        public Product GetProduct(int id)
        {
            return _productRepository.GetById(id);
        }
    }
}

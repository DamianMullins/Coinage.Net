using System;
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

        public List<Product> GetFeaturedProducts()
        {
            return _productRepository.Table.Where(p => p.IsFeatured).ToList();
        }

        public Product GetProduct(int id)
        {
            return _productRepository.GetById(id);
        }

        public void Update(Product product)
        {
            product.ModifiedOn = DateTime.Now;

            _productRepository.Update(product);
        }

        public void Create(Product product)
        {
            product.CreatedOn = DateTime.Now;
            product.ModifiedOn = DateTime.Now;

            _productRepository.Insert(product);
        }
    }
}

using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System;
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
            return _productRepository.Table.Where(p => p.IsFeatured).OrderByDescending(p => p.ModifiedOn).ToList();
        }

        public List<Product> GetLatestProducts(int count)
        {
            return _productRepository.Table.OrderByDescending(p => p.CreatedOn).Take(count).ToList();
        }

        public Product GetProduct(int id)
        {
            return _productRepository.GetById(id);
        }

        public void Update(Product product)
        {
            if (product != null)
            {
                product.ModifiedOn = DateTime.Now;

                _productRepository.Update(product);
            }
            else
            {
                throw new Exception("Product was null");
            }
        }

        public void Create(Product product)
        {
            if (product != null)
            {
                product.CreatedOn = DateTime.Now;

                _productRepository.Insert(product);
            }
            else
            {
                throw new Exception("Product was null");
            }
        }
    }
}

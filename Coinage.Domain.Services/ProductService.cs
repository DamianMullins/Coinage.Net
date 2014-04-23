using Coinage.Domain.Data;
using Coinage.Domain.Entites;
using Coinage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coinage.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryAsync<Product> _productRepository;

        public ProductService(IRepositoryAsync<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public EntityActionResponse Update(Product product)
        {
            if (product == null) return new EntityActionResponse { Exception = new ArgumentNullException("product") };

            var response = new EntityActionResponse();
            try
            {
                _productRepository.Update(product);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public EntityActionResponse Create(Product product)
        {
            if (product == null) return new EntityActionResponse { Exception = new ArgumentNullException("product") };

            var response = new EntityActionResponse();
            try
            {
                _productRepository.Insert(product);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<EntityActionResponse> UpdateAsync(Product product)
        {
            if (product == null) return new EntityActionResponse { Exception = new ArgumentNullException("product") };

            var response = new EntityActionResponse();
            try
            {
                await _productRepository.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<EntityActionResponse> CreateAsync(Product product)
        {
            if (product == null) return new EntityActionResponse { Exception = new ArgumentNullException("product") };

            var response = new EntityActionResponse();
            try
            {
                await _productRepository.InsertAsync(product);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetAll();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public IEnumerable<Product> GetFeaturedProducts()
        {
            return _productRepository.FindAll(p => p.IsFeatured).OrderByDescending(p => p.ModifiedOn);
        }

        public IEnumerable<Product> GetLatestProducts(int count)
        {
            return _productRepository.Table.OrderByDescending(p => p.CreatedOn).Take(count);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.Find(p => p.Id == id);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.FindAsync(p => p.Id == id);
        }
    }
}

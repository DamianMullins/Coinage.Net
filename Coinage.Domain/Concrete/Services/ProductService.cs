﻿using Coinage.Domain.Abstract.Data;
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

        public Product GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public EntityActionResponse Update(Product product)
        {
            var response = new EntityActionResponse();
            if (product != null)
            {
                try
                {
                    product.ModifiedOn = DateTime.Now;
                    _productRepository.Update(product);
                    response.Successful = true;
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                }
            }
            else
            {
                response.Exception = new Exception("Product was null");
            }
            return response;
        }

        public EntityActionResponse Create(Product product)
        {
            var response = new EntityActionResponse();
            if (product != null)
            {
                try
                {
                    product.CreatedOn = DateTime.Now;
                    _productRepository.Insert(product);
                    response.Successful = true;
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                }
            }
            else
            {
                response.Exception = new Exception("Product was null");
            }
            return response;
        }
    }
}

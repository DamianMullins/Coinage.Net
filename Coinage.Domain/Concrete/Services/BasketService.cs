using System;
using System.Linq;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Concrete.Services
{
    public class BasketService : IBasketService
    {
        private readonly IRepository<Basket> _basketRepository;

        public BasketService(IRepository<Basket> basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public Basket GetBasket(int id)
        {
            return _basketRepository.GetById(id);
        }

        public Basket GetCustomerBasket(int customerId)
        {
            return _basketRepository.Table.FirstOrDefault(b => b.CustomerId == customerId);
        }

        public void AddToCart(Basket basket, Product product, int quantity)
        {
            if (basket == null)
            {
                throw new ArgumentNullException("basket");
            }
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }
            if (quantity <= 0)
            {
                return;
            }

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(b => b.ProductId == product.Id);
            if (basketItem != null)
            {
                // Existing item
                basketItem.Quantity += quantity;
                basketItem.ModifiedOn = DateTime.Now;
                this.Update(basket);
            }
            else
            {
                // New item
                basketItem = new BasketItem
                {
                    Quantity = quantity,
                    ProductId = product.Id,
                    CreatedOn = DateTime.Now
                };
                basket.BasketItems.Add(basketItem);
                this.Update(basket);
            }
        }

        public EntityActionResponse Update(Basket basket)
        {
            var response = new EntityActionResponse();
            if (basket != null)
            {
                try
                {
                    basket.ModifiedOn = DateTime.Now;
                    _basketRepository.Update(basket);
                    response.Successful = true;
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                }
            }
            else
            {
                response.Exception = new ArgumentNullException("basket");
            }
            return response;
        }

        public EntityActionResponse Create(Basket basket)
        {
            var response = new EntityActionResponse();
            if (basket != null)
            {
                try
                {
                    basket.CreatedOn = DateTime.Now;
                    _basketRepository.Insert(basket);
                    response.Successful = true;
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                }
            }
            else
            {
                response.Exception = new ArgumentNullException("basket");
            }
            return response;
        }
    }
}

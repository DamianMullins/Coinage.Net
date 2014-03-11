using Coinage.Domain.Abstract;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System;
using System.Linq;

namespace Coinage.Domain.Concrete.Services
{
    public class BasketService : IBasketService
    {
        private readonly IWorkContext _workContext;
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<BasketItem> _basketItemRepository;

        public BasketService(IWorkContext workContext, IRepository<Basket> basketRepository, IRepository<BasketItem> basketItemRepository)
        {
            _workContext = workContext;
            _basketRepository = basketRepository;
            _basketItemRepository = basketItemRepository;
        }

        public Basket GetBasket(int id)
        {
            return _basketRepository.GetById(id);
        }

        public Basket GetCustomerBasket()
        {
            Customer customer = _workContext.CurrentCustomer;
            Basket basket = _basketRepository.Table.FirstOrDefault(b => b.CustomerId == customer.Id);

            if (basket == null)
            {
                // Create new Basket
                basket = new Basket { CustomerId = customer.Id };
                EntityActionResponse response = Create(basket);

                if (!response.Successful)
                {
                    throw new Exception("Could not create basket.", response.Exception);
                }
            }

            return basket;
        }

        public EntityActionResponse AddProductToBasket(Basket basket, Product product, int quantity)
        {
            if (basket == null) return new EntityActionResponse { Exception = new ArgumentNullException("basket") };
            if (product == null) return new EntityActionResponse { Exception = new ArgumentNullException("product") };
            if (quantity <= 0) return new EntityActionResponse { Exception = new Exception("Quantity less than one") };

            try
            {
                BasketItem basketItem = basket.BasketItems.FirstOrDefault(b => b.ProductId == product.Id);
                if (basketItem != null)
                {
                    // Existing item
                    basketItem.Quantity += quantity;
                    basketItem.ModifiedOn = DateTime.Now;
                    return Update(basket);
                }
                else
                {
                    // New item
                    basketItem = new BasketItem
                    {
                        Quantity = quantity,
                        ProductId = product.Id
                    };
                    basket.BasketItems.Add(basketItem);
                    return Update(basket);
                }

            }
            catch (Exception exception)
            {
                return new EntityActionResponse { Exception = exception };
            }
        }

        public EntityActionResponse UpdateProductInBasket(int basketItemId, int productId, int quantity)
        {
            try
            {
                Basket basket = GetCustomerBasket();
                BasketItem basketItem = _basketItemRepository.GetById(basketItemId);

                if (basketItem == null) throw new NullReferenceException("Basket Item was not found");

                // Verify that we are updating the correct basket
                if (!basket.BasketItems.Contains(basketItem)) throw new Exception("Basket Item was not found");

                if (quantity > 0)
                {
                    // Update item
                    basketItem.Quantity = quantity;
                    basketItem.ModifiedOn = DateTime.Now;
                    _basketItemRepository.Update(basketItem);
                }
                else
                {
                    // Delete item
                    _basketItemRepository.Delete(basketItem);
                }
                return new EntityActionResponse { Successful = true };
            }
            catch (Exception exception)
            {
                return new EntityActionResponse { Exception = exception };
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

using Coinage.Domain.Abstract;
using Coinage.Domain.Abstract.Authentication;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System;
using System.Linq;

namespace Coinage.Domain.Concrete.Services
{
    public class BasketService : IBasketService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<BasketItem> _basketItemRepository;

        public BasketService(IAuthenticationService authenticationService, IRepository<Basket> basketRepository, IRepository<BasketItem> basketItemRepository)
        {
            _authenticationService = authenticationService;
            _basketRepository = basketRepository;
            _basketItemRepository = basketItemRepository;
        }

        public Basket GetBasket(int id)
        {
            return _basketRepository.GetById(id);
        }

        public Basket GetCustomerBasket()
        {
            Customer customer = _authenticationService.CurrentCustomer;
            Basket basket = _basketRepository.Table.FirstOrDefault(b => b.CustomerId == customer.Id);

            if (basket == null)
            {
                // Create new Basket
                basket = new Basket { CustomerId = customer.Id };
                EntityActionResponse response = Create(basket);

                if (!response.Success)
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
                //BasketItem basketItem = _basketItemRepository.GetById(basketItemId);
                BasketItem basketItem = basket.BasketItems.FirstOrDefault(bi => bi.Id == basketItemId);

                if (basketItem == null) throw new NullReferenceException("Basket Item was not found");

                if (quantity > 0)
                {
                    // Update item
                    basketItem.Quantity = quantity;
                    _basketItemRepository.Update(basketItem);
                }
                else
                {
                    // Delete item
                    _basketItemRepository.Delete(basketItem);
                }

                _basketRepository.Update(basket);
                return new EntityActionResponse();
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
                    _basketRepository.Update(basket);
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

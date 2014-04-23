using System.Threading.Tasks;
using Coinage.Domain.Authentication;
using System;
using System.Linq;
using Coinage.Domain.Data;
using Coinage.Domain.Entites;
using Coinage.Domain.Models;

namespace Coinage.Domain.Services
{
    public class BasketService : IBasketService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepositoryAsync<Basket> _basketRepository;
        private readonly IRepositoryAsync<BasketItem> _basketItemRepository;

        public BasketService(IAuthenticationService authenticationService, IRepositoryAsync<Basket> basketRepository, IRepositoryAsync<BasketItem> basketItemRepository)
        {
            _authenticationService = authenticationService;
            _basketRepository = basketRepository;
            _basketItemRepository = basketItemRepository;
        }

        public EntityActionResponse Update(Basket basket)
        {
            if (basket == null) return new EntityActionResponse { Exception = new ArgumentNullException("basket") };

            var response = new EntityActionResponse();
            try
            {
                _basketRepository.Update(basket);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public EntityActionResponse Create(Basket basket)
        {
            if (basket == null) return new EntityActionResponse { Exception = new ArgumentNullException("basket") };

            var response = new EntityActionResponse();
            try
            {
                _basketRepository.Insert(basket);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<EntityActionResponse> UpdateAsync(Basket basket)
        {
            if (basket == null) return new EntityActionResponse { Exception = new ArgumentNullException("basket") };

            var response = new EntityActionResponse();
            try
            {
                await _basketRepository.UpdateAsync(basket);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<EntityActionResponse> CreateAsync(Basket basket)
        {
            if (basket == null) return new EntityActionResponse { Exception = new ArgumentNullException("basket") };

            var response = new EntityActionResponse();
            try
            {
                await _basketRepository.InsertAsync(basket);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<Basket> GetBasketAsync(int id)
        {
            return await _basketRepository.FindAsync(b => b.Id == id);
        }

        public async Task<Basket> GetCustomerBasketAsync()
        {
            Customer customer = _authenticationService.CurrentCustomer;
            Basket basket = await _basketRepository.FindAsync(b => b.CustomerId == customer.Id);

            if (basket == null)
            {
                basket = new Basket { CustomerId = customer.Id };
                EntityActionResponse response = await CreateAsync(basket);

                if (!response.Success) throw new Exception("Could not create basket.", response.Exception);
            }
            return basket;
        }

        public async Task<EntityActionResponse> AddProductToBasketAsync(Basket basket, Product product, int quantity)
        {
            if (basket == null) return new EntityActionResponse { Exception = new ArgumentNullException("basket") };
            if (product == null) return new EntityActionResponse { Exception = new ArgumentNullException("product") };
            if (quantity <= 0) return new EntityActionResponse { Exception = new Exception("Quantity less than one") };

            try
            {
                BasketItem basketItem = basket.BasketItems.FirstOrDefault(b => b.ProductId == product.Id);
                if (basketItem != null)
                {
                    basketItem.Quantity += quantity;
                }
                else
                {
                    basket.BasketItems.Add(new BasketItem
                    {
                        Quantity = quantity,
                        ProductId = product.Id
                    });
                }
                return await UpdateAsync(basket);
            }
            catch (Exception exception)
            {
                return new EntityActionResponse { Exception = exception };
            }
        }

        public async Task<EntityActionResponse> UpdateProductInBasketAsync(int basketItemId, int productId, int quantity)
        {
            try
            {
                Basket basket = await GetCustomerBasketAsync();
                if (basket == null) return new EntityActionResponse { Exception = new ArgumentException("Basket was not found") };

                BasketItem basketItem = basket.BasketItems.FirstOrDefault(bi => bi.Id == basketItemId);
                if (basketItem == null) throw new NullReferenceException("Basket Item was not found");

                if (quantity > 0)
                {
                    basketItem.Quantity = quantity;
                    await _basketItemRepository.UpdateAsync(basketItem);
                }
                else
                {
                    _basketItemRepository.Delete(basketItem);
                }

                await _basketRepository.UpdateAsync(basket);
                return new EntityActionResponse();
            }
            catch (Exception exception)
            {
                return new EntityActionResponse { Exception = exception };
            }
        }
    }
}

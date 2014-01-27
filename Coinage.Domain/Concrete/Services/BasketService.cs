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
                response.Exception = new Exception("Basket was null");
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
                response.Exception = new Exception("Basket was null");
            }
            return response;
        }
    }
}

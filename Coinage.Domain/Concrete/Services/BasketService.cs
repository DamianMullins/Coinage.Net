using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Concrete.Services
{
    public class BasketService  :IBasketService
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
    }
}

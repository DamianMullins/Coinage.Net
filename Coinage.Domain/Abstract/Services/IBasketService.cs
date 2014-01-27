using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Abstract.Services
{
    public interface IBasketService
    {
        Basket GetBasket(int id);
    }
}

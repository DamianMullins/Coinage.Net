using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Coinage.Domain.Entites
{
    public class Basket : EditableEntity
    {
        //private ICollection<BasketItem> _basketItems;

        public int CustomerId { get; set; }

        #region Navigation Properties

        public virtual ICollection<BasketItem> BasketItems { get; private set; }

        #endregion

        public Basket()
        {
            BasketItems = new Collection<BasketItem>();
        }

        #region Helper Methods

        public int TotalItems
        {
            get
            {
                if (BasketItems.Any())
                {
                    return BasketItems.Sum(item => item.Quantity);
                }
                return 0;
            }
        }

        public decimal TotalAmount
        {
            get
            {
                if (BasketItems.Any())
                {
                    return BasketItems.Sum(item => item.Product.Price * item.Quantity);
                }
                return 0m;
            }
        }

        /// <summary>
        /// Added for tests. TODO: remove or replace
        /// </summary>
        /// <param name="item"></param>
        public void AddBasketItem(BasketItem item)
        {
            BasketItems.Add(item);
        }

        #endregion
    }
}

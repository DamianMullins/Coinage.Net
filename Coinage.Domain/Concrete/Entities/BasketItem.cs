using System;

namespace Coinage.Domain.Concrete.Entities
{
    public class BasketItem : EditableEntity
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        #region Navigational Properties

        public virtual Basket Basket { get; set; }
        public virtual Product Product { get; set; }

        #endregion

        #region Helper Methods

        public decimal SubTotal
        {
            get { return Product.Price * Quantity; }
        }

        #endregion
    }
}

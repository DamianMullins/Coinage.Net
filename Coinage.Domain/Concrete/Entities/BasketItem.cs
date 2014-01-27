using System;

namespace Coinage.Domain.Concrete.Entities
{
    public class BasketItem : EditableEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        #region Navigational Properties

        public virtual Product Product { get; set; }

        #endregion
    }
}

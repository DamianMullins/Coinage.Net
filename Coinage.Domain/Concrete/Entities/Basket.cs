﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Coinage.Domain.Concrete.Entities
{
    public class Basket : EditableEntity
    {
        //private ICollection<BasketItem> _basketItems;

        public int CustomerId { get; set; }

        #region Navigation Properties

        public virtual ICollection<BasketItem> BasketItems { get; protected internal set; }

        #endregion

        public Basket()
        {
            BasketItems = new Collection<BasketItem>();
        }

        #region Helper Methods

        public int TotalItems
        {
            get { return BasketItems.Sum(item => item.Quantity); }
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
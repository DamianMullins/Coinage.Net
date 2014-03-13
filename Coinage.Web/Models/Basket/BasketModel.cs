using System.Collections.Generic;

namespace Coinage.Web.Models.Basket
{
    public class BasketModel
    {
        public IList<BasketItemModel> Items { get; set; }
        public string Total { get; set; }

        public BasketModel()
        {
            Items = new List<BasketItemModel>();
        }

        public class BasketItemModel
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public string ItemName { get; set; }
            public string UnitPrice { get; set; }
            public string SubTotal { get; set; }
        }
    }
}

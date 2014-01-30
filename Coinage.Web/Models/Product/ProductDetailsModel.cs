using System;

namespace Coinage.Web.Models.Product
{
    public class ProductDetailsModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductPriceModel ProductPrice { get; set; }
        public AddToBasketModel AddToBasket { get; set; }

        public ProductDetailsModel()
        {
            ProductPrice = new ProductPriceModel();
            AddToBasket = new AddToBasketModel();
        }

        public class ProductPriceModel
        {
            public int ProductId { get; set; }
            public string Price { get { return PriceValue.ToString("0.00"); } }
            public decimal PriceValue { get; set; }
        }

        public class AddToBasketModel
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }

            public AddToBasketModel()
            {
                Quantity = 1;
            }
        }
    }
}

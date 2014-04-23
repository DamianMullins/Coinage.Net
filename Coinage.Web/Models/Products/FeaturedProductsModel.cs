using System.Collections.Generic;
using Coinage.Domain.Entites;

namespace Coinage.Web.Models.Products
{
    public class FeaturedProductsModel
    {
        public List<Product> Products { get; set; }

        public FeaturedProductsModel()
        {
            Products = new List<Product>();
        }
    }
}

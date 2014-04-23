using Coinage.Domain.Entites;
using System.Collections.Generic;

namespace Coinage.Web.Models.Products
{
    public class FeaturedProductsModel
    {
        public IEnumerable<Product> Products { get; set; }

        public FeaturedProductsModel()
        {
            Products = new List<Product>();
        }
    }
}

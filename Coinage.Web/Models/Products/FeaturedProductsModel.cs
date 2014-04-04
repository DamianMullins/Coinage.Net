using Coinage.Domain.Concrete.Entities;
using System.Collections.Generic;

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

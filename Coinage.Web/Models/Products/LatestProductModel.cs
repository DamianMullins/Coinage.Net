using Coinage.Domain.Entites;
using System.Collections.Generic;

namespace Coinage.Web.Models.Products
{
    public class LatestProductModel
    {
        public IEnumerable<Product> Products { get; set; }

        public LatestProductModel()
        {
            Products = new List<Product>();
        }
    }
}

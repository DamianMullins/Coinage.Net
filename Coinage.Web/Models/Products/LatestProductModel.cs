using System.Collections.Generic;
using Coinage.Domain.Entites;

namespace Coinage.Web.Models.Products
{
    public class LatestProductModel
    {
        public List<Product> Products { get; set; }

        public LatestProductModel()
        {
            Products = new List<Product>();
        }
    }
}

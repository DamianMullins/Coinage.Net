using Coinage.Domain.Concrete.Entities;
using System.Collections.Generic;

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

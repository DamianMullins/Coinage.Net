using System.Collections.Generic;

namespace Coinage.Web.Models.Product
{
    public class FeaturedProductsModel
    {
        public List<Domain.Concrete.Entities.Product> Products { get; set; }

        public FeaturedProductsModel()
        {
            Products = new List<Domain.Concrete.Entities.Product>();
        }
    }
}

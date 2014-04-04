using System.Collections.Generic;

namespace Coinage.Web.Models.Product
{
    public class LatestProductModel
    {
        public List<Domain.Concrete.Entities.Product> Products { get; set; }

        public LatestProductModel()
        {
            Products = new List<Domain.Concrete.Entities.Product>();
        }
    }
}

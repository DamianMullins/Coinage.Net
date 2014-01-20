using Coinage.Domain.Abstract.Entites;
using System;

namespace Coinage.Domain.Concrete.Entities
{
    public class Product : ITimestamps
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public bool IsFeatured { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}

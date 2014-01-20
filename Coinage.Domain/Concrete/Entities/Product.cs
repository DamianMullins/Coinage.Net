using System;

namespace Coinage.Domain.Concrete.Entities
{
    public class Product : EditableEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
    }
}

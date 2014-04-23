using System;

namespace Coinage.Domain.Entites
{
    public class Product : EditableEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
    }
}

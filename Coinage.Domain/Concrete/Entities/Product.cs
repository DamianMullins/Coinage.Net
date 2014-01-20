using Coinage.Domain.Abstract.Entites;
using System;

namespace Coinage.Domain.Concrete.Entities
{
    public class Product : EditableEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public bool IsFeatured { get; set; }
    }
}

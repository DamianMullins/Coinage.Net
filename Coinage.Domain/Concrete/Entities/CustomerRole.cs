using System;

namespace Coinage.Domain.Concrete.Entities
{
    public class CustomerRole : Entity
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}

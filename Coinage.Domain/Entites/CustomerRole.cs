using System;

namespace Coinage.Domain.Entites
{
    public class CustomerRole : Entity
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}

using System;

namespace Coinage.Domain.Concrete.Entities
{
    public class Customer : EditableEntity
    {
        public Guid CustomerGuid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool Active { get; set; }

        public Customer()
        {
            CustomerGuid = Guid.NewGuid();
        }
    }
}

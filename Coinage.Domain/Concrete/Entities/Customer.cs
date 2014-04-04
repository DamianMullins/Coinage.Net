using System;
using System.Collections.Generic;

namespace Coinage.Domain.Concrete.Entities
{
    public class Customer : EditableEntity
    {
        public Guid CustomerGuid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }

        #region Navigation Properties

        public virtual ICollection<CustomerRole> Roles { get; set; }

        #endregion

        public Customer()
        {
            CustomerGuid = Guid.NewGuid();
            Roles = new HashSet<CustomerRole>();
        }
    }
}

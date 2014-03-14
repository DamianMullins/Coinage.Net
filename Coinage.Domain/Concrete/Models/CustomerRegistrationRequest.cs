using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Concrete.Models
{
    public class CustomerRegistrationRequest
    {
        public Customer Customer { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public CustomerRegistrationRequest(Customer customer, string email, string password)
        {
            Customer = customer;
            Email = email;
            Password = password;
        }
    }
}

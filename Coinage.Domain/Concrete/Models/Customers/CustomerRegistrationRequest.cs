using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Concrete.Models.Customers
{
    public class CustomerRegistrationRequest
    {
        public Customer Customer { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public CustomerRegistrationRequest(Customer customer, string email, string password, string firstName, string lastName, string phone)
        {
            Customer = customer;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }
    }
}

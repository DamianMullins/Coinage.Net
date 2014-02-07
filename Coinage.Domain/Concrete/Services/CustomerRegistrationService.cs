using Coinage.Domain.Abstract.Security;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Extensions;

namespace Coinage.Domain.Concrete.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;

        public CustomerRegistrationService(ICustomerService customerService, IEncryptionService encryptionService)
        {
            _customerService = customerService;
            _encryptionService = encryptionService;
        }

        public bool ValidateCustomer(string email, string password)
        {
            Customer customer = _customerService.GetCustomerByEmail(email);

            if (customer == null || !customer.Active)
            {
                return false;
            }

            if (!customer.IsRegistered())
            {
                return false;
            }

            string passwordHash = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt);
            bool isValid = passwordHash == customer.Password;

            if (isValid)
            {
                // TODO: Implement & update last login then update customer
            }
            return isValid;
        }
    }
}

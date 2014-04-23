using Coinage.Domain.Data;
using Coinage.Domain.Entites;
using Coinage.Domain.Enums;
using Coinage.Domain.Extensions;
using Coinage.Domain.Models;
using Coinage.Domain.Models.Customers;
using Coinage.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coinage.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepositoryAsync<Customer> _customerRepository;
        private readonly IRepositoryAsync<CustomerRole> _customerRoleRepository;
        private readonly IEncryptionService _encryptionService;

        public CustomerService(IRepositoryAsync<Customer> customerRepository, IRepositoryAsync<CustomerRole> customerRoleRepository, IEncryptionService encryptionService)
        {
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _encryptionService = encryptionService;
        }
        
        public EntityActionResponse Update(Customer customer)
        {
            if (customer == null) return new EntityActionResponse { Exception = new ArgumentNullException("customer") };

            var response = new EntityActionResponse();
            try
            {
                _customerRepository.Update(customer);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public EntityActionResponse Create(Customer customer)
        {
            if (customer == null) return new EntityActionResponse { Exception = new ArgumentNullException("customer") };

            var response = new EntityActionResponse();
            try
            {
                _customerRepository.Insert(customer);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<EntityActionResponse> UpdateAsync(Customer customer)
        {
            if (customer == null) return new EntityActionResponse { Exception = new ArgumentNullException("customer") };

            var response = new EntityActionResponse();
            try
            {
                await _customerRepository.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<EntityActionResponse> CreateAsync(Customer customer)
        {
            if (customer == null) return new EntityActionResponse { Exception = new ArgumentNullException("customer") };

            var response = new EntityActionResponse();
            try
            {
                await _customerRepository.InsertAsync(customer);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.FindAsync(c => c.Id == id);
        }

        public Customer GetCustomerByGuid(Guid customerGuid)
        {
            return _customerRepository.Find(c => c.CustomerGuid == customerGuid);
        }

        public async Task<Customer> GetCustomerByGuidAsync(Guid customerGuid)
        {
            return await _customerRepository.FindAsync(c => c.CustomerGuid == customerGuid);
        }

        public Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email", "Email cannot be empty");

            return _customerRepository.Find(c => c.Email == email);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email", "Email cannot be empty");

            return await _customerRepository.FindAsync(c => c.Email == email);
        }

        public Customer InsertGuestCustomer()
        {
            CustomerRole guestRole = GetCustomerRole(CustomerRoles.Guest);
            if (guestRole == null) throw new Exception("Guest role was not found");

            var customer = new Customer { Active = true };

            customer.Roles.Add(guestRole);

            EntityActionResponse response = Create(customer);
            return response.Success ? customer : null;
        }

        public async Task<Customer> InsertGuestCustomerAsync()
        {
            CustomerRole guestRole = await GetCustomerRoleAsync(CustomerRoles.Guest);
            if (guestRole == null) throw new Exception("Guest role was not found");

            var customer = new Customer { Active = true };

            customer.Roles.Add(guestRole);

            EntityActionResponse response = await CreateAsync(customer);
            return response.Success ? customer : null;
        }

        public async Task<EntityActionResponse> RegisterCustomerAsync(CustomerRegistrationRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (request.Customer == null) throw new ArgumentException("Requested customer was not found", "request");

            var validRequest = ValidateCustomerRegistrationRequest(request);
            if (!validRequest.Success)
            {
                return validRequest;
            }

            // We have a valid request; continue
            request.Customer.Email = request.Email;

            string saltKey = _encryptionService.CreateSaltKey(5);
            request.Customer.PasswordSalt = saltKey;
            request.Customer.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey);

            request.Customer.FirstName = request.FirstName;
            request.Customer.LastName = request.LastName;
            request.Customer.Phone = request.Phone;
            request.Customer.Active = true;

            // Add the customer to the registered role
            CustomerRole registeredRole = await GetCustomerRoleAsync(CustomerRoles.Registered);
            if (registeredRole == null) throw new Exception("'Registered' role could not be loaded");
            request.Customer.Roles.Add(registeredRole);

            // Remove the customer from the guests role
            CustomerRole guestRole = request.Customer.Roles.FirstOrDefault(role => role.Id == (int)CustomerRoles.Guest);
            if (guestRole != null) request.Customer.Roles.Remove(guestRole);

            // Update rather than create record as we are working with a pre exiting customer record
            return await UpdateAsync(request.Customer);
        }

        private CustomerRole GetCustomerRole(CustomerRoles role)
        {
            return _customerRoleRepository.Find(cr => cr.Id == (int)role);
        }

        private async Task<CustomerRole> GetCustomerRoleAsync(CustomerRoles role)
        {
            return await _customerRoleRepository.FindAsync(cr => cr.Id == (int)role);
        }

        private EntityActionResponse ValidateCustomerRegistrationRequest(CustomerRegistrationRequest request)
        {
            var response = new EntityActionResponse();

            if (request.Customer.IsRegistered())
            {
                response.Errors.Add("Customer is already registered.");
            }

            Customer customerFromDb = GetCustomerByEmail(request.Email);
            if (customerFromDb != null)
            {
                response.Errors.Add("The specified email already exists.");
            }

            return response;
        }
    }
}

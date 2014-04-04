using System.Data;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Security;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System;
using System.Linq;
using Coinage.Domain.Concrete.Enums;
using Coinage.Domain.Concrete.Extensions;
using Coinage.Domain.Concrete.Models;

namespace Coinage.Domain.Concrete.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly IEncryptionService _encryptionService;

        public CustomerService(IRepository<Customer> customerRepository, IRepository<CustomerRole> customerRoleRepository, IEncryptionService encryptionService)
        {
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _encryptionService = encryptionService;
        }

        public Customer GetCustomerById(int id)
        {
            return _customerRepository.GetById(id);
        }

        public Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty) return null;

            Customer customer = _customerRepository.Table
                .FirstOrDefault(c => c.CustomerGuid == customerGuid);

            return customer;
        }

        public Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            Customer customer = _customerRepository.Table
                .FirstOrDefault(c => c.Email == email);

            return customer;
        }

        public Customer InsertGuestCustomer()
        {
            var customer = new Customer
            {
                Active = true
            };

            CustomerRole guestRole = GetCustomerRole(CustomerRoleName.Guest);
            if (guestRole == null)
            {
                throw new Exception("Guests role could not be loaded");
            }
            customer.Roles.Add(guestRole);

            EntityActionResponse response = Create(customer);
            return response.Success ? customer : null;
        }

        public EntityActionResponse RegisterCustomer(CustomerRegistrationRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (request.Customer == null) throw new ArgumentNullException("customer is null");

            var response = new EntityActionResponse();

            if (request.Customer.IsRegistered())
            {
                response.Errors.Add("Customer is already registered.");
            }

            if (GetCustomerByEmail(request.Email) != null)
            {
                response.Errors.Add("The specified email already exists.");
            }

            if (!response.Success)
            {
                return response;
            }

            request.Customer.Email = request.Email;

            string saltKey = _encryptionService.CreateSaltKey(5);
            request.Customer.PasswordSalt = saltKey;
            request.Customer.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey);

            request.Customer.FirstName = request.FirstName;
            request.Customer.LastName = request.LastName;
            request.Customer.Phone = request.Phone;
            request.Customer.Active = true;
            
            CustomerRole registeredRole = GetCustomerRoleByName(CustomerRoleName.Registered);
            if (registeredRole == null) throw new Exception("'Registered' role could not be loaded");
            request.Customer.Roles.Add(registeredRole);

            CustomerRole guestRole = request.Customer.Roles.FirstOrDefault(role => role.Id == (int)CustomerRoleName.Guest);
            if (guestRole != null)
            {
                request.Customer.Roles.Remove(guestRole);
            }

            _customerRepository.Update(request.Customer);

            return response;
        }

        public CustomerRole GetCustomerRoleByName(CustomerRoleName roleName)
        {
            // TODO: caching
            return _customerRoleRepository.Table.OrderBy(cr => cr.Id).FirstOrDefault(cr => cr.Id == (int)roleName);
        }

        public EntityActionResponse Update(Customer customer)
        {
            var response = new EntityActionResponse();
            if (customer != null)
            {
                try
                {
                    _customerRepository.Update(customer);
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                }
            }
            else
            {
                response.Exception = new ArgumentNullException("customer");
            }
            return response;
        }

        public EntityActionResponse Create(Customer customer)
        {
            var response = new EntityActionResponse();
            if (customer != null)
            {
                try
                {
                    _customerRepository.Insert(customer);
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                }
            }
            else
            {
                response.Exception = new ArgumentNullException("customer");
            }
            return response;
        }

        #region Customer roles

        public virtual CustomerRole GetCustomerRole(CustomerRoleName role)
        {
            return _customerRoleRepository.Table.OrderBy(cr => cr.Id).FirstOrDefault(cr => cr.Id == (int)role);
        }

        #endregion
    }
}

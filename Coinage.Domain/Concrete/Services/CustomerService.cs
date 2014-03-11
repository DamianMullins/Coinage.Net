using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System;
using System.Linq;
using Coinage.Domain.Concrete.Enums;

namespace Coinage.Domain.Concrete.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;

        public CustomerService(IRepository<Customer> customerRepository, IRepository<CustomerRole> customerRoleRepository)
        {
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
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

            CustomerRole guestRole = GetCustomerRole(CustomerRoleNames.Guests);
            if (guestRole == null)
            {
                throw new Exception("Guests role could not be loaded");
            }
            customer.Roles.Add(guestRole);

            EntityActionResponse response = Create(customer);
            return response.Successful ? customer : null;
        }

        public EntityActionResponse Update(Customer customer)
        {
            var response = new EntityActionResponse();
            if (customer != null)
            {
                try
                {
                    customer.ModifiedOn = DateTime.Now;
                    _customerRepository.Update(customer);
                    response.Successful = true;
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
                    response.Successful = true;
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

        public virtual CustomerRole GetCustomerRole(CustomerRoleNames role)
        {
            return _customerRoleRepository.Table.OrderBy(cr => cr.Id).FirstOrDefault(cr => cr.Id == (int)role);
        }

        #endregion
    }
}

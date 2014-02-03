using System.Linq;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System;

namespace Coinage.Domain.Concrete.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer GetCustomerById(int id)
        {
            return _customerRepository.GetById(id);
        }

        public Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty) return null;

            Customer customer = _customerRepository.Table
                .Where(c => c.CustomerGuid == customerGuid)
                .OrderByDescending(c => c.CreatedOn)
                .FirstOrDefault();

            return customer;
        }

        public Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            Customer customer = _customerRepository.Table
                .Where(c => c.Email == email)
                .OrderByDescending(c => c.CreatedOn)
                .FirstOrDefault();

            return customer;
        }

        public Customer InsertGuestCustomer()
        {
            var customer = new Customer
            {
                Active = true
            };

            // TODO: Implement Customer roles & retrieve guest role
            //customer.Roles.Add

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
                    customer.CreatedOn = DateTime.Now;
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
    }
}

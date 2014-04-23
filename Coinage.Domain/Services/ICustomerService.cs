using Coinage.Domain.Entites;
using Coinage.Domain.Models;
using Coinage.Domain.Models.Customers;
using System;
using System.Threading.Tasks;

namespace Coinage.Domain.Services
{
    public interface ICustomerService : IEditableServiceAsync<Customer>
    {
        /// <summary>
        /// Asynchronously get a Customer by its primary key.
        /// </summary>
        /// <param name="id">Primary key of the Customer.</param>
        /// <returns>A Customer object.</returns>
        Task<Customer> GetCustomerByIdAsync(int id);

        /// <summary>
        /// Get a customer by Guid.
        /// </summary>
        /// <param name="customerGuid">Customer Guid.</param>
        /// <returns>A Customer object.</returns>
        Customer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// Asynchronously get a customer by Guid.
        /// </summary>
        /// <param name="customerGuid">Customer Guid.</param>
        /// <returns>A Customer object.</returns>
        Task<Customer> GetCustomerByGuidAsync(Guid customerGuid);

        /// <summary>
        /// Asynchronously get a Customer by their email address.
        /// </summary>
        /// <param name="email">Email to look up.</param>
        /// <returns>A Customer object.</returns>
        Task<Customer> GetCustomerByEmailAsync(string email);

        /// <summary>
        /// Insert a guest Customer.
        /// </summary>
        /// <returns>A Customer object.</returns>
        Customer InsertGuestCustomer();

        /// <summary>
        /// Asynchronously insert a guest Customer.
        /// </summary>
        /// <returns>A Customer object.</returns>
        Task<Customer> InsertGuestCustomerAsync();

        Task<EntityActionResponse> RegisterCustomerAsync(CustomerRegistrationRequest request);
    }
}

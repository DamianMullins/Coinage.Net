﻿using System;
using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Abstract.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomerService : IEditableService<Customer>
    {
        /// <summary>
        /// Get a Customer by its primary key.
        /// </summary>
        /// <param name="id">Primary key of the Customer.</param>
        /// <returns>A Customer object.</returns>
        Customer GetCustomerById(int id);

        /// <summary>
        /// Gets a customer by Guid.
        /// </summary>
        /// <param name="customerGuid">Customer Guid.</param>
        /// <returns>A Customer object.</returns>
        Customer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// Get a Customer by their email address.
        /// </summary>
        /// <param name="email">Email to look up.</param>
        /// <returns>A Customer object.</returns>
        Customer GetCustomerByEmail(string email);

        /// <summary>
        /// Insert a guest Customer.
        /// </summary>
        /// <returns>A Customer object.</returns>
        Customer InsertGuestCustomer();
    }
}
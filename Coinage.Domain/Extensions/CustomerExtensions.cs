using Coinage.Domain.Entites;
using Coinage.Domain.Enums;
using System;
using System.Linq;

namespace Coinage.Domain.Extensions
{
    public static class CustomerExtensions
    {
        /// <summary>
        /// Gets a value indicating whether customer is in a certain customer role
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="customerRoleName"></param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsInCustomerRole(this Customer customer, CustomerRoles customerRoleName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null) throw new ArgumentNullException("customer");

            var result = customer.Roles
                .FirstOrDefault(cr => (!onlyActiveCustomerRoles || cr.Active) && (cr.Name == customerRoleName.ToString())) != null;

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customer is registered
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, CustomerRoles.Registered, onlyActiveCustomerRoles);
        }
    }
}

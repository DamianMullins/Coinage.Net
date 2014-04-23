using System.Threading.Tasks;
using Coinage.Domain.Entites;

namespace Coinage.Domain.Authentication
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// 
        /// </summary>
        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// Sign a Customer in.
        /// </summary>
        /// <param name="customer">Customer to sign in.</param>
        /// <param name="createPersistentCookie">Create a persistent cookie?</param>
        void SignIn(Customer customer, bool createPersistentCookie);

        /// <summary>
        /// Sign the current Customer out.
        /// </summary>
        void SignOut();

        ///// <summary>
        ///// Get the current authenticated Customer.
        ///// </summary>
        ///// <returns>A Customer object.</returns>
        //Customer GetAuthenticatedCustomer();

        ///// <summary>
        ///// Asynchronously get the current authenticated Customer.
        ///// </summary>
        ///// <returns>A Customer object.</returns>
        //Task<Customer> GetAuthenticatedCustomerAsync();
    }
}

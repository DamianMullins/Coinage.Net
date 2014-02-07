using Coinage.Domain.Concrete;

namespace Coinage.Domain.Abstract.Services
{
    public interface ICustomerRegistrationService
    {
        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        bool ValidateCustomer(string email, string password);

        ///// <summary>
        ///// Register customer
        ///// </summary>
        ///// <param name="request">Request</param>
        ///// <returns>Result</returns>
        //EntityActionResponse RegisterCustomer(CustomerRegistrationRequest request);
    }
}

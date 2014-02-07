using System;
using System.Web;
using System.Web.Security;
using Coinage.Domain.Abstract.Authentication;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Extensions;

namespace Coinage.Domain.Concrete.Services.Authentication
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private readonly ICustomerService _customerService;
        private readonly HttpContextBase _httpContext;

        private Customer _cachedCustomer;

        public FormsAuthenticationService(ICustomerService customerService, HttpContextBase httpContext)
        {
            _customerService = customerService;
            _httpContext = httpContext;
        }

        public void SignIn(Customer customer, bool createPersistentCookie)
        {
            DateTime now = DateTime.Now;
            var ticket = new FormsAuthenticationTicket(1, 
                customer.Email, 
                now, 
                now.Add(FormsAuthentication.Timeout),
                createPersistentCookie, 
                customer.Email, 
                FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }

            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedCustomer = customer;
        }

        public void SignOut()
        {
            _cachedCustomer = null;
            FormsAuthentication.SignOut();
        }

        public Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
            {
                return _cachedCustomer;
            }

            if (_httpContext == null || !_httpContext.Request.IsAuthenticated || !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);

            if (customer != null && customer.Active && customer.IsRegistered())
            {
                _cachedCustomer = customer;
            }

            return _cachedCustomer;
        }

        private Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null) throw new ArgumentNullException("ticket");

            string email = ticket.UserData;

            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            Customer customer = _customerService.GetCustomerByEmail(email);
            return customer;
        }
    }
}

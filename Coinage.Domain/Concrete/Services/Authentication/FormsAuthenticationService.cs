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
        private const string CustomerCookieName = "coinage.customer";

        private readonly ICustomerService _customerService;
        private readonly HttpContextBase _httpContext;

        private Customer _currentCustomer;
        public Customer CurrentCustomer
        {
            get
            {
                if (_currentCustomer != null) return _currentCustomer;

                Customer customer = GetAuthenticatedCustomer();

                // Load guest Customer
                if (customer == null || !customer.Active)
                {
                    HttpCookie customerCookie = GetCustomerCookie();
                    if (customerCookie != null && !string.IsNullOrEmpty(customerCookie.Value))
                    {
                        Guid customerGuid;
                        if (Guid.TryParse(customerCookie.Value, out customerGuid))
                        {
                            Customer customerFromCookie = _customerService.GetCustomerByGuid(customerGuid);
                            if (customerFromCookie != null && !customerFromCookie.IsRegistered())
                            {
                                customer = customerFromCookie;
                            }
                        }
                    }
                }

                // Create new guest Customer
                if (customer == null || !customer.Active)
                {
                    customer = _customerService.InsertGuestCustomer();
                }

                if (customer.Active)
                {
                    SetCustomerCookie(customer.CustomerGuid);
                    _currentCustomer = customer;
                }

                return _currentCustomer;
            }
            set
            {
                SetCustomerCookie(value.CustomerGuid);
                _currentCustomer = value;
            }
        }

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
            _currentCustomer = customer;
        }

        public void SignOut()
        {
            _currentCustomer = null;
            FormsAuthentication.SignOut();
        }

        public Customer GetAuthenticatedCustomer()
        {
            if (_currentCustomer != null)
            {
                return _currentCustomer;
            }

            if (_httpContext == null || !_httpContext.Request.IsAuthenticated || !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);

            if (customer != null && customer.Active && customer.IsRegistered())
            {
                _currentCustomer = customer;
            }

            return _currentCustomer;
        }

        private HttpCookie GetCustomerCookie()
        {
            return _httpContext == null ? null : _httpContext.Request.Cookies[CustomerCookieName];
        }

        private void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContext == null) return;

            var cookie = new HttpCookie(CustomerCookieName)
            {
                HttpOnly = true,
                Value = customerGuid.ToString(),
                Expires = customerGuid == Guid.Empty ? DateTime.Now.AddMonths(-1) : DateTime.Now.AddYears(1)
            };

            _httpContext.Response.Cookies.Remove(CustomerCookieName);
            _httpContext.Response.Cookies.Add(cookie);
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

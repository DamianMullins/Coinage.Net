using Coinage.Domain.Abstract;
using Coinage.Domain.Abstract.Authentication;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using System;
using System.Web;

namespace Coinage.Web.Framework
{
    public class WebWorkContext : IWorkContext
    {
        private const string CustomerCookieName = "coinage.customer";

        private readonly ICustomerService _customerService;
        private readonly IAuthenticationService _authenticationService;
        private readonly HttpContextBase _httpContext;

        private Customer _cachedCustomer;

        public WebWorkContext(ICustomerService customerService, IAuthenticationService authenticationService, HttpContextBase httpContext)
        {
            _customerService = customerService;
            _authenticationService = authenticationService;
            _httpContext = httpContext;
        }

        public Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null) return _cachedCustomer;

                Customer customer = _authenticationService.GetAuthenticatedCustomer();

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
                            if (customerFromCookie != null) // TODO: Implement customer extension methods && !customerByCookie.IsRegistered())
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
                    _cachedCustomer = customer;
                }

                return _cachedCustomer;
            }
            set
            {
                SetCustomerCookie(value.CustomerGuid);
                _cachedCustomer = value;
            }
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
    }
}

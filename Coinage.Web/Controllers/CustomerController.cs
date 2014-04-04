using Coinage.Domain.Abstract.Authentication;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Extensions;
using Coinage.Domain.Concrete.Models;
using Coinage.Web.Models;
using System.Web.Mvc;
using Coinage.Web.Models.Customer;

namespace Coinage.Web.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerService _customerService;

        public CustomerController(IAuthenticationService authenticationService, ICustomerService customerService)
        {
            _authenticationService = authenticationService;
            _customerService = customerService;
        }

        public ActionResult Register()
        {
            var model = new RegisterModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            if (_authenticationService.CurrentCustomer.IsRegistered())
            {
                // TODO
                // Already registered so create a new record
                _authenticationService.SignOut();
                _customerService.InsertGuestCustomer();
            }

            if (ModelState.IsValid)
            {
                Customer customer = _authenticationService.CurrentCustomer;

                var registrationRequest = new CustomerRegistrationRequest(customer, model.Email, model.Password, model.FirstName, model.LastName, model.Phone);
                CustomerRegistrationResult registrationResult = _customerService.RegisterCustomer(registrationRequest);

                if (registrationResult.Success)
                {
                    // TODO: Newsletter subscription

                    // TODO: Sign in

                    // TODO: Email notify

                    return RedirectToAction("RegisterResult");
                }
                else
                {
                    ErrorAlert(registrationResult.ErrorMessage);
                }
            }
            return View(model);
        }

        public ActionResult RegisterResult()
        {
            return View();
        }
    }
}

using Coinage.Domain.Authentication;
using Coinage.Domain.Entites;
using Coinage.Domain.Extensions;
using Coinage.Domain.Models;
using Coinage.Domain.Models.Customers;
using Coinage.Domain.Services;
using Coinage.Web.Models.Customers;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            if (_authenticationService.CurrentCustomer.IsRegistered()) return RedirectToAction("Index", "Home");

            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Customer customer = _authenticationService.CurrentCustomer;

                var registrationRequest = new CustomerRegistrationRequest(customer, model.Email, model.Password, model.FirstName, model.LastName, model.Phone);
                EntityActionResponse registrationResult = await _customerService.RegisterCustomerAsync(registrationRequest);

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

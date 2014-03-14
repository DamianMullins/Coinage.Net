using Coinage.Domain.Abstract.Authentication;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Extensions;
using Coinage.Web.Models.Customer;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private ICustomerService _customerService;

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
                // Already registered so create a new record
                _authenticationService.SignOut();
                _customerService.InsertGuestCustomer();
            }

            if (ModelState.IsValid)
            {
                Customer customer = _authenticationService.CurrentCustomer;
            }
        }
	}
}

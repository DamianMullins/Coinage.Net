using Coinage.Web.Models.Customer;
using FluentValidation;

namespace Coinage.Web.Validation.Customer
{
    public class CustomerRegistrationValidation : AbstractValidator<RegisterModel>
    {
        public CustomerRegistrationValidation()
        {
            RuleFor(cr => cr.ConfirmPassword).NotEmpty().WithMessage("Password is required.");
            RuleFor(cr => cr.ConfirmPassword).Equal(cr => cr.Password).WithMessage("The password and confirmation password do not match.");
        }
    }
}

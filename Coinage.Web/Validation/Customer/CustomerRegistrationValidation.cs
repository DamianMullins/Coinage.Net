using Coinage.Web.Models.Customer;
using FluentValidation;

namespace Coinage.Web.Validation.Customer
{
    public class CustomerRegistrationValidation : AbstractValidator<RegisterModel>
    {
        public CustomerRegistrationValidation()
        {
            RuleFor(cr => cr.Email).EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(cr => cr.Email).NotEmpty().WithMessage("Email is required.");

            RuleFor(cr => cr.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(cr => cr.Password).Equal(cr => cr.ConfirmPassword).WithMessage("The password and confirmation password do not match.");

            RuleFor(cr => cr.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(cr => cr.LastName).NotEmpty().WithMessage("Last name is required.");
        }
    }
}

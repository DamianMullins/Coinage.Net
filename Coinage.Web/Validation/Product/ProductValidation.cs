using FluentValidation;

namespace Coinage.Web.Validation.Product
{
    public class ProductValidation : AbstractValidator<Domain.Concrete.Entities.Product>
    {
        public ProductValidation()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(p => p.Name).Length(0, 500).WithMessage("Maximum length for name is 500 characters");

            RuleFor(p => p.Price).NotEmpty().WithMessage("Price is required");
        }
    }
}

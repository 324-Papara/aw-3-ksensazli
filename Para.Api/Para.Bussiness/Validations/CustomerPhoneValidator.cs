using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validations;

public class CustomerPhoneValidator : AbstractValidator<CustomerPhoneRequest>
{
    public CustomerPhoneValidator()
    {
        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .Length(3)
            .WithMessage("Country code must be 3 characters");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Length(10)
            .WithMessage("Phone number must be 10 digits");
    }
}
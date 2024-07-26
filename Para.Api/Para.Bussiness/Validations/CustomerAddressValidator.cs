using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validations;

public class CustomerAddressValidator : AbstractValidator<CustomerAddressRequest>
{
    public CustomerAddressValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .Length(1, 25)
            .WithMessage("Country must be between 1 and 25 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .Length(1, 25)
            .WithMessage("City must be between 1 and 25 characters");

        RuleFor(x => x.AddressLine)
            .NotEmpty()
            .MaximumLength(300)
            .WithMessage("Address can not be empty");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .Length(5)
            .WithMessage("Zip code must be 5 digits");
    }
}
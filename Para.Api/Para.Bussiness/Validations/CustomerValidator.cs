using FluentValidation;
using Para.Schema;
using Para.Data.Domain;

namespace Para.Bussiness.Validations;

public class CustomerValidator : AbstractValidator<CustomerRequest>
{
    public CustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 30)
            .WithMessage("First name must be between 2 and 30 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 30)
            .WithMessage("Last name must be between 2 and 30 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("E-mail address must be valid");

        RuleFor(x => x.IdentityNumber)
            .NotEmpty()
            .Length(11)
            .WithMessage("Id number must be 11 digits");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty();
    }
}
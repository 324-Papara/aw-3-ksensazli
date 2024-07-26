using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validations;

public class CustomerDetailValidator : AbstractValidator<CustomerDetailRequest>
{
    public CustomerDetailValidator()
    {
        RuleFor(x => x.FatherName)
            .NotEmpty()
            .Length(1, 20)
            .WithMessage("Father name must be entered");

        RuleFor(x => x.MotherName)
            .NotEmpty()
            .Length(1, 20)
            .WithMessage("Mother name must be entered");

        RuleFor(x => x.MonthlyIncome)
            .NotEmpty()
            .WithMessage("Monthly income must be entered");

        RuleFor(x => x.EducationStatus)
            .NotEmpty()
            .Length(1, 20)
            .WithMessage("Education status must be entered");

        RuleFor(x => x.Occupation)
            .NotEmpty()
            .Length(1, 30)
            .WithMessage("Occupation must be entered");
    }
}
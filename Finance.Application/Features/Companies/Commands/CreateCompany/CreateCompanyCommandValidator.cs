using FluentValidation;

namespace Finance.Application.Features.Companies.Commands.CreateCompany;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters.");
        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Company code is required.")
            .MaximumLength(50).WithMessage("Company code must not exceed 50 characters.");
        RuleFor(c => c.BaseCurrencyCode)
            .NotEmpty().WithMessage("Base currency code is required.")
            .Length(3).WithMessage("Base currency code must be exactly 3 characters.");
    }
}

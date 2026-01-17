using FluentValidation;

namespace Finance.Application.Features.Companies.Commands.UpdateCompany;

public sealed class UpdateCompanyCommandValidator
    : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.BaseCurrencyCode)
            .Length(3);
    }
}

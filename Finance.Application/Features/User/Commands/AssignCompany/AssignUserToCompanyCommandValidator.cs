using FluentValidation;

namespace Finance.Application.Features.User.Commands.AssignCompany;

public sealed class AssignUserToCompanyCommandValidator
    : AbstractValidator<AssignUserToCompanyCommand>
{
    public AssignUserToCompanyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
    }
}

using FluentValidation;

namespace Finance.Application.Features.User.Commands.CreateUser;

public sealed class CreateUserCompanyValidator
    : AbstractValidator<CreateUserCompanyDto>
{
    public CreateUserCompanyValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.RoleIds)
            .NotNull()
            .NotEmpty()
            .WithMessage("Company must have at least one role");

        RuleForEach(x => x.RoleIds)
            .NotEmpty();
    }
}

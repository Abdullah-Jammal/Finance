using FluentValidation;

namespace Finance.Application.Features.User.Commands.AssignRole;

public sealed class AssignUserRoleCommandValidator
    : AbstractValidator<AssignUserRoleCommand>
{
    public AssignUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}

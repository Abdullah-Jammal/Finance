using FluentValidation;

namespace Finance.Application.Features.User.Commands.UpdateUser;


public sealed class UpdateUserCompanyValidator
    : AbstractValidator<UpdateUserCompanyDto>
{
    public UpdateUserCompanyValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.RoleIds)
            .NotNull()
            .NotEmpty()
            .WithMessage("Each company must have at least one role");

        RuleFor(x => x.RoleIds)
            .Must(r => r.Distinct().Count() == r.Count)
            .WithMessage("Duplicate roles in the same company are not allowed");

        RuleForEach(x => x.RoleIds)
            .NotEmpty();
    }
}

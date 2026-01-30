using FluentValidation;

namespace Finance.Application.Features.User.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Companies)
            .NotNull()
            .NotEmpty()
            .WithMessage("User must belong to at least one company");

        RuleFor(x => x.Companies)
            .Must(c =>
                c.Select(x => x.CompanyId).Distinct().Count() == c.Count)
            .WithMessage("Duplicate companies are not allowed");

        RuleForEach(x => x.Companies)
            .SetValidator(new UpdateUserCompanyValidator());
    }
}

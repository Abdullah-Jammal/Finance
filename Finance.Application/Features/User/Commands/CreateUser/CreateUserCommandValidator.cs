using FluentValidation;

namespace Finance.Application.Features.User.Commands.CreateUser;

public sealed class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Companies)
            .NotNull()
            .NotEmpty()
            .WithMessage("User must belong to at least one company");

        RuleForEach(x => x.Companies)
            .SetValidator(new CreateUserCompanyValidator());
    }
}

using FluentValidation;

namespace Finance.Application.Features.Accounts.Commands.CreateAccount;

public sealed class CreateAccountCommandValidator
    : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company is required.");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Account code is required.")
            .MaximumLength(50).WithMessage("Account code must not exceed 50 characters.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Account name is required.")
            .MaximumLength(200).WithMessage("Account name must not exceed 200 characters.");
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Account type is required.");
        RuleFor(x => x.Subtype)
            .IsInEnum().WithMessage("Account subtype is invalid.");
    }
}

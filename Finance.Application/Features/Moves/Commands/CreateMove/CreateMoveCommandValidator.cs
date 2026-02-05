using FluentValidation;

namespace Finance.Application.Features.Moves.Commands.CreateMove;

public sealed class CreateMoveCommandValidator : AbstractValidator<CreateMoveCommand>
{
    public CreateMoveCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.Lines)
            .NotNull()
            .Must(lines => lines.Count >= 2)
            .WithMessage("Move must have at least two lines.");

        RuleForEach(x => x.Lines)
            .ChildRules(line =>
            {
                line.RuleFor(x => x.AccountId)
                    .NotEmpty();
                line.RuleFor(x => x.Debit)
                    .GreaterThanOrEqualTo(0m);
                line.RuleFor(x => x.Credit)
                    .GreaterThanOrEqualTo(0m);
                line.RuleFor(x => x)
                    .Must(x => !(x.Debit > 0m && x.Credit > 0m))
                    .WithMessage("Line cannot have both debit and credit.")
                    .Must(x => x.Debit != 0m || x.Credit != 0m)
                    .WithMessage("Line must have debit or credit.");
            });
    }
}

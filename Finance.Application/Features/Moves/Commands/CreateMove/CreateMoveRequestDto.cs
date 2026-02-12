using System.ComponentModel.DataAnnotations;

namespace Finance.Application.Features.Moves.Commands.CreateMove;

public sealed class CreateMoveRequestDto
{
    [Required]
    public Guid CompanyId { get; init; }

    [Required]
    public DateOnly Date { get; init; }

    [Required]
    public IReadOnlyList<CreateMoveLineRequestDto> Lines { get; init; } = [];
}

public sealed class CreateMoveLineRequestDto
{
    [Required]
    public Guid AccountId { get; init; }

    public decimal Debit { get; init; }

    public decimal Credit { get; init; }

    public Guid? PartnerId { get; init; }
}
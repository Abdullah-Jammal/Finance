using MediatR;

namespace Finance.Application.Features.Moves.Commands.CreateMove;

public sealed record CreateMoveCommand(
    Guid CompanyId,
    DateOnly Date,
    IReadOnlyList<CreateMoveLineDto> Lines) : IRequest<Guid>;

public sealed record CreateMoveLineDto(
    Guid AccountId,
    decimal Debit,
    decimal Credit,
    Guid? PartnerId);

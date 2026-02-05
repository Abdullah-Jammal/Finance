using Finance.Application.Contracts.Accounting;
using MediatR;

namespace Finance.Application.Features.Moves.Commands.CreateMove;

public sealed class CreateMoveCommandHandler(IMoveService service)
    : IRequestHandler<CreateMoveCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateMoveCommand request,
        CancellationToken cancellationToken)
    {
        var lines = request.Lines
            .Select(line => new MoveLineRequest(
                line.AccountId,
                line.Debit,
                line.Credit,
                line.PartnerId))
            .ToList();

        return await service.CreateAsync(
            request.CompanyId,
            request.Date,
            lines,
            cancellationToken);
    }
}

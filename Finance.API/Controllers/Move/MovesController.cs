using Finance.Application.Features.Moves.Commands.CreateMove;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finance.API.Controllers.Move;

[ApiController]
[Route("accounting/moves")]
[Authorize]
public sealed class MovesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMoveRequestDto request,
        CancellationToken ct)
    {
        var command = new CreateMoveCommand(
            request.CompanyId,
            request.Date,
            request.Lines
                .Select(l => new CreateMoveLineDto(
                    l.AccountId,
                    l.Debit,
                    l.Credit,
                    l.PartnerId))
                .ToList()
        );

        var moveId = await mediator.Send(command, ct);

        return CreatedAtAction(
            nameof(Create),
            new { id = moveId },
            moveId);
    }
}

using Finance.Application.Common.Authorization;
using Finance.Application.Features.Accounts.Commands.CreateAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finance.API.Controllers.Accounting;

[Route("api/[controller]")]
[ApiController]
public sealed class AccountsController(IMediator mediator) : ControllerBase
{
    [HttpPost("create-account")]
    [Authorize(Policy = Permissions.Accounts_Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAccountRequestDto request,
        CancellationToken cancellationToken)
    {
        var accountId = await mediator.Send(
            new CreateAccountCommand(
                request.CompanyId,
                request.Code,
                request.Name,
                request.Type,
                request.Subtype,
                request.ParentId,
                request.IsReconcilable,
                request.AllowPosting,
                request.IsActive),
            cancellationToken);

        return CreatedAtAction(
            nameof(Create),
            new { id = accountId },
            accountId);
    }
}

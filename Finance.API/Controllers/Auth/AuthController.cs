using Finance.Application.Features.Auth.Login;
using Finance.Application.Features.Auth.Refresh;
using Finance.Application.Features.Auth.SelectCompany;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finance.API.Controllers.Auth;

[ApiController]
[Route("auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("select-company")]
    public async Task<IActionResult> SelectCompany(
        [FromBody] SelectCompanyRequest request,
        CancellationToken ct)
    {
        var userId = request.UserId;
        if (!userId.HasValue
            && Guid.TryParse(
                Request.Headers["X-User-Id"].FirstOrDefault(),
                out var headerUserId))
        {
            userId = headerUserId;
        }

        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var result = await mediator.Send(
            new SelectCompanyCommand(userId.Value, request.CompanyId),
            ct);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}

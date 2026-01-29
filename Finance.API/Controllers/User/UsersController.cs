using Finance.Application.Features.User.Commands.AssignCompany;
using Finance.Application.Features.User.Commands.AssignRole;
using Finance.Application.Features.User.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("admin/users")]
[Authorize(Policy = "users.manage")]
public sealed class UsersController(IMediator mediator)
    : ControllerBase
{
    [HttpPost("{userId:guid}/companies/{companyId:guid}/roles")]
    public async Task<IActionResult> AssignRole(
    Guid userId,
    Guid companyId,
    [FromBody] AssignUserRoleRequest request,
    CancellationToken ct)
    {
        await mediator.Send(
            new AssignUserRoleCommand(
                userId,
                companyId,
                request.RoleId),
            ct);

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateUserRequest request,
    CancellationToken ct)
    {
        var userId = await mediator.Send(
            new CreateUserCommand(
                request.Email,
                request.Password,
                request.FullName,
                request.CompanyId,
                request.RoleId),
            ct);

        return CreatedAtAction(
            nameof(Create),
            new { id = userId },
            userId);
    }

    [HttpPost("{userId:guid}/companies")]
    public async Task<IActionResult> AssignToCompany(
    Guid userId,
    [FromBody] AssignUserToCompanyRequest request,
    CancellationToken ct)
    {
        await mediator.Send(
            new AssignUserToCompanyCommand(
                userId,
                request.CompanyId),
            ct);

        return NoContent();
    }
}

public sealed class AssignUserToCompanyRequest
{
    public Guid CompanyId { get; init; }
}


public sealed class AssignUserRoleRequest
{
    public Guid RoleId { get; init; }
}


public sealed class CreateUserRequest
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public Guid CompanyId { get; init; }
    public Guid RoleId { get; init; }
}

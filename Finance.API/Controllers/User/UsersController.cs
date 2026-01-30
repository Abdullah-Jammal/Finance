using Finance.API.Models.Queries.Users;
using Finance.Application.Features.User.Commands.CreateUser;
using Finance.Application.Features.User.Commands.UpdateUser;
using Finance.Application.Features.User.Queries.GetAllUsers;
using Finance.Application.Features.User.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("admin/users")]
[Authorize(Policy = "users.manage")]
public sealed class UsersController(IMediator mediator)
    : ControllerBase
{
    // Create User
    [HttpPost("create-user")]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequestDto request,
        CancellationToken ct)
    {
        var userId = await mediator.Send(
            new CreateUserCommand(
                request.Email,
                request.Password,
                request.FullName,
                request.Companies.Select(c =>
                    new CreateUserCompanyDto(
                        c.CompanyId,
                        c.RoleIds
                    )).ToList()
            ),
            ct);

        return CreatedAtAction(
            nameof(Create),
            new { id = userId },
            userId);
    }

    // Get All Users
    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAll(
    [FromQuery] UserQueryParameters query,
    CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetAllUsersQuery(
                query.CompanyId,
                query.Search,
                query.SortBy,
                query.SortDirection,
                query.PageNumber,
                query.PageSize),
            ct);

        return Ok(result);
    }

    // Get User By Id
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetById(
    Guid userId,
    CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetUserByIdQuery(userId),
            ct);

        return Ok(result);
    }

    // Update User
    [HttpPut("update-user/{userId:guid}")]
    public async Task<IActionResult> Update(
    Guid userId,
    [FromBody] UpdateUserRequestDto request,
    CancellationToken ct)
    {
        await mediator.Send(
            new UpdateUserCommand(
                userId,
                request.FullName,
                request.IsActive,
                request.Companies.Select(c =>
                    new UpdateUserCompanyDto(
                        c.CompanyId,
                        c.RoleIds)).ToList()),
            ct);

        return NoContent();
    }
}

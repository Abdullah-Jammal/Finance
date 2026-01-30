namespace Finance.Application.Features.User.Queries.GetAllUsers;

public sealed record UserListItemDto(
    Guid Id,
    string FullName,
    string Email,
    bool IsActive,
    DateTime CreatedAt
);

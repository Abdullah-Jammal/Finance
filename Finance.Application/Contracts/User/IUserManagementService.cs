using Finance.Application.Common.Pagination;
using Finance.Application.Common.Sorting;
using Finance.Application.Features.User.Commands.CreateUser;
using Finance.Application.Features.User.Commands.UpdateUser;
using Finance.Application.Features.User.Queries.GetAllUsers;
using Finance.Application.Features.User.Queries.GetUserById;
using Finance.Application.Features.Users.Queries.GetAllUsers;

namespace Finance.Application.Contracts.User;

public interface IUserManagementService
{
    Task<Guid> CreateUserAsync(
        string email,
        string password,
        string fullName,
        IReadOnlyCollection<CreateUserCompanyDto> companies,
        CancellationToken ct);
    Task<PagedResult<UserListItemDto>> GetAllAsync(
        Guid? companyId,
        string? search,
        UserSortField sortBy,
        SortDirection sortDirection,
        int pageNumber,
        int pageSize,
        CancellationToken ct);
    Task<UserDetailsDto> GetByIdAsync(
        Guid userId,
        CancellationToken ct);
    Task UpdateUserAsync(
    Guid userId,
    string fullName,
    bool isActive,
    IReadOnlyCollection<UpdateUserCompanyDto> companies,
    CancellationToken ct);
}

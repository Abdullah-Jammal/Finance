namespace Finance.Application.Contracts.User;

public interface IUserManagementService
{
    Task AssignRoleAsync(
        Guid userId,
        Guid companyId,
        Guid roleId,
        CancellationToken ct);

    Task<Guid> CreateUserAsync(
        string email,
        string password,
        string fullName,
        Guid companyId,
        Guid roleId,
        CancellationToken ct);
    Task AssignUserToCompanyAsync(
    Guid userId,
    Guid companyId,
    CancellationToken ct);
}

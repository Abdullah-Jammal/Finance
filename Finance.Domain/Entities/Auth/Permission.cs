
namespace Finance.Domain.Entities.Auth;

public class Permission
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = default!;
    public string? Description { get; private set; }

    private Permission() { }

    public Permission(string code, string? description = null)
    {
        Code = code;
        Description = description;
    }
}

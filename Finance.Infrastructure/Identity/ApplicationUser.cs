using Microsoft.AspNetCore.Identity;

namespace Finance.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

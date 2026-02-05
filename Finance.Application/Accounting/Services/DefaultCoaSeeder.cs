using Finance.Application.Accounting.CoaTemplates;
using Finance.Application.Contracts.Account;

namespace Finance.Application.Accounting.Services;

public sealed class DefaultCoaSeeder(IAccountService accountService)
    : ICoaSeeder
{
    public async Task SeedAsync(Guid companyId, CancellationToken ct)
    {
        if (await accountService.HasAnyAccountAsync(companyId, ct))
            return;

        var createdAccounts = new Dictionary<string, Guid>();

        foreach (var def in DefaultCoaTemplate.Accounts)
        {
            Guid? parentId = null;

            if (def.ParentCode is not null)
                parentId = createdAccounts[def.ParentCode];

            var accountId = await accountService.CreateAsync(
                companyId,
                def.Code,
                def.Name,
                def.Type,
                def.Subtype,
                parentId,
                def.AllowPosting,
                def.IsActive,
                ct);

            createdAccounts[def.Code] = accountId;
        }
    }
}

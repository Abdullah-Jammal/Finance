using Finance.Domain.Enums;

namespace Finance.Domain.Rules;

public static class AccountSubtypeRules
{
    private static readonly Dictionary<AccountType, HashSet<AccountSubtype>> _allowed
        = new()
        {
            [AccountType.Asset] = new()
            {
                AccountSubtype.Cash,
                AccountSubtype.Bank,
                AccountSubtype.AccountsReceivable
            },

            [AccountType.Liability] = new()
            {
                AccountSubtype.AccountsPayable,
                AccountSubtype.TaxPayable
            },

            [AccountType.Revenue] = new()
            {
                AccountSubtype.Revenue
            },

            [AccountType.Expense] = new()
            {
                AccountSubtype.Expense
            },

            [AccountType.Equity] = new()
            {
                AccountSubtype.None
            }
        };

    public static bool IsValid(
        AccountType type,
        AccountSubtype subtype)
    {
        if (!_allowed.TryGetValue(type, out var subtypes))
            return false;

        return subtypes.Contains(subtype);
    }
}

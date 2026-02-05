using Finance.Domain.Enums;

namespace Finance.Domain.Rules;

public static class AccountBehaviorRules
{
    public static bool IsReconcilable(AccountSubtype subtype) =>
        subtype switch
        {
            AccountSubtype.Cash => true,
            AccountSubtype.Bank => true,
            AccountSubtype.AccountsReceivable => true,
            AccountSubtype.AccountsPayable => true,
            _ => false
        };

    public static bool RequiresPartner(AccountSubtype subtype) =>
        subtype switch
        {
            AccountSubtype.AccountsReceivable => true,
            AccountSubtype.AccountsPayable => true,
            _ => false
        };

    public static bool AllowsManualPosting(AccountSubtype subtype) =>
        subtype switch
        {
            AccountSubtype.Revenue => true,
            AccountSubtype.Expense => true,
            AccountSubtype.Cash => true,
            AccountSubtype.Bank => true,
            _ => false
        };
}

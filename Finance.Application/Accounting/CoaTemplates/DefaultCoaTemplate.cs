using Finance.Domain.Enums;

namespace Finance.Application.Accounting.CoaTemplates;

public static class DefaultCoaTemplate
{
    public static IReadOnlyList<CoaAccountDefinition> Accounts =>
        new List<CoaAccountDefinition>
        {
            // ===== ASSETS =====
            new("1000", "Assets", AccountType.Asset, AccountSubtype.None, null, false),
            new("1100", "Cash", AccountType.Asset, AccountSubtype.Cash, "1000", true),
            new("1110", "Bank", AccountType.Asset, AccountSubtype.Bank, "1000", true),
            new("1200", "Accounts Receivable", AccountType.Asset, AccountSubtype.AccountsReceivable, "1000", true),

            // ===== LIABILITIES =====
            new("2000", "Liabilities", AccountType.Liability, AccountSubtype.None, null, false),
            new("2100", "Accounts Payable", AccountType.Liability, AccountSubtype.AccountsPayable, "2000", true),
            new("2200", "Tax Payable", AccountType.Liability, AccountSubtype.TaxPayable, "2000", true),

            // ===== EQUITY =====
            new("3000", "Equity", AccountType.Equity, AccountSubtype.None, null, false),

            // ===== REVENUE =====
            new("4000", "Revenue", AccountType.Revenue, AccountSubtype.Revenue, null, false),

            // ===== EXPENSE =====
            new("5000", "Expenses", AccountType.Expense, AccountSubtype.Expense, null, false),
        };
}

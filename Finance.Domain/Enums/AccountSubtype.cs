namespace Finance.Domain.Enums;

public enum AccountSubtype
{
    None = 0,

    // Assets
    Cash = 1,
    Bank = 2,
    AccountsReceivable = 3,

    // Liabilities
    AccountsPayable = 4,
    TaxPayable = 5,

    // P&L
    Revenue = 6,
    Expense = 7
}

using Finance.Domain.Entities.Company;

namespace Finance.Domain.Tests.Entities.Company;

public sealed class CompanyTests
{
    [Fact]
    public void Constructor_Normalizes_Code_And_Currency()
    {
        var company = new Company("Acme", " ab-123 ", " usd ");

        Assert.Equal("AB-123", company.Code);
        Assert.Equal("USD", company.BaseCurrencyCode);
    }

    [Fact]
    public void UpdateDetails_Normalizes_Code_And_Currency()
    {
        var company = new Company("Acme", "AB-123", "USD");

        company.UpdateDetails("Acme Updated", " xy-9 ", " eur ");

        Assert.Equal("XY-9", company.Code);
        Assert.Equal("EUR", company.BaseCurrencyCode);
        Assert.Equal("Acme Updated", company.Name);
    }

    [Fact]
    public void UpdateDetails_Throws_When_Code_Is_Missing()
    {
        var company = new Company("Acme", "AB-123", "USD");

        var exception = Assert.Throws<ArgumentException>(() =>
            company.UpdateDetails("Acme", "   ", "USD"));

        Assert.Contains("Company code is required", exception.Message);
    }
}

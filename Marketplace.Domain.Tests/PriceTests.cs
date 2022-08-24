using System.Security.AccessControl;

namespace Marketplace.Domain.Tests;

public class PriceTests
{
    private static readonly ICurrencyLookup CurrencyLookup = new FakeCurrencyLookup();
    
    [Fact]
    public void Can_create_a_Price_with_a_positive_value()
    {
        var price = Price.FromDecimal(100.0m,"NZD", CurrencyLookup);
        Assert.Equal(100.0m, price.Amount);
    }

    [Fact]
    public void A_negative_price_throws_an_exception()
    {
        Assert.Throws<ArgumentException>(() => Price.FromDecimal(-100.0m,"NZD", CurrencyLookup));
    }
    
    [Fact]
    public void Sum_of_price_gives_full_amount()
    {
        var coin1 = Price.FromDecimal(1,"NZD", CurrencyLookup);
        var coin2 = Price.FromDecimal(2,"NZD", CurrencyLookup);
        var coin3 = Price.FromDecimal(2,"NZD", CurrencyLookup);
        var banknote = Price.FromDecimal(5,"NZD", CurrencyLookup);
        Assert.Equal(banknote, coin1 + coin2 + coin3);
    }
}
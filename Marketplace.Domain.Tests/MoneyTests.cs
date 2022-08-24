namespace Marketplace.Domain.Tests;

public class MoneyTests {
    private static readonly ICurrencyLookup CurrencyLookup = new FakeCurrencyLookup();
    
    [Fact]
    public void Money_objects_with_the_same_amount_should_be_equal() {
        var firstAmount = Money.FromDecimal(5, "NZD", CurrencyLookup);
        var secondAmount = Money.FromDecimal(5,"NZD", CurrencyLookup);
        Assert.Equal(firstAmount, secondAmount);
    }

    [Fact]
    public void Money_objects_with_the_same_amount_should_be_equal_using_op() {
        var firstAmount = Money.FromDecimal(5,"NZD", CurrencyLookup);
        var secondAmount = Money.FromDecimal(5,"NZD", CurrencyLookup);
        Assert.True(firstAmount == secondAmount);
    }

    [Fact]
    public void Money_objects_with_different_amounts_should_not_be_equal_using_op() {
        var firstAmount = Money.FromDecimal(5,"NZD", CurrencyLookup);
        var secondAmount = Money.FromDecimal(9,"NZD", CurrencyLookup);
        Assert.True(firstAmount != secondAmount);
    }

    [Fact]
    public void Sum_of_money_gives_full_amount() {
        var coin1 = Money.FromDecimal(1,"NZD", CurrencyLookup);
        var coin2 = Money.FromDecimal(2,"NZD", CurrencyLookup);
        var coin3 = Money.FromDecimal(2,"NZD", CurrencyLookup);
        var banknote = Money.FromDecimal(5,"NZD", CurrencyLookup);
        Assert.Equal(banknote, coin1 + coin2 + coin3);
    }

    [Fact]
    public void Subtracting_money_gives_expected_amount() {
        var x = Money.FromDecimal(10.0m,"NZD", CurrencyLookup);
        var y = Money.FromDecimal(2.5m,"NZD", CurrencyLookup);
        var result = x - y;
        Assert.Equal(7.5m, result.Amount);
    }

    [Fact]
    public void Cannot_add_amounts_with_different_currencies() {
        var x = Money.FromDecimal(10.0m, "NZD", CurrencyLookup);
        var y = Money.FromDecimal(2.5m, "USD", CurrencyLookup);
        Assert.Throws<ArgumentException>(() => x + y);
    }

    [Fact]
    public void Cannot_subtract_amounts_with_different_currencies() {
        var x = Money.FromDecimal(10.0m, "NZD", CurrencyLookup);
        var y = Money.FromDecimal(2.5m, "USD", CurrencyLookup);
        Assert.Throws<ArgumentException>(() => x - y);
    }
}
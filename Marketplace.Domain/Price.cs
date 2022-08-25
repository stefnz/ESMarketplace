namespace Marketplace.Domain;

public record Price : Money {
    private Price(decimal amount, string currencyCode, ICurrencyLookup lookup) : base(amount, currencyCode, lookup) {
        if (amount < 0) {
            throw new ArgumentException("Price cannot be negative", nameof(amount));
        }
    }
    
    internal Price(decimal amount, Currency currency) : base(amount, currency) { }

    internal Price(decimal amount, string currencyCode) : base(amount, new Currency {CurrencyCode = currencyCode}) { }

    public static readonly Price Default = new(0m, Currency.Default);

    public new static Price FromDecimal(decimal amount, string currencyCode, ICurrencyLookup lookup) => new (amount, currencyCode, lookup);

    public static Price operator +(Price x) => x;
    public static Price operator +(Price x, Price y) => new(x.Amount + y.Amount, x.Currency);

    public static Price operator -(Price x) => new(0 - x.Amount, x.Currency);
    public static Price operator -(Price x, Price y) => new(x.Amount - y.Amount, x.Currency);
}
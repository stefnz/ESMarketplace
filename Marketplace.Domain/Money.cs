namespace Marketplace.Domain;

public record Money
{
    public decimal Amount{ get; }
    public Currency Currency { get; }
    private const string DefaultCurrencyCode="NZD";

    public static Money FromDecimal(decimal amount, string currencyCode, ICurrencyLookup lookup) => new Money(amount, currencyCode, lookup);
    public static Money FromString(string amount, string currencyCode, ICurrencyLookup lookup ) => new Money(decimal.Parse(amount), currencyCode, lookup);
    
    protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
    {
        if (string.IsNullOrWhiteSpace(currencyCode)) {
            throw new ArgumentNullException(nameof(currencyCode), "Currency code must be specified");
        }

        var currency = currencyLookup.FindCurrency(currencyCode);
        
        if (!currency.InUse) {
            throw new ArgumentException($"Currency {currency.CurrencyCode} is not in use.");
        }

        if (decimal.Round(amount, currency.DecimalPlaces) != amount) {
            throw new ArgumentOutOfRangeException(nameof(amount),
                $"Amount in {currency.CurrencyCode} cannot have more than {currency.DecimalPlaces} decimals.");
        }

        if (decimal.Round(amount, 2) != amount) {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot have more than two decimals");
        }

        Amount = amount;
        Currency = currency;
    }

    protected Money(decimal amount, Currency currency) {
        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money value) {
        if (Currency != value.Currency) {
            throw new ArgumentException("Cannot subtract amounts with different currencies", nameof(value));
        }
        return new Money(Amount + value.Amount, Currency);
    }
    
    public Money Subtract(Money value) {
        if (Currency != value.Currency) {
            throw new ArgumentException("Cannot subtract amounts with different currencies", nameof(value));
        }
        return new Money(Amount - value.Amount, Currency);
    }

    public static Money operator +(Money x) => x;
    public static Money operator +(Money x, Money y) => x.Add(y);
    public static Money operator -(Money x) => new Money(0, x.Currency).Subtract(x);
    public static Money operator -(Money x, Money y) => x.Subtract(y);
}

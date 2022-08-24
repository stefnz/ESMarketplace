namespace Marketplace.Domain.Tests;

public class FakeCurrencyLookup : ICurrencyLookup {
    private static readonly IEnumerable<Currency> currencies =
        new[] {
            new Currency {
                CurrencyCode = "EUR",
                DecimalPlaces = 2,
                InUse = true
            },
            new Currency {
                CurrencyCode = "USD",
                DecimalPlaces = 2,
                InUse = true
            },
            new Currency {
                CurrencyCode = "NZD",
                DecimalPlaces = 2,
                InUse = true
            },
            new Currency {
                CurrencyCode = "JPY",
                DecimalPlaces = 0,
                InUse = true
            },
            new Currency {
                CurrencyCode = "DEM",
                DecimalPlaces = 2,
                InUse = false
            }
        };

    public Currency FindCurrency(string currencyCode) {
        var currency = currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode);
        return currency ?? Currency.None;
    }
}

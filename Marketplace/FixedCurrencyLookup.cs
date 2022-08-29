using Marketplace.Domain;

namespace Marketplace; 

public class FixedCurrencyLookup: ICurrencyLookup {
    private static readonly IEnumerable<Currency> currencies =
        new[] {
            new Currency(currencyCode: "EUR", decimalPlaces: 2, inUse: true),
            new Currency(currencyCode: "USD", decimalPlaces: 2, inUse: true),
            new Currency(currencyCode: "NZD", decimalPlaces: 2, inUse: true),
            new Currency(currencyCode: "JPY", decimalPlaces: 0, inUse: true),
            new Currency(currencyCode: "DEM", decimalPlaces: 2, inUse: false)
        };

    public Currency FindCurrency(string currencyCode) {
        var currency = currencies.FirstOrDefault(currency => currency.CurrencyCode == currencyCode);
        return currency ?? Currency.None;
    }    
}
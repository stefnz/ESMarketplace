namespace Marketplace.Domain; 

public record Currency {
    public string CurrencyCode { get; init; }
    public bool InUse { get; init; }
    public int DecimalPlaces { get; init; }

    public static readonly Currency None = new Currency {InUse = false};
    public static readonly Currency Default = new Currency { CurrencyCode = "NZD", InUse = true, DecimalPlaces = 2};
    public Currency() { }

    public Currency(string currencyCode, int decimalPlaces, bool inUse) {
        CurrencyCode = currencyCode;
        DecimalPlaces = decimalPlaces;
        InUse = inUse;
    }
}
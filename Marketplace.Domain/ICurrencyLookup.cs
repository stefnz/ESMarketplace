namespace Marketplace.Domain; 

/// <summary>
/// Get a currency for the provided currency code.
/// 
/// Domain service, the dependency is captured by an interface. DI is used in the application service to inject
/// a lookup.
/// </summary>
public interface ICurrencyLookup {
    Currency FindCurrency(string currencyCode);
}
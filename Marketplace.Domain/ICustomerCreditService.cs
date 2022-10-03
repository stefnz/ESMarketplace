namespace Marketplace.Domain; 

public interface ICustomerCreditService {
    Task<bool> EnsureSufficientCredit(int customerId, decimal amount); // Should use CustomerId type?
}
namespace Marketplace.Projections;

public interface IReadModel { }

/// <summary>
/// Read models
/// Using RavenDb, therefore model identities are string properties named Id  
/// </summary>
public class ReadModels {
    public class ClassifiedAdDetails: IReadModel {
        public String  Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public Guid SellerId { get; set; }
        public string SellersDisplayName { get; set; }
        public string SellersPhotoUrl { get; set; }
        public string[] PhotoUrls { get; set; }
    }

    public class UserDetails: IReadModel {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class ClassifiedAdListItem: IReadModel {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string PhotoUrl { get; set; }
    }
    
    public class PublicClassifiedAdListItem: IReadModel {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string PhotoUrl { get; set; }
    }
}
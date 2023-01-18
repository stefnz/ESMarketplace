namespace Marketplace.Domain;

public interface IClassifiedAdEvent {
    Guid Id { get; }
}

public static class ClassifiedAdEvents {
    public class ClassifiedAdCreated : IClassifiedAdEvent {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
    }

    public class ClassifiedAdTitleChanged : IClassifiedAdEvent {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }

    public class ClassifiedAdTextUpdated : IClassifiedAdEvent {
        public Guid Id { get; set; }
        public string AdText { get; set; }
    }

    public class ClassifiedAdPriceUpdated : IClassifiedAdEvent {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class ClassifiedAdSentForReview : IClassifiedAdEvent {
        public Guid Id { get; set; }
    }

    public class ClassifiedAdPublished : IClassifiedAdEvent {
        public Guid Id { get; set; }
        public Guid ApprovedBy { get; set; }
        public Guid OwnerId { get; set; }
    }

    public class PictureAddedToAClassifiedAd {
        public Guid ClassifiedAdId { get; set; }
        public Guid PictureId { get; set; }
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Order { get; set; }
    }

    public class PictureResized {
        public Guid PictureId { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
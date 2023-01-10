namespace Marketplace.ClassifiedAds; 

public static class ClassifiedAdsQueries {
    public static ReadModels.ClassifiedAdDetails Query(
        this IEnumerable<ReadModels.ClassifiedAdDetails> items,
        QueryModels.GetPublicClassifiedAd query)
        => items.FirstOrDefault(adDetails => adDetails.ClassifiedAdId == query.ClassifiedAdId);
}


using Marketplace.Projections;
using Raven.Client.Documents.Session;

namespace Marketplace.ClassifiedAds; 

public static class ClassifiedAdsQueries {
    public static Task<ReadModels.ClassifiedAdDetails> Query(this IAsyncDocumentSession session, QueryModels.GetPublicClassifiedAd query)
        => session.LoadAsync<ReadModels.ClassifiedAdDetails>(query.ClassifiedAdId.ToString());
}


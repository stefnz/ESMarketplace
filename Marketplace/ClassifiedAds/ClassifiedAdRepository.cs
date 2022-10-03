using Marketplace.Domain;
using Raven.Client.Documents.Session;

namespace Marketplace.Infrastructure; 

public class ClassifiedAdRepository: RavenDbRepository<ClassifiedAd, ClassifiedAdId>, IClassifiedAdRepository {
    public ClassifiedAdRepository(IAsyncDocumentSession session) 
        : base(session, id => $"ClassifiedAd/{id.Value.ToString()}") { }
}
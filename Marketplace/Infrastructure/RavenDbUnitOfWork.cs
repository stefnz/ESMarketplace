using ES.Framework;
using Raven.Client.Documents.Session;

namespace Marketplace.Infrastructure; 

public class RavenDbUnitOfWork: IUnitOfWork {
    private readonly IAsyncDocumentSession session;

    public RavenDbUnitOfWork(IAsyncDocumentSession session) => this.session = session;

    public Task Commit() => session.SaveChangesAsync();
}
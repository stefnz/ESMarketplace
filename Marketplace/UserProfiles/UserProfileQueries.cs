using Marketplace.Projections;
using Raven.Client.Documents.Session;

namespace Marketplace.UserProfiles;

public static class UserProfileQueries
{
    public static Task<ReadModels.UserDetails> GetUserDetails(this Func<IAsyncDocumentSession> getSession, Guid id) {
        using var session = getSession();
        return session.LoadAsync<ReadModels.UserDetails>(id.ToString());
    }
}
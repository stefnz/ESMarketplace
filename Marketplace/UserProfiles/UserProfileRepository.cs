using Marketplace.Domain.UserProfiles;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;

namespace Marketplace.UserProfiles; 

public class UserProfileRepository: RavenDbRepository<UserProfile, UserId>, IUserProfileRepository {
    public UserProfileRepository(IAsyncDocumentSession session) 
        : base(session, id => $"UserProfile/{id.Value.ToString()}") {}

}

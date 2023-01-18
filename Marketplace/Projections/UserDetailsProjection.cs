using ES.Framework;
using Marketplace.Domain.UserProfiles;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;
using Serilog;

namespace Marketplace.Projections;

public class UserDetailsProjection : RavenDbProjection<ReadModels.UserDetails> {
    public UserDetailsProjection(Func<IAsyncDocumentSession> getSession) : base(getSession) { }

    public override Task Project(object @event) {
        Log.Debug("Projecting event {eventType} to projection UserDetails", @event.GetType().Name);
        
        switch(@event) {
            case UserProfileEvents.UserRegistered e:
                Create(() => Task.FromResult(new ReadModels.UserDetails {
                    Id = e.UserId.ToString(),
                    DisplayName = e.DisplayName
                }));
                break;
            case UserProfileEvents.UserDisplayNameUpdated e:
                UpdateSingle(e.UserId, user => user.DisplayName = e.DisplayName);
                break;
            case UserProfileEvents.ProfilePhotoUploaded e:
                UpdateSingle(e.UserId, user => user.PhotoUrl = e.PhotoUrl);
                break;
        }

        return Task.CompletedTask;
    }
}

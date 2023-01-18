using ES.Framework;
using EventStore.ClientAPI;
using Marketplace.Domain;
using Marketplace.Infrastructure;

namespace Marketplace.Projections.Upcasters;

public class ClassifiedAdUpcasters: IProjection
{
    private readonly IEventStoreConnection eventStoreConnection;
    private readonly Func<Guid, Task<string>> getUserPhoto;
    private const string StreamName = "UpcastedClassifiedAdEvents";
    
    public ClassifiedAdUpcasters(IEventStoreConnection eventStoreConnection, Func<Guid, Task<string>> getUserPhoto) {
        this.eventStoreConnection = eventStoreConnection;
        this.getUserPhoto = getUserPhoto;
    }
    public async Task Project(object @event)
    {
        switch (@event) {
            case ClassifiedAdEvents.ClassifiedAdPublished e:
                var photoUrl = await getUserPhoto(e.OwnerId);
                var newEvent = new ClassifiedAdUpcastEvents.V1.ClassifiedAdPublished {
                    Id = e.Id,
                    OwnerId = e.OwnerId,
                    ApprovedBy = e.ApprovedBy,
                    SellersPhotoUrl = photoUrl
                };
                await eventStoreConnection.AppendEvents(StreamName, ExpectedVersion.Any, newEvent);
                break;
        }
    }    
}
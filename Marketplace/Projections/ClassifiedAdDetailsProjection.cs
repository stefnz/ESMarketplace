using Marketplace.Domain;
using Marketplace.Domain.UserProfiles;
using Marketplace.Projections;
using Marketplace.Projections.Upcasters;
using Raven.Client.Documents.Session;
using Serilog;

namespace Marketplace.Infrastructure; 

public class ClassifiedAdDetailsProjection : RavenDbProjection<ReadModels.ClassifiedAdDetails>
{
    private readonly Func<Guid, Task<string>> getUserDisplayName;

    public ClassifiedAdDetailsProjection(Func<IAsyncDocumentSession> getSession, Func<Guid, Task<string>> getUserDisplayName) : base(getSession)
        => this.getUserDisplayName = getUserDisplayName;

    public override Task Project(object @event)
    {
        Log.Debug("Projecting event {eventType} to projection ClassifiedAdDetails", @event.GetType().Name);
        
        switch (@event) {
            case ClassifiedAdEvents.ClassifiedAdCreated e:
                Create( async () => 
                    new ReadModels.ClassifiedAdDetails {
                        Id = e.Id.ToString(), 
                        SellerId = e.OwnerId,
                        SellersDisplayName = await getUserDisplayName(e.OwnerId)
                    }
                );
                break;
            case ClassifiedAdEvents.ClassifiedAdTitleChanged e:
                UpdateSingle(e.Id, ad => ad.Title = e.Title);
                break;
            case ClassifiedAdEvents.ClassifiedAdTextUpdated e:
                UpdateSingle(e.Id, ad => ad.Description = e.AdText);
                break;
            case ClassifiedAdEvents.ClassifiedAdPriceUpdated e:
                UpdateSingle(e.Id, ad => {
                    ad.Price = e.Price;
                    ad.CurrencyCode = e.CurrencyCode;
                });
                break;
            case UserProfileEvents.UserDisplayNameUpdated e:
                UpdateWhere(
                    ad => ad.SellerId == e.UserId,
                    ad => ad.SellersDisplayName = e.DisplayName);
                break;
            case ClassifiedAdUpcastEvents.V1.ClassifiedAdPublished e:
                UpdateSingle(e.Id, ad => ad.SellersPhotoUrl = e.SellersPhotoUrl);
                break;
            default:
                Log.Information("Received event {eventType} to project into ClassifiedAdDetails but there is no handler.", @event.GetType().FullName);
                break;
        }
        return Task.CompletedTask;
    }
}


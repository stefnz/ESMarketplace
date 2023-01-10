using EventStore.ClientAPI;
using Marketplace.ClassifiedAds;
using Marketplace.Domain;
using Serilog;
using Serilog.Events;

namespace Marketplace.Infrastructure; 

public class ClassifiedAdEventsSubscription {
    private static readonly Serilog.ILogger log = Serilog.Log.ForContext<ClassifiedAdEventsSubscription>();

    private readonly IEventStoreConnection connection;
    private readonly IList<ReadModels.ClassifiedAdDetails> items;
    private EventStoreAllCatchUpSubscription subscription;

    public ClassifiedAdEventsSubscription(IEventStoreConnection connection, IList<ReadModels.ClassifiedAdDetails> items) {
        this.connection = connection;
        this.items = items;
    }

    public void Start() {
        var settings = new CatchUpSubscriptionSettings(
            2000,
            500,
            Log.IsEnabled(LogEventLevel.Verbose),
            true,
            "try-out-subscription");

        subscription = connection.SubscribeToAllFrom(Position.Start, settings, EventAppeared);
    }

    private Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent) {

        if (resolvedEvent.Event.EventType.StartsWith("$")) {
            return Task.CompletedTask;
        }

        // try... catch errors during deserialization
        var @event = resolvedEvent.Deserialize();
        log.Debug("Projecting event {type}", @event.GetType().Name);

        switch (@event) {
            case ClassifiedAdEvents.ClassifiedAdCreated e:
                items.Add(new ReadModels.ClassifiedAdDetails {
                    ClassifiedAdId = e.Id
                });
                break;
            
            case ClassifiedAdEvents.ClassifiedAdTitleChanged e:
                UpdateItem(e.Id, ad => ad.Title = e.Title);
                break;
            
            case ClassifiedAdEvents.ClassifiedAdTextUpdated e:
                UpdateItem(e.Id, ad => ad.Description = e.AdText);
                break;
            
            case ClassifiedAdEvents.ClassifiedAdPriceUpdated e:
                UpdateItem(e.Id, ad => {
                    ad.Price = e.Price;
                    ad.CurrencyCode = e.CurrencyCode;
                });
                break;
        }
        return Task.CompletedTask;
    }

    private void UpdateItem(Guid id, Action<ReadModels.ClassifiedAdDetails> update) {
        var item = items.FirstOrDefault(ad => ad.ClassifiedAdId == id);
        if (item == null) { return; }

        update(item);
    }

    public void Stop() => subscription.Stop();
}
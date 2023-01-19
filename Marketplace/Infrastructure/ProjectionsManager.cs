using ES.Framework;
using EventStore.ClientAPI;
using Serilog;
using Serilog.Events;

namespace Marketplace.Infrastructure;

public class ProjectionsManager {
    private readonly IEventStoreConnection connection;
    private readonly ICheckpointStore checkpointStore;
    private readonly IProjection[] projections;
    private EventStoreAllCatchUpSubscription subscription;
    
    public ProjectionsManager(IEventStoreConnection connection, ICheckpointStore checkpointStore, params IProjection[] projections) {
        this.connection = connection;
        this.checkpointStore = checkpointStore;
        this.projections = projections;
    }
    
    public async Task Start() {
        var settings = new CatchUpSubscriptionSettings(
            2000, 
            500,
            Log.IsEnabled(LogEventLevel.Verbose),
            false, 
            "try-out-subscription");

        var position = await checkpointStore.GetCheckpoint();
        subscription = connection.SubscribeToAllFrom(position, settings, EventAppeared);
    }

    public void Stop() => subscription.Stop();

    private async Task EventAppeared(EventStoreCatchUpSubscription _, ResolvedEvent resolvedEvent) {
        // ignore system events, we shouldn't see any if the 'resolveLinkTos' setting is false
        if (resolvedEvent.Event.EventType.StartsWith("$")) {
            return;
        }

        var @event = resolvedEvent.Deserialize();
        //Log.Debug("Projecting event {type}", @event.GetType().Name);
        
        await Task.WhenAll(projections.Select(projection => projection.Project(@event)));
        await checkpointStore.StoreCheckpoint(resolvedEvent.OriginalPosition.Value);
    }
}
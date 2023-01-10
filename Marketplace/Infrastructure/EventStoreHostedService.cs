using EventStore.ClientAPI;

namespace Marketplace.Infrastructure; 

public class EventStoreHostedService: IHostedService {
    private readonly IEventStoreConnection connection;
    private readonly ClassifiedAdEventsSubscription subscription;

    public EventStoreHostedService(IEventStoreConnection connection, ClassifiedAdEventsSubscription subscription) {
        this.connection = connection;
        this.subscription = subscription;
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        await connection.ConnectAsync();
        subscription.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        subscription.Stop();
        connection.Close();
        return Task.CompletedTask;
    }
}
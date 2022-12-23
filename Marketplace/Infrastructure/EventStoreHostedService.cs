using EventStore.ClientAPI;

namespace Marketplace.Infrastructure; 

public class EventStoreHostedService: IHostedService {
    private readonly IEventStoreConnection connection;

    public EventStoreHostedService(IEventStoreConnection connection) {
        this.connection = connection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
        => connection.ConnectAsync();

    public Task StopAsync(CancellationToken cancellationToken) {
        connection.Close();
        return Task.CompletedTask;
    }
}
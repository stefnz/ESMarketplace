using EventStore.ClientAPI;

namespace Marketplace.Infrastructure; 

public class EventStoreHostedService: IHostedService {
    private readonly IEventStoreConnection connection;
    private readonly ProjectionsManager projectionsManager;

    public EventStoreHostedService(IEventStoreConnection connection, ProjectionsManager manager) {
        this.connection = connection;
        this.projectionsManager = manager;
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        await connection.ConnectAsync();
        await projectionsManager.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        projectionsManager.Stop();
        connection.Close();
        return Task.CompletedTask;
    }
}
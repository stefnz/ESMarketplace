using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace Marketplace.Infrastructure; 

public class HostedService: IHostedService {
    private readonly IEventStoreConnection esConnection;

    public HostedService(IEventStoreConnection connection) {
        esConnection = connection;
    }

    public Task StartAsync(CancellationToken cancellationToken) =>
        esConnection.ConnectAsync();
    

    public Task StopAsync(CancellationToken cancellationToken) {
        esConnection.Close();
        return Task.CompletedTask;
    }
}
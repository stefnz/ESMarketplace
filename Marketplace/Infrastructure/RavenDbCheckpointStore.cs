using ES.Framework;
using EventStore.ClientAPI;
using Raven.Client.Documents.Session;

namespace Marketplace.Infrastructure;

public class RavenDbCheckpointStore: ICheckpointStore
{
    private readonly Func<IAsyncDocumentSession> getSession;
    private readonly string checkpointName;

    public RavenDbCheckpointStore(Func<IAsyncDocumentSession> getSession, string checkpointName) {
        this.getSession = getSession;
        this.checkpointName = checkpointName;
    }

    public async Task<Position> GetCheckpoint() {
        using var session = getSession();
        var checkpoint = await session.LoadAsync<Checkpoint>(checkpointName);

        return checkpoint?.Position ?? Position.Start;
    }

    public async Task StoreCheckpoint(Position position) {
        using var session = getSession();

        var checkpoint = await session.LoadAsync<Checkpoint>(checkpointName);

        if (checkpoint == null) {
            checkpoint = new Checkpoint { Id = checkpointName };
            await session.StoreAsync(checkpoint);
        }

        checkpoint.Position = position;
        await session.SaveChangesAsync();
    }
}
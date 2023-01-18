using EventStore.ClientAPI; // Note: currently using the legacy TCP client, rather than the preferred gRPC client

namespace ES.Framework;

public interface ICheckpointStore {
    
    // Note: use of Position forces dependency on EventStore library
    // consider wrapping
    Task<Position> GetCheckpoint();
    Task StoreCheckpoint(Position checkpoint);
}
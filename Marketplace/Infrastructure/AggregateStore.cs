using System.Text;
using ES.Framework;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Marketplace.Infrastructure; 

public class AggregateStore: IAggregateStore {
    private const int MaximumAllowedStreamSliceSize = 4096;
    private readonly IEventStoreConnection connection;

    public AggregateStore(IEventStoreConnection connection) {
        this.connection = connection;
    }

    private static string GetStreamName<T, TId>(TId aggregateId) where TId : IAggregateId
        => $"{typeof(T).Name}-{aggregateId.ToString()}";

    private static string GetStreamName<T, TId>(T aggregate) where T : Aggregate<TId> where TId : IAggregateId
        => $"{typeof(T).Name}-{aggregate.Id.ToString()}";

    private static byte[] Serialize(object data) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

    private class EventInfo {
        public string ClrType { get; set; }
    }

    public async Task<bool> Exists<T, TId>(TId aggregateId) where T:Aggregate<TId> where TId: IAggregateId {
        var stream = GetStreamName<T, TId>(aggregateId);
        var result = await connection.ReadEventAsync(stream, 1, false);
        return result.Status != EventReadStatus.NoStream;
    }

    public async Task Save<T, TId>(T aggregate) where T: Aggregate<TId> where TId: IAggregateId {
        if (aggregate == null) {
            throw new ArgumentNullException(nameof(aggregate));
        }

        var changes = aggregate.GetChanges().ToArray();

        if (!changes.Any()) {
            return;
        }

        var streamName = GetStreamName<T, TId>(aggregate);

        await connection.AppendEvents(streamName, aggregate.Version, changes);
        
        aggregate.ClearChanges();
    }

    public async Task<T> Load<T, TId>(TId aggregateId) where T : Aggregate<TId> where TId : IAggregateId {
        if (aggregateId == null) {
            throw new ArgumentNullException(nameof(aggregateId));
        }

        var stream = GetStreamName<T, TId>(aggregateId);
        var aggregate = (T)Activator.CreateInstance(typeof(T), true);

        var page = await connection.ReadStreamEventsForwardAsync(stream, 0, MaximumAllowedStreamSliceSize, false);
        // For large numbers of events, paging would be required. The max stream slice supported is 4096, after this paging is needed.
        
        aggregate.Load(page.Events.Select(resolvedEvent => resolvedEvent.Deserialize()).ToArray());

        return aggregate;
    }
}
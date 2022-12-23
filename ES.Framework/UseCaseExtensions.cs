namespace ES.Framework;

public static class UseCaseExtensions {
    public static async Task HandleUpdate<T, TId>(
        this IUseCases service,
        IAggregateStore store, TId aggregateId,
        Action<T> operation) where T : Aggregate<TId> where TId : IAggregateId {
        
        var aggregate = await store.Load<T, TId>(aggregateId);
        
        if (aggregate == null) {
            throw new InvalidOperationException($"Entity with id {aggregateId.ToString()} cannot be found");
        }

        operation(aggregate);
        await store.Save<T, TId>(aggregate);
    }
}
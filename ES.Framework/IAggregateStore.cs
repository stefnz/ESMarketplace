namespace ES.Framework; 

public interface IAggregateStore {
    Task<bool> Exists<T, TId>(TId aggregateId)  where T : Aggregate<TId> where TId: IAggregateId;
    Task Save<T, TId>(T aggregate) where T : Aggregate<TId> where TId: IAggregateId;
    Task<T> Load<T, TId>(TId aggregateId) where T : Aggregate<TId> where TId: IAggregateId;
}
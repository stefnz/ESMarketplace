namespace ES.Framework; 

public interface IAggregateStore {
    Task<T> Load<T>(string id) where T : Aggregate;
    Task Save<T>(T aggregate) where T : Aggregate;
}
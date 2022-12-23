namespace ES.Framework; 

public interface IAggregateId {
    public Guid Value { get; }
    public string ToString(); // avoid boxing
}
namespace ES.Framework;

/// <summary>
/// An aggregate from Domain Driven Design, a domain concept with a unique identity and lifetime.
/// </summary>
public abstract class Aggregate<TId>: IHandleEvents where TId: IAggregateId {
    public TId Id { get; protected set; }
    
    private readonly List<object> events;
    
    // List of events that have occurred in this entities history
    protected Aggregate() => events = new List<object>();

    public IEnumerable<object> GetChanges() => events.AsEnumerable();

    public void ClearChanges() => events.Clear();

    /// <summary>
    /// Applies an event to the aggregate, mutating the current state
    /// The event, if valid, is added to the history of events applied to the aggregate
    /// </summary>
    /// <param name="event">The event (immutable fact) to record</param>
    protected void Apply(object @event) {
        When(@event);
        EnsureValidState();
        events.Add(@event);
    }

    /// <summary>
    /// Called when an event is applied. The aggregate implements the required changes to the current state dictated by the event
    /// </summary>
    /// <param name="event"></param>
    protected abstract void When(object @event);

    /// <summary>
    /// Ensures the entity is in a valid state.
    /// </summary>
    protected abstract void EnsureValidState();

    protected void ApplyToEntity(IHandleEvents entity, object @event) => entity?.Handle(@event);

    void IHandleEvents.Handle(object @event) => When(@event);
}
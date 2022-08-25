using System.Diagnostics.CodeAnalysis;

namespace ES.Framework;

/// <summary>
/// An aggregate from Domain Driven Design, a domain concept with a unique identity and lifetime.
/// </summary>
public abstract class Aggregate {
    private readonly List<object> events;
    
    // List of events that have occurred in this entities history
    protected Aggregate() => events = new List<object>();

    public IEnumerable<object> GetChanges() => events.AsEnumerable();

    public void ClearChanges() => events.Clear();

    /// <summary>
    /// 
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
}
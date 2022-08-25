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

    protected abstract void When(object @event);

    protected abstract void EnsureValidState();
}
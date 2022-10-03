namespace ES.Framework;

/// <summary>
/// An entity is an object with a unique Id within the aggregate boundary.
/// An entity cannot be referenced directly outside of the aggregate boundary.
/// The aggregate root must provide methods to access and work with entities.
///
/// The owning aggregate will provide an applier action via a delegate.
/// The double dispatch pattern is used to inform the aggregate root of events
/// that the entity will be producing. This allows the aggregate root to handle events
/// from its child entities and so that it can ensure it is in a valid state. 
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class Entity<TId> : IHandleEvents {
    private readonly Action<object> _applier;

    public TId Id { get; protected set; }

    protected Entity(Action<object> applier) => _applier = applier;

    protected abstract void When(object @event);

    protected void Apply(object @event) {
        When(@event);
        _applier(@event);
    }

    void IHandleEvents.Handle(object @event) => When(@event);
}
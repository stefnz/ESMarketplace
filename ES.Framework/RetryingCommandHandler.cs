namespace ES.Framework; 

// See Polly: http://www.thepollyproject.org/

public class RetryingCommandHandler<T>: ICommandHandler<T> {
    static RetryPolicy policy = Policy
        .Handle<InvalidOperationException>()
        .RetryingCommandHandler();

    private ICommandHandler<T> next;

    public RetryingCommandHandler(ICommandHandler<T> next) => this.next = next;

    public Task Handle(T command) => policy.ExecuteAsync(() => this.next.Handle(command));
}
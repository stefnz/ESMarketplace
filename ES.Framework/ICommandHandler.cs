namespace ES.Framework; 

public interface ICommandHandler<in T> {
    Task Handle(T command);
}
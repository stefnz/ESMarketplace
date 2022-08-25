namespace ES.Framework; 

public interface IUseCases {
    Task Handle(object command);
}
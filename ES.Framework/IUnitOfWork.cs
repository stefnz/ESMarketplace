namespace ES.Framework; 

public interface IUnitOfWork {
    Task Commit();
}
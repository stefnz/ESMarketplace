namespace ES.Framework;

public interface IProjection {
    Task Project(object @event);
}
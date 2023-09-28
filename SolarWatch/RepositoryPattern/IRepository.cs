namespace SolarWatch.RepositoryPattern;

public interface IRepository<T> where T : EntityBase
{
    T GetByID(int id);
    void Add(T entity);
    // void Delete(T entity);
    // void Update(T entity);
}
public abstract class EntityBase
{
    public int Id { get; protected set; }
}
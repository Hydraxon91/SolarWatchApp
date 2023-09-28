using SolarWatch.Model;

namespace SolarWatch.RepositoryPattern;

public interface ICityRepository
{
    City? GetById(int id);
    City? GetByName(string name);
    int Add(City city);
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
    }
}
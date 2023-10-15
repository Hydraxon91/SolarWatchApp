using SolarWatch.RepositoryPattern;

namespace SolarWatch.Model;

public class CityName : EntityBase
{
    public int Id { get; init; }
    public string Name { get; init; }
}
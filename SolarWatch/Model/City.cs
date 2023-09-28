using SolarWatch.RepositoryPattern;

namespace SolarWatch.Model;

public class City : EntityBase
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Latitude { get; init; }
    public string Longitude { get; init; }
    public string? State { get; init; }
    public string Country { get; init; }
}
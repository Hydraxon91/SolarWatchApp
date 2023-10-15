namespace SolarWatch.RepositoryPattern;

public interface ICityNameRepository
{
    IEnumerable<string> GetAllCityNames();
    Task AddCityNameAsync(string cityName);
    Task AddRangeAsync(IEnumerable<string> cityNames);
}
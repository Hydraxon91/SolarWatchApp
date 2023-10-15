using SolarWatch.Model;

namespace SolarWatch.RepositoryPattern;

public interface ICityNameRepository
{
    IEnumerable<CityName> GetAllCityNames();
    IEnumerable<string> GetAllCityNameStrings();
    Task AddCityNameAsync(string cityName);
    Task AddRangeAsync(IEnumerable<string> cityNames);
}
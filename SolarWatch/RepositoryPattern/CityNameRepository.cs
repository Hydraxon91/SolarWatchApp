using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.RepositoryPattern;

public class CityNameRepository : ICityNameRepository
{
    private readonly SolarWatchContext _context;

    public CityNameRepository(SolarWatchContext context)
    {
        _context = context;
    }

    public IEnumerable<CityName> GetAllCityNames()
    {
        return _context.CityNames.ToList();
    }
    
    public IEnumerable<string> GetAllCityNameStrings()
    {
        return _context.CityNames.Select(city => city.Name).ToList();
    }

    public async Task AddCityNameAsync(string cityName)
    {
        var city = new CityName() { Name = cityName };
        await _context.CityNames.AddAsync(city);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<string> cityNames)
    {
        var cities = cityNames.Select(cityName => new CityName { Name = cityName });
        await _context.CityNames.AddRangeAsync(cities);
        await _context.SaveChangesAsync();
    }
}
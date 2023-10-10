using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.RepositoryPattern;

public class CityRepository : ICityRepository
{
    private readonly SolarWatchContext _dbContext;
    public CityRepository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }
    public City? GetById(int id)
    {
        return _dbContext.Cities.FirstOrDefault(item => item.Id == id);
    }
    
    public City? GetByName(string name)
    {
        return _dbContext.Cities.FirstOrDefault(item => item.Name == name);
    }
    

    public int Add(City city)
    {
        _dbContext.Add(city);
        _dbContext.SaveChanges();
        return city.Id;
    }

    public void AddSunriseSunsetToCity(City city, SunriseSunset sunriseSunset)
    {
        city.SunriseSunsets.Add(sunriseSunset);
        _dbContext.SaveChanges();
    }

    public bool DeleteByName(string name)
    {
        var cityToDelete = _dbContext.Cities.FirstOrDefault(c => c.Name == name);

        if (cityToDelete != null)
        {
            _dbContext.Cities.Remove(cityToDelete);
            _dbContext.SaveChanges();
            return true;
        }

        return false;
    }
}
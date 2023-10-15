using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Context;
using SolarWatch.Model;
using SolarWatch.RepositoryPattern;
using SolarWatch.Service;
using WeatherApi.Service;

namespace SolarWatch.Controllers;

[EnableCors("AllowLocalhost")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ILongitudeAndLatitudeProvider _longitudeAndLatitudeProvider;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;
    private readonly ICityNameRepository _cityNameRepository;
 
    public SolarWatchController(ILogger<SolarWatchController> logger, ILongitudeAndLatitudeProvider longitudeAndLatitudeProvider,
        IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider, 
        ICityRepository cityRepository, ISunriseSunsetRepository sunriseSunsetRepository,
        ICityNameRepository cityNameRepository)
    {
        _logger = logger;
        _longitudeAndLatitudeProvider = longitudeAndLatitudeProvider;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
        _cityRepository = cityRepository;
        _sunriseSunsetRepository = sunriseSunsetRepository;
        _cityNameRepository = cityNameRepository;
    }
    
    [HttpGet("GetSunriseSunset"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<SunriseSunset>> GetSunrise([Required] string city, [Required] DateTime date)
    {
        Console.WriteLine($"GetSunriseSunset Running: {city}");
        
        var dbCity = _cityRepository.GetByName(city);
        if (dbCity == null)
        {
            _logger.LogInformation("City not found, using API to find it");
            return await GetDataIfNoCity(city, date);
        }

        var dbSunriseSunsetData = _sunriseSunsetRepository.GetByIdAndDate(dbCity.Id, date);

        if (dbSunriseSunsetData == null)
        {
            _logger.LogInformation("SolarWatchData not found, using API to find it");
            return await GetDataIfSolarDataNotFound(dbCity, date);
        }

        Console.WriteLine("------------------------------");
        Console.WriteLine(dbSunriseSunsetData.City.Name);
        return Ok(dbSunriseSunsetData);
    }

    private async Task<ActionResult<SunriseSunset>> GetDataIfNoCity(string city, DateTime date)
    {
        try
        {
            var longLatData = await _longitudeAndLatitudeProvider.GetCurrent(city);
            var longLatProcessed = _jsonProcessor.ProcessLongLat(longLatData);
            try
            {
                var sunriseData = await _sunriseSunsetProvider.GetOnDate(longLatProcessed, date);
                var processCity = _jsonProcessor.ProcessCity(longLatData);
                var cityId = _cityRepository.Add(processCity);
                var sunriseSunset =
                    _jsonProcessor.ProcessSunriseSunset(sunriseData, date, cityId, processCity);
                _sunriseSunsetRepository.Add(sunriseSunset);
                //_cityRepository.AddSunriseSunsetToCity(processCity ,sunriseSunset);
                return Ok(sunriseSunset);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting SunriseSunset data");
                return NotFound("Error getting SunriseSunset data");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting LongLat data");
            return NotFound("Error getting LongLat data");
        }   
    }

    private async Task<ActionResult<SunriseSunset>> GetDataIfSolarDataNotFound(City dbCity, DateTime date)
    {
        try
        {
            Tuple<string, string> dbCityLongLat = new Tuple<string, string>(dbCity.Longitude, dbCity.Latitude);
            var sunriseData = await _sunriseSunsetProvider.GetOnDate(dbCityLongLat, date);
            var sunriseSunset =
                _jsonProcessor.ProcessSunriseSunset(sunriseData, date, dbCity.Id, dbCity);
            _sunriseSunsetRepository.Add(sunriseSunset);
            //_cityRepository.AddSunriseSunsetToCity(dbCity, sunriseSunset);
            return Ok(sunriseSunset);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting SunriseSunset data in already existing city");
            return NotFound("Error getting SunriseSunset data in already existing city");
        }
    }

    [HttpGet("Secret")]
    public ActionResult<SunriseSunset> GetTest()
    {
        return Ok("sunriseSunset");
    }

    [HttpDelete("DeleteCityByName"), Authorize(Roles = "Admin")]
    public ActionResult DeleteCityByName([Required] string name)
    {
        var result = _cityRepository.DeleteByName(name);
        if (result)
        {
            return Ok($"Successfully deleted {name}");
        }

        return NotFound($"cannot find city by name: {name}");
    }

    [HttpPatch("UpdateCityDateSunriseSunset"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SunriseSunset>> UpdateCityDateSunriseSunset([Required] string city,
        [Required] DateTime oldDate, [Required] DateTime newDate)
    {
        var dbCity = _cityRepository.GetByName(city);
        if (dbCity == null)
        {
            _logger.LogInformation("City not found, using API to find it");
            return NotFound($"cannot find city by name: {city}");
        }

        var oldDbSunriseSunsetData = _sunriseSunsetRepository.GetByIdAndDate(dbCity.Id, oldDate);

        if (oldDbSunriseSunsetData == null)
        {
            _logger.LogInformation("SolarWatchData not found, using API to find it");
            return NotFound($"found city by name: {city}, but it has no date: {oldDate}");
        }

        
        
        var newSunriseSunsetData = await _sunriseSunsetProvider.GetOnDate(new Tuple<string, string>(dbCity.Longitude, dbCity.Latitude), newDate);
        var newSunriseSunset =
            _jsonProcessor.ProcessSunriseSunset(newSunriseSunsetData, newDate, dbCity.Id, dbCity);
        
        _sunriseSunsetRepository.Update(oldDbSunriseSunsetData, newSunriseSunset);

        return Ok(oldDbSunriseSunsetData);
    }

    private async Task<ActionResult<SunriseSunset>> GetSunriseSunset(City dbCity, DateTime date)
    {
        Tuple<string, string> dbCityLongLat = new Tuple<string, string>(dbCity.Longitude, dbCity.Latitude);
        var sunriseData = await _sunriseSunsetProvider.GetOnDate(dbCityLongLat, date);
        var sunriseSunset =
            _jsonProcessor.ProcessSunriseSunset(sunriseData, date, dbCity.Id, dbCity);

        return sunriseSunset;
    }
    
    [HttpGet("GetClosestCity"), Authorize(Roles = "Admin, User")]
    public  ActionResult<CityName>  GetClosestCity([Required] string searchString)
    {
        Console.WriteLine($"GetClosestCity Running: {searchString}");

        // Retrieve a list of all city names from the repository
        var cityNames = _cityNameRepository.GetAllCityNames();

        // Find the city name that best matches the search string
        var closestCity = cityNames.FirstOrDefault(city => city.Name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));

        if (closestCity !=null)
        {
            return Ok(closestCity);
        }

        return NotFound($"cannot find city starting with: {searchString}");
    }
    

}
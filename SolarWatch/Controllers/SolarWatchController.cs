using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Context;
using SolarWatch.Model;
using SolarWatch.RepositoryPattern;
using SolarWatch.Service;
using WeatherApi.Service;

namespace SolarWatch.Controllers;

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ILongitudeAndLatitudeProvider _longitudeAndLatitudeProvider;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

 
    public SolarWatchController(ILogger<SolarWatchController> logger, ILongitudeAndLatitudeProvider longitudeAndLatitudeProvider,
        IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider, ICityRepository cityRepository, ISunriseSunsetRepository sunriseSunsetRepository)
    {
        _logger = logger;
        _longitudeAndLatitudeProvider = longitudeAndLatitudeProvider;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
        _cityRepository = cityRepository;
        _sunriseSunsetRepository = sunriseSunsetRepository;
    }
    
    [HttpGet("GetSunriseSunset"), Authorize]
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
                    _jsonProcessor.ProcessSunriseSunset(sunriseData, date, cityId);
                _sunriseSunsetRepository.Add(sunriseSunset);
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
                _jsonProcessor.ProcessSunriseSunset(sunriseData, date, dbCity.Id);
            _sunriseSunsetRepository.Add(sunriseSunset);
            return Ok(sunriseSunset);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting SunriseSunset data in already existing city");
            return NotFound("Error getting SunriseSunset data in already existing city");
        }
    }

    [HttpGet("Test"), Authorize]
    public  ActionResult<SunriseSunset> GetTest()
    {
        return Ok("sunriseSunset");
    }
    
}
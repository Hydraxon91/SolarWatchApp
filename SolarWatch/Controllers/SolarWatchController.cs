using System.ComponentModel.DataAnnotations;
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

    // public SolarWatchController(ILogger<SolarWatchController> logger, ILongitudeAndLatitudeProvider longitudeAndLatitudeProvider,
    //     IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider,
    //     ICityRepository citySunriseSunsetRepository, ISunriseSunsetRepository sunriseSunsetRepository)
    // {
    //     _logger = logger;
    //     _longitudeAndLatitudeProvider = longitudeAndLatitudeProvider;
    //     _jsonProcessor = jsonProcessor;
    //     _sunriseSunsetProvider = sunriseSunsetProvider;
    //     _cityRepository = citySunriseSunsetRepository;
    //     _sunriseSunsetRepository = sunriseSunsetRepository;
    // }
    
    public SolarWatchController(ILogger<SolarWatchController> logger, ILongitudeAndLatitudeProvider longitudeAndLatitudeProvider,
        IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider)
    {
        var dbContext = new SolarWatchContext();
        _logger = logger;
        _longitudeAndLatitudeProvider = longitudeAndLatitudeProvider;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
        _cityRepository = new CityRepository(dbContext);
        _sunriseSunsetRepository = new SunriseSunsetRepository(dbContext);
    }
    
    // [HttpGet("TestLatLongApiCall")]
    // public ActionResult<Tuple<string, string>> Get([Required] string city)
    // {
    //     try
    //     {
    //         var longLatData = _longitudeAndLatitudeProvider.GetCurrent(city);
    //         return Ok(_jsonProcessor.ProcessLongLat(longLatData));
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, "Error getting LongLat data");
    //         return NotFound("Error getting LongLat data");
    //     }  
    // }

    // [HttpGet("GetSunrise")]
    // public async Task<ActionResult<SolarWatch>> GetSunrise([Required] string city, [Required] DateTime date)
    // {
    //     try
    //     {
    //         var longLatData = await _longitudeAndLatitudeProvider.GetCurrent(city);
    //         var longLatProcessed = _jsonProcessor.ProcessLongLat(longLatData);
    //         try
    //         {
    //             var sunriseData = await _sunriseSunsetProvider.GetOnDate(longLatProcessed, date);
    //             return Ok(_jsonProcessor.ProcessSunrise(sunriseData));
    //         }
    //         catch (Exception e)
    //         {
    //             _logger.LogError(e, "Error getting Sunrise data");
    //             return NotFound("Error getting Sunrise data");
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, "Error getting LongLat data");
    //         return NotFound("Error getting LongLat data");
    //     }  
    // }
    //
    // [HttpGet("GetSunset")]
    // public async Task<ActionResult<SolarWatch>> GetSunset([Required] string city, [Required] DateTime date)
    // {
    //     try
    //     {
    //         var longLatData = await _longitudeAndLatitudeProvider.GetCurrent(city);
    //         var longLatProcessed = _jsonProcessor.ProcessLongLat(longLatData);
    //         try
    //         {
    //             var sunsetData = await _sunriseSunsetProvider.GetOnDate(longLatProcessed, date);
    //             return Ok(_jsonProcessor.ProcessSunrise(sunsetData));
    //         }
    //         catch (Exception e)
    //         {
    //             _logger.LogError(e, "Error getting Sunrise data");
    //             return NotFound("Error getting Sunrise data");
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, "Error getting LongLat data");
    //         return NotFound("Error getting LongLat data");
    //     }  
    // }
    
    [HttpGet("GetSunriseSunset")]
    public async Task<ActionResult<SunriseSunset>> GetSunrise([Required] string city, [Required] DateTime date)
    {
        await using var dbContext = new SolarWatchContext();
        
        var dbCity = _cityRepository.GetByName(city);
        if (dbCity == null)
        {
            _logger.LogInformation("City not found, using API to find it");
            return await GetDataIfNoCity(city, date);
        }

        var dbSunriseSunsetData = _sunriseSunsetRepository.GetByIdAndDate(dbCity.Id, date);
        // return dbSunriseSunsetData != null ? dbSunriseSunsetData : NotFound("Error getting LongLat data, something went wrong");
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
            // return Ok(_jsonProcessor.ProcessSunrise(sunriseData));
            return Ok(sunriseSunset);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting SunriseSunset data in already existing city");
            return NotFound("Error getting SunriseSunset data in already existing city");
        }
    }
    
}
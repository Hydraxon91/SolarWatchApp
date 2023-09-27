using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Service;
using WeatherApi.Service;

namespace SolarWatch.Controllers;

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ILongitudeAndLatitudeProvider _longitudeAndLatitudeProvider;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;
    private readonly IJsonProcessor _jsonProcessor;
    
    public SolarWatchController(ILogger<SolarWatchController> logger, ILongitudeAndLatitudeProvider longitudeAndLatitudeProvider,
        IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider)
    {
        _logger = logger;
        _longitudeAndLatitudeProvider = longitudeAndLatitudeProvider;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
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

    [HttpGet("GetSunrise")]
    public async Task<ActionResult<SolarWatch>> GetSunrise([Required] string city, [Required] DateTime date)
    {
        try
        {
            var longLatData = await _longitudeAndLatitudeProvider.GetCurrent(city);
            var longLatProcessed = _jsonProcessor.ProcessLongLat(longLatData);
            try
            {
                var sunriseData = await _sunriseSunsetProvider.GetOnDate(longLatProcessed, date);
                return Ok(_jsonProcessor.ProcessSunrise(sunriseData));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Sunrise data");
                return NotFound("Error getting Sunrise data");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting LongLat data");
            return NotFound("Error getting LongLat data");
        }  
    }
    
    [HttpGet("GetSunset")]
    public async Task<ActionResult<SolarWatch>> GetSunset([Required] string city, [Required] DateTime date)
    {
        try
        {
            var longLatData = await _longitudeAndLatitudeProvider.GetCurrent(city);
            var longLatProcessed = _jsonProcessor.ProcessLongLat(longLatData);
            try
            {
                var sunsetData = await _sunriseSunsetProvider.GetOnDate(longLatProcessed, date);
                return Ok(_jsonProcessor.ProcessSunrise(sunsetData));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Sunrise data");
                return NotFound("Error getting Sunrise data");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting LongLat data");
            return NotFound("Error getting LongLat data");
        }  
    }
}
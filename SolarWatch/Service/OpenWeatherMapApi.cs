using System.Net;
using System.Text.Json;

namespace WeatherApi.Service;

public class OpenWeatherMapApi : ILongitudeAndLatitudeProvider
{
    private readonly ILogger<OpenWeatherMapApi> _logger;
    
    public OpenWeatherMapApi(ILogger<OpenWeatherMapApi> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetCurrent(string cityName)
    {
        var apiKey = "69883ee2fa7a4b6cec1f2856c0622922";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={apiKey}&limit=1";

        var client = new HttpClient();

        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
    
}
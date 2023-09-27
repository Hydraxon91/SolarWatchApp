using System.Net;
using WeatherApi.Service;

namespace SolarWatch.Service;

public class SunriseSunsetApi : ISunriseSunsetProvider
{
    private readonly ILogger<SunriseSunsetApi> _logger;
    
    public SunriseSunsetApi(ILogger<SunriseSunsetApi> logger)
    {
        _logger = logger;
    }
    public async Task<string> GetCurrent(Tuple<string, string> lanLong)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lanLong.Item1}lng=-{lanLong.Item2}";
        
        var client = new HttpClient();

        _logger.LogInformation("Calling SunriseSunset API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> GetOnDate(Tuple<string, string> lanLong, DateTime date)
    {
        Console.WriteLine(date);
        var url = $"https://api.sunrise-sunset.org/json?lat={lanLong.Item1}lng=-{lanLong.Item2}&date={date:yyyy-MM-dd}";
        
        var client = new HttpClient();

        _logger.LogInformation("Calling SunriseSunset API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}
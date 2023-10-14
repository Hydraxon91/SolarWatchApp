using System.Text.Json;
using SolarWatch.Model;
using SolarWatch.Service;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{

    public Tuple<string, string> ProcessLongLat(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        string lat = json.RootElement[0].GetProperty("lat").GetDouble().ToString();
        string lon = json.RootElement[0].GetProperty("lon").GetDouble().ToString();
        return new Tuple<string, string>(lat, lon);
    }

    //SolarWatch5
    public City ProcessCity(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        var element = json.RootElement[0];
        var name = element.GetProperty("name").ToString();
        var lat = element.GetProperty("lat").ToString();
        var lon = element.GetProperty("lon").ToString();
        var country = element.GetProperty("country").ToString();
        string? state = element.TryGetProperty("state", out var stateProperty) ? stateProperty.ToString() : null;
        
        return new City
        {
            Name = name,
            Latitude = lat,
            Longitude = lon,
            Country = country,
            State = state,
        };
    }

    public SunriseSunset ProcessSunriseSunset(string data, DateTime date, int cityId, City city)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        var sunRise = new SunriseSunset
        {
            CityId = cityId,
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString(),
            SolarNoon = results.GetProperty("solar_noon").GetString(),
            Date = date,
            City = city
        };
        Console.WriteLine(sunRise.City.Name);
        return sunRise;
    }

public SolarWatch ProcessSunrise(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        return new SolarWatch
        {
            SolarNoon = results.GetProperty("solar_noon").GetString(),
            DayLength = results.GetProperty("day_length").GetString(),
            Time = results.GetProperty("sunrise").GetString()
        };
    }
    public SolarWatch ProcessSunset(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        return new SolarWatch
        {
            SolarNoon = results.GetProperty("solar_noon").GetString(),
            DayLength = results.GetProperty("day_length").GetString(),
            Time = results.GetProperty("sunset").GetString()
        };
    }

    private static DateTime GetDateTimeFromUnixTimeStamp(long timeStamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
        DateTime dateTime = dateTimeOffset.UtcDateTime;

        return dateTime;
    }
}
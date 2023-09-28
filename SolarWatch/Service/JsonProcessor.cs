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
    public City ProcessCity(string data, int id)
    {
        JsonDocument json = JsonDocument.Parse(data);
        var name = json.RootElement[0].GetProperty("name").GetDouble().ToString();
        var lat = json.RootElement[0].GetProperty("lat").GetDouble();
        var lon = json.RootElement[0].GetProperty("lon").GetDouble();
        var country = json.RootElement[0].GetProperty("country").GetDouble().ToString();
        string? state = json.RootElement[0].GetProperty("state").GetDouble().ToString();

        return new City
        {
            Id = id,
            Name = name,
            Latitude = lat,
            Longitude = lon,
            Country = country,
            State = state
        };
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
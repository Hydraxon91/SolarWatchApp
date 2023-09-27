using System.Text.Json;
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
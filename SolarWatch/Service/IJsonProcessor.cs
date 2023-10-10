using SolarWatch;
using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    Tuple<string, string> ProcessLongLat(string data);
    public City ProcessCity(string data);
    public SunriseSunset ProcessSunriseSunset(string data, DateTime date, int cityId, City city);
    SolarWatch ProcessSunset(string data);
    SolarWatch ProcessSunrise(string data);
}
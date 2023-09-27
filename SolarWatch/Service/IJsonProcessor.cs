using SolarWatch;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    Tuple<string, string> ProcessLongLat(string data);
    SolarWatch ProcessSunset(string data);
    SolarWatch ProcessSunrise(string data);
}
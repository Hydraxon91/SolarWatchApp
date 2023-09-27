namespace WeatherApi.Service;

public interface ILongitudeAndLatitudeProvider
{
    Task<string> GetCurrent(string cityName);
}
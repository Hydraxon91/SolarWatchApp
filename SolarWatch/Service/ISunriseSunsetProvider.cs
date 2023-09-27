namespace SolarWatch.Service;

public interface ISunriseSunsetProvider
{
    Task<string> GetCurrent(Tuple<string, string> lanLong);
    Task<string> GetOnDate(Tuple<string, string> lanLong, DateTime date);
}
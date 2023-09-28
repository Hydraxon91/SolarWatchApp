using System.Runtime.InteropServices.JavaScript;
using SolarWatch.RepositoryPattern;

namespace SolarWatch.Model;

public class SunriseSunset : EntityBase
{
    public int Id { get; init; }
    
    public int CityId { get; init; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    
    public DateTime Date { get; init; }
}
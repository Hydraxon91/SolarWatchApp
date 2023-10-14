using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using SolarWatch.RepositoryPattern;

namespace SolarWatch.Model;

public class SunriseSunset : EntityBase
{
    public int Id { get; init; }
    public int CityId { get; init; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public string SolarNoon { get; set; }
    public DateTime Date { get; set; }
    [JsonIgnore]
    public City City { get; set; }
}
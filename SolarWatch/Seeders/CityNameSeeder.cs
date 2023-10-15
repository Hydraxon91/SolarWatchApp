using System.Text.RegularExpressions;
using SolarWatch.RepositoryPattern;

namespace SolarWatch.Seeders;

public class CityNameSeeder
{
    private readonly ICityNameRepository _cityNameRepository;

    public CityNameSeeder(ICityNameRepository cityNameRepository)
    {
        _cityNameRepository = cityNameRepository;
    }

    public async Task SeedCityNamesAsync()
    {
        try
        {
            // Load city names from the JSON file (assuming it's in the same folder as your code)
            var cityNamesJson = await File.ReadAllTextAsync("Seeders/citynames.json");
            var cityNamesArray = cityNamesJson.Split(',');
            Console.WriteLine(cityNamesArray.Length);
            // Trim and remove any extra whitespace from the city names
            var cityNames = cityNamesArray.Select(cityName => cityName.Trim());

            // Add city names to the repository
            foreach (var cityName in cityNames)
            {
                var cleanedName = cityName.Trim('[', ']', ' ', '\n', '"');
                if (!_cityNameRepository.GetAllCityNameStrings().Contains(cleanedName))
                {
                    await _cityNameRepository.AddCityNameAsync(cleanedName);
                }
            }
            Console.WriteLine("City names seeded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding city names: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }
}
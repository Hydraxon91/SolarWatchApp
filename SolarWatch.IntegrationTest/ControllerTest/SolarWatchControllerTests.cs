using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SolarWatch.Model;
using WeatherApi.Contracts;


namespace SolarWatch.IntegrationTest.ControllerTest;

[TestFixture]
public class SolarWatchControllerTests : IDisposable
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;
    

    public SolarWatchControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }


    [Test]
    public async Task GetSunshineSunset_LoggedOut_Returns_401()
    {
        // var mockCities = new City[]
        // {
        //     new() { Id = 1, Name = "Budapest" },
        //     new() { Id = 2, Name = "London" }
        // };
        // var mockSunriseSuntest = new SunriseSunset[]
        // {
        //     new() {Id = 1, CityId = 1, Sunrise = "Sunrise", Sunset = "Sunset", Date = DateTime.Now}
        // };
        //
        // var mockCityRepo = new Mock<ICityRepository>();
        // mockCityRepo.Setup(r => r.GetByName("Budapest")).Returns(mockCities[0]);
        var city = "Budapest";
        var date = DateTime.Now;
        var queryString = $"?city={city}&date={date:yyyy-MM-dd}";

        var response = await _client.GetAsync($"/GetSunriseSunset{queryString}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
    [Test]
    public async Task GetSunshineSunset_LoggedIn_Returns_Ok()
    {
        var email = "admin@admin.com";
        var password = "admin123";
        var loginResponse = await _client.PostAsync("/Auth/Login", new StringContent(JsonConvert.SerializeObject(new { email, password }), Encoding.UTF8, "application/json"));
        loginResponse.EnsureSuccessStatusCode();
        
        var responseContent = await loginResponse.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
        var token = authResponse.Token;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var city = "Budapest";
        var date = DateTime.Now;
        var queryString = $"?city={city}&date={date:yyyy-MM-dd}";
        var response = await _client.GetAsync($"/GetSunriseSunset{queryString}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetSunshineSunset_With_Mocked_City_And_Sunrise_Results()
    {
        //setting up CityRepositoryMock
        var mockCity = new City[]
        {
            new() { Id = 0, Name = "Budapest" },
            new() { Id = 1, Name = "London" },
        };
        _factory.CityRepositoryMock.Setup(r => r.GetByName("Budapest")).Returns(mockCity[0]);
        //setting up SunriseSunsetRepositoryMock
        var mockSunriseSunsets = new SunriseSunset[]
        {
            new() {Id = 0, 
                Sunrise = "ShouldBeThis", 
                Sunset = "Budapest", 
                CityId = 0, 
                Date = DateTime.Today,
                SolarNoon = "Budapest",
                City = new City()
            },
            new() {Id = 1, Sunrise = "London", Sunset = "London", CityId = 1, Date = DateTime.Today, SolarNoon = "London", City = new City()},
        };
        _factory.SunriseSunsetRepositoryMock.Setup(r => r.GetByIdAndDate(0, DateTime.Today))
            .Returns(mockSunriseSunsets[0]);

        //Getting authentication --Needs db to run, I don't know how to do otherwise
        var email = "admin@admin.com";
        var password = "admin123";
        var loginResponse = await _client.PostAsync("/Auth/Login", new StringContent(JsonConvert.SerializeObject(new { email, password }), Encoding.UTF8, "application/json"));
        loginResponse.EnsureSuccessStatusCode();
        
        //Getting token out of response 
        var responseContent = await loginResponse.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
        var token = authResponse.Token;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        //Getting mock response
        var city = "Budapest";
        var date = DateTime.Now;
        var queryString = $"?city={city}&date={date:yyyy-MM-dd}";
        var response = await _client.GetAsync($"/GetSunriseSunset{queryString}");

        var data = JsonConvert.DeserializeObject<SunriseSunset>(await response.Content.ReadAsStringAsync());
        Assert.That(data.Sunrise, Is.EqualTo("ShouldBeThis"));
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
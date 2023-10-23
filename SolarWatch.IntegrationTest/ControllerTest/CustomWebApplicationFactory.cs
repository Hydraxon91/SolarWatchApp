using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SolarWatch.Data;
using SolarWatch.RepositoryPattern;

namespace SolarWatch.IntegrationTest.ControllerTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ISunriseSunsetRepository> SunriseSunsetRepositoryMock { get; }
    public Mock<ICityRepository> CityRepositoryMock { get; }
    
    
    public CustomWebApplicationFactory()
    {
        SunriseSunsetRepositoryMock = new Mock<ISunriseSunsetRepository>();
        CityRepositoryMock = new Mock<ICityRepository>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(SunriseSunsetRepositoryMock.Object);
            services.AddSingleton(CityRepositoryMock.Object);
        });
    }
}
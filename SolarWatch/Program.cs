using Microsoft.EntityFrameworkCore;
using SolarWatch.Context;
using SolarWatch.Model;
using SolarWatch.RepositoryPattern;
using SolarWatch.Service;
using WeatherApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SolarWatchContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SolarWatchDb")));;
builder.Services.AddTransient<ILongitudeAndLatitudeProvider, OpenWeatherMapApi>();
builder.Services.AddTransient<ISunriseSunsetProvider, SunriseSunsetApi>();
builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<ISunriseSunsetRepository, SunriseSunsetRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
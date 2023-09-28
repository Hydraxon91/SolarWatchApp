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

builder.Services.AddDbContext<SolarWatchContext>();
builder.Services.AddSingleton<ILongitudeAndLatitudeProvider, OpenWeatherMapApi>();
builder.Services.AddSingleton<ISunriseSunsetProvider, SunriseSunsetApi>();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
// builder.Services.AddSingleton<ICityRepository, CityRepository>();
// builder.Services.AddSingleton<ISunriseSunsetRepository, SunriseSunsetRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
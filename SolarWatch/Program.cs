using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Context;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.RepositoryPattern;
using SolarWatch.Service;
using SolarWatch.Service.Authentication;
using WeatherApi.Service;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

AddServices();
ConfigureSwagger();
AddDbContext();
AddAuthentication();
AddIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AddRoles();

app.Run();

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    
    builder.Services.AddTransient<ILongitudeAndLatitudeProvider, OpenWeatherMapApi>();
    builder.Services.AddTransient<ISunriseSunsetProvider, SunriseSunsetApi>();
    builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
    builder.Services.AddTransient<ICityRepository, CityRepository>();
    builder.Services.AddTransient<ISunriseSunsetRepository, SunriseSunsetRepository>();
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
}

void AddDbContext()
{
    builder.Services.AddDbContext<SolarWatchContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SolarWatchDb")));
    builder.Services.AddDbContext<UsersContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SolarWatchDb")));
}

void AddAuthentication()
{
    builder.Services.AddScoped<IAuthService, AuthService>();
    
    var validIssuer = builder.Configuration["Authentication:ValidIssuer"];
    var validAudience = builder.Configuration["Authentication:ValidAudience"];
    var issuerSigningKey = builder.Configuration["Authentication:IssuerSigningKey"];
    Console.WriteLine($"{validIssuer}, {validAudience}, {issuerSigningKey}");
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuerSigningKey)
                )
            };
        });
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<UsersContext>(); 
}

void AddRoles()
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var tAdmin = CreateAdminRole(roleManager);
    tAdmin.Wait();

    var tUser = CreateUserRole(roleManager);
    tUser.Wait();
}

async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}

async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("User"));
}
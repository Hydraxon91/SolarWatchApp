using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Context;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.RepositoryPattern;
using SolarWatch.Seeders;
using SolarWatch.Service;
using SolarWatch.Service.Authentication;
using WeatherApi.Service;

var  myAllowSpecificOrigins = "MyPolicy";
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

AddServices();
ConfigureSwagger();
AddDbContext();
AddAuthentication();
AddIdentity();

var app = builder.Build();

MigrateContexts();


var environment = builder.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)  // Load the default configuration
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);  // Load environment-specific configuration



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AddRoles();

AddAdmin();

AddCityNames();

app.Run();

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: myAllowSpecificOrigins, policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyHeader()  
                .AllowAnyMethod();  
        });
        
        options.AddPolicy("AllowLocalhost", policy =>
        {
            policy
                .WithOrigins("http://localhost:3000") // Allow requests from your client's origin
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
    
    builder.Services.AddTransient<ILongitudeAndLatitudeProvider, OpenWeatherMapApi>();
    builder.Services.AddTransient<ISunriseSunsetProvider, SunriseSunsetApi>();
    builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
    builder.Services.AddTransient<ICityRepository, CityRepository>();
    builder.Services.AddTransient<ISunriseSunsetRepository, SunriseSunsetRepository>();
    builder.Services.AddTransient<ICityNameRepository, CityNameRepository>();
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

void MigrateContexts()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var solarWatchContext = services.GetRequiredService<SolarWatchContext>();
        var usersContext = services.GetRequiredService<UsersContext>();
        if (solarWatchContext.Database.GetPendingMigrations().Any())
        {
            solarWatchContext.Database.Migrate();
        }
        if (usersContext.Database.GetPendingMigrations().Any())
        {
            usersContext.Database.Migrate();
        }
    }
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
    builder.Services.AddScoped<ITokenService, TokenService>();
    
    var validIssuer = builder.Configuration["AuthenticationSettings:ValidIssuer"];
    var validAudience = builder.Configuration["AuthenticationSettings:ValidAudience"];
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

async Task CreateAdminIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
    if (adminInDb == null)
    {
        var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
        var adminCreated = await userManager.CreateAsync(admin, "admin123");

        if (adminCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

void AddAdmin()
{
    var tAdmin = CreateAdminIfNotExists();
    tAdmin.Wait();
}

async Task SeedCityNames()
{
    try
    {
        using var scope = app.Services.CreateScope();
        var cityNameRepository = scope.ServiceProvider.GetRequiredService<ICityNameRepository>();
        var cityNameSeeder = new CityNameSeeder(cityNameRepository);
        await cityNameSeeder.SeedCityNamesAsync();
        Console.WriteLine("City names seeded successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding city names: {ex.Message}");
    }
}

void AddCityNames()
{
    var aCityNames = SeedCityNames();
    aCityNames.Wait();
}

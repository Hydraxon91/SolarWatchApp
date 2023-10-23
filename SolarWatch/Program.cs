using System;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Context;
using SolarWatch.Data;
using SolarWatch.RepositoryPattern;
using SolarWatch.Seeders;
using SolarWatch.Service;
using SolarWatch.Service.Authentication;
using WeatherApi.Service;

namespace SolarWatch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var myAllowSpecificOrigins = "MyPolicy";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AddServices(builder.Services, myAllowSpecificOrigins);
            ConfigureSwagger(builder.Services);
            AddDbContext(builder.Services, builder);
            AddAuthentication(builder);
            AddIdentity(builder.Services);

            var app = builder.Build();

            MigrateContexts(app.Services);

            var environment = builder.Environment;
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

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

            AddRoles(app);

            AddAdmin(app);

            AddCityNames(app.Services);

            app.Run();
        }

        private static void AddServices(IServiceCollection services, string myAllowSpecificOrigins)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            
            services.AddCors(options =>
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
            
            services.AddTransient<ILongitudeAndLatitudeProvider, OpenWeatherMapApi>();
            services.AddTransient<ISunriseSunsetProvider, SunriseSunsetApi>();
            services.AddTransient<IJsonProcessor, JsonProcessor>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ISunriseSunsetRepository, SunriseSunsetRepository>();
            services.AddTransient<ICityNameRepository, CityNameRepository>();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
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

    private static void MigrateContexts(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
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

    private static void AddDbContext(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<SolarWatchContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SolarWatchDb")));
        services.AddDbContext<UsersContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SolarWatchDb")));
    }

    private static void AddAuthentication(WebApplicationBuilder builder)
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

    private static void AddIdentity(IServiceCollection services)
    {
        services
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

        private static void AddRoles(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var tAdmin = CreateAdminRole(roleManager);
            tAdmin.Wait();

            var tUser = CreateUserRole(roleManager);
            tUser.Wait();
        }

        private static async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        private static async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        private static async Task CreateAdminIfNotExists(WebApplication app)
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

        private static void AddAdmin(WebApplication app)
        {
            var tAdmin = CreateAdminIfNotExists(app);
            tAdmin.Wait();
        }

        private static async Task SeedCityNames(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
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

        private static void AddCityNames(IServiceProvider serviceProvider)
        {
            var aCityNames = SeedCityNames(serviceProvider);
            aCityNames.Wait();
        }
    }
}

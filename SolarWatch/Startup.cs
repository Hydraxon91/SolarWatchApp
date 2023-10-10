using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Context;
using SolarWatch.RepositoryPattern;
using SolarWatch.Service;
using WeatherApi.Service;

namespace SolarWatch // Replace with your actual namespace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AddServices(services);
            ConfigureSwagger(services);
            AddDbContext(services);
            AddAuthentication(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        private static void AddServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddEndpointsApiExplorer();
            services.AddTransient<ILongitudeAndLatitudeProvider, OpenWeatherMapApi>();
            services.AddTransient<ISunriseSunsetProvider, SunriseSunsetApi>();
            services.AddTransient<IJsonProcessor, JsonProcessor>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ISunriseSunsetRepository, SunriseSunsetRepository>();
        }
        
        private void ConfigureSwagger(IServiceCollection services)
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

        private void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<SolarWatchContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SolarWatchDb")));
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var validIssuer = Configuration["Authentication:ValidIssuer"];
            var validAudience = Configuration["Authentication:ValidAudience"];

            var issuerSigningKey = Configuration["Authentication:IssuerSigningKey"];
            services
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
    }
}
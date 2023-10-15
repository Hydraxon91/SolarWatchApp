using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Context;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }
    public DbSet<CityName> CityNames { get; set; }
    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) :base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<City>()
            .HasIndex(c => c.Name)
            .IsUnique();
        
        builder.Entity<City>()
            .HasMany(city => city.SunriseSunsets)  
            .WithOne(sunriseSunset => sunriseSunset.City) 
            .HasForeignKey(sunriseSunset => sunriseSunset.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CityName>().HasIndex(c => c.Name).IsUnique();
    }
}